using Microsoft.Extensions.Logging;
using STranslate.Plugin.Tts.Vocu.View;
using STranslate.Plugin.Tts.Vocu.ViewModel;
using System.Text.Json;
using System.Windows.Controls;

namespace STranslate.Plugin.Tts.Vocu;

/// <summary>
/// vocu.ai / wusound 语音合成插件主入口
/// </summary>
public class Main : ITtsPlugin
{
    private Control? _settingUi;
    private SettingsViewModel? _viewModel;
    private Settings Settings { get; set; } = null!;
    private IPluginContext Context { get; set; } = null!;

    /// <summary>
    /// 待处理的用户信息拉取任务，在 Init 中启动（仅活跃站点）
    /// </summary>
    private Task<UserInfoResult?>? _pendingUserInfoTask;

    /// <summary>
    /// Init 阶段预拉取的站点类型
    /// </summary>
    private SiteType _pendingSiteType;

    public Control GetSettingUI()
    {
        if (_viewModel is null)
        {
            var vocuTask = _pendingSiteType == SiteType.Vocu ? _pendingUserInfoTask : null;
            var wusoundTask = _pendingSiteType == SiteType.Wusound ? _pendingUserInfoTask : null;
            _viewModel = new SettingsViewModel(Context, Settings, vocuTask, wusoundTask);
        }
        _settingUi ??= new SettingsView { DataContext = _viewModel };
        return _settingUi;
    }

    public void Init(IPluginContext context)
    {
        Context = context;
        Settings = context.LoadSettingStorage<Settings>();

        MigrateIfNeeded();

        // 插件加载时自动拉取活跃站点的用户信息（静默失败）
        var (site, siteType) = GetActiveSite();
        _pendingSiteType = siteType;

        if (!string.IsNullOrWhiteSpace(site.ApiKey))
        {
            var baseUrl = SiteInfo.GetApiBaseUrl(siteType);
            _pendingUserInfoTask = FetchUserInfoCoreAsync(baseUrl, site.ApiKey);
        }
    }

    public void Dispose() => _viewModel?.Dispose();

    /// <summary>
    /// 调用 TTS API 合成并播放语音（使用当前活跃站点的配置）
    /// </summary>
    public async Task PlayAudioAsync(string text, CancellationToken cancellationToken = default)
    {
        var (site, siteType) = GetActiveSite();
        var apiBaseUrl = SiteInfo.GetApiBaseUrl(siteType);

        if (string.IsNullOrWhiteSpace(site.ApiKey))
        {
            Context.Snackbar.ShowError(Context.GetTranslation("STranslate_Plugin_Tts_Vocu_ApiKey_Empty"));
            return;
        }

        if (string.IsNullOrWhiteSpace(site.VoiceId))
        {
            Context.Snackbar.ShowError(Context.GetTranslation("STranslate_Plugin_Tts_Vocu_VoiceId_Empty"));
            return;
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            Context.Snackbar.ShowWarning(Context.GetTranslation("STranslate_Plugin_Tts_Vocu_Text_Empty"));
            return;
        }

        try
        {
            var requestBody = new
            {
                voiceId = site.VoiceId,
                text,
                promptId = string.IsNullOrWhiteSpace(site.PromptId) ? "default" : site.PromptId,
                preset = site.Preset,
                break_clone = site.BreakClone,
                language = site.Language,
                vivid = site.Vivid,
                emo_switch = new[] { site.EmoAngry, site.EmoHappy, site.EmoNeutral, site.EmoSad, site.EmoContext },
                speechRate = site.SpeechRate,
                flash = site.Flash,
                seed = site.Seed,
                stream = false,
                srt = false
            };

            var option = new Options
            {
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Bearer {site.ApiKey}",
                    ["Content-Type"] = "application/json"
                }
            };

#if DEBUG
            // Debug: 将请求体发送到 httpbin.org 以验证 GUI→API 参数映射
            try
            {
                var debugJson = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions { WriteIndented = true });
                Context.Logger.LogWarning("[Vocu TTS] Request body:\n{Body}", debugJson);

                var httpbinResponse = await Context.HttpService.PostAsync(
                    "http://httpbin.org/post", requestBody, new Options(), cancellationToken);
                Context.Logger.LogWarning("[Vocu TTS] httpbin echo:\n{Response}", httpbinResponse);
            }
            catch (Exception debugEx)
            {
                Context.Logger.LogWarning("[Vocu TTS] httpbin failed: {Error}", debugEx.Message);
            }
#endif

            var url = $"{apiBaseUrl}/api/tts/simple-generate";
            var response = await Context.HttpService.PostAsync(url, requestBody, option, cancellationToken);

            using var doc = JsonDocument.Parse(response);
            var root = doc.RootElement;

            var status = root.GetProperty("status").GetInt32();
            if (status != 200)
            {
                var message = root.TryGetProperty("message", out var msg)
                    ? msg.GetString() ?? "Unknown error"
                    : "Unknown error";
                Context.Snackbar.ShowError(message);
                return;
            }

            var audioUrl = root.GetProperty("data").GetProperty("audio").GetString();
            if (string.IsNullOrEmpty(audioUrl))
            {
                Context.Snackbar.ShowWarning(Context.GetTranslation("STranslate_Plugin_Tts_Vocu_Audio_Empty"));
                return;
            }

            await Context.AudioPlayer.PlayAsync(audioUrl, cancellationToken);

            // TTS 成功后静默刷新用户信息（更新剩余点数）
            if (_viewModel is not null)
                _ = _viewModel.ActiveSiteViewModel.RefreshUserInfoSilentlyAsync();
        }
        catch (JsonException)
        {
            Context.Snackbar.ShowError(Context.GetTranslation("STranslate_Plugin_Tts_Vocu_Parse_Error"));
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            Context.Snackbar.ShowError(ex.Message);
        }
    }

    /// <summary>
    /// 获取当前活跃站点的配置和类型
    /// </summary>
    private (SiteSettings site, SiteType type) GetActiveSite()
    {
        return Settings.ActiveSiteIndex switch
        {
            1 => (Settings.Wusound, SiteType.Wusound),
            _ => (Settings.Vocu, SiteType.Vocu)
        };
    }

    /// <summary>
    /// 读取用户点数：credit + credits 之和
    /// </summary>
    private static double ReadCredit(JsonElement user)
    {
        double total = 0;
        if (user.TryGetProperty("credit", out var c) && c.ValueKind == JsonValueKind.Number)
            total += c.GetDouble();
        if (user.TryGetProperty("credits", out var cs) && cs.ValueKind == JsonValueKind.Number)
            total += cs.GetDouble();
        return total;
    }

    /// <summary>
    /// 拉取用户信息（供 Init 和 ViewModel 共用）
    /// </summary>
    internal async Task<UserInfoResult?> FetchUserInfoCoreAsync(string baseUrl, string apiKey)
    {
        try
        {
            var option = new Options
            {
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Bearer {apiKey}"
                }
            };

            var url = $"{baseUrl.TrimEnd('/')}/api/account/info";
            var response = await Context.HttpService.GetAsync(url, option, CancellationToken.None);

            using var doc = JsonDocument.Parse(response);
            var user = doc.RootElement.GetProperty("user");

            return new UserInfoResult(
                user.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String ? id.GetString() ?? "" : "",
                user.TryGetProperty("name", out var nm) && nm.ValueKind == JsonValueKind.String ? nm.GetString() ?? "" : "",
                user.TryGetProperty("email", out var em) && em.ValueKind == JsonValueKind.String ? em.GetString() ?? "" : "",
                user.TryGetProperty("avatar", out var av) ? av.GetString() ?? "" : "",
                ReadCredit(user)
            );
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// v1 扁平配置格式迁移到 v2 双站点嵌套格式
    /// </summary>
    private void MigrateIfNeeded()
    {
        if (Settings.ExtensionData is not { Count: > 0 } ext || !ext.ContainsKey("ApiKey"))
            return;

        // 判断旧配置属于哪个站点
        var targetIsWusound = ext.TryGetValue("Url", out var urlElem)
            && urlElem.GetString()?.Contains("wusound", StringComparison.OrdinalIgnoreCase) == true;

        var target = targetIsWusound ? Settings.Wusound : Settings.Vocu;
        Settings.ActiveSiteIndex = targetIsWusound ? 1 : 0;

        // 字符串字段
        if (ext.TryGetValue("ApiKey", out var v) && v.ValueKind == JsonValueKind.String)
            target.ApiKey = v.GetString() ?? "";
        if (ext.TryGetValue("VoiceId", out v) && v.ValueKind == JsonValueKind.String)
            target.VoiceId = v.GetString() ?? "";
        if (ext.TryGetValue("PromptId", out v) && v.ValueKind == JsonValueKind.String)
            target.PromptId = v.GetString() ?? "";
        if (ext.TryGetValue("Preset", out v) && v.ValueKind == JsonValueKind.String)
            target.Preset = v.GetString() ?? "balance";
        if (ext.TryGetValue("Language", out v) && v.ValueKind == JsonValueKind.String)
            target.Language = v.GetString() ?? "auto";

        // double 字段
        if (ext.TryGetValue("SpeechRate", out v) && v.ValueKind == JsonValueKind.Number)
            target.SpeechRate = v.GetDouble();

        // bool 字段
        if (ext.TryGetValue("BreakClone", out v) && (v.ValueKind == JsonValueKind.True || v.ValueKind == JsonValueKind.False))
            target.BreakClone = v.GetBoolean();
        if (ext.TryGetValue("Vivid", out v) && (v.ValueKind == JsonValueKind.True || v.ValueKind == JsonValueKind.False))
            target.Vivid = v.GetBoolean();
        if (ext.TryGetValue("Flash", out v) && (v.ValueKind == JsonValueKind.True || v.ValueKind == JsonValueKind.False))
            target.Flash = v.GetBoolean();

        // int 字段
        if (ext.TryGetValue("EmoAngry", out v) && v.ValueKind == JsonValueKind.Number)
            target.EmoAngry = v.GetInt32();
        if (ext.TryGetValue("EmoHappy", out v) && v.ValueKind == JsonValueKind.Number)
            target.EmoHappy = v.GetInt32();
        if (ext.TryGetValue("EmoNeutral", out v) && v.ValueKind == JsonValueKind.Number)
            target.EmoNeutral = v.GetInt32();
        if (ext.TryGetValue("EmoSad", out v) && v.ValueKind == JsonValueKind.Number)
            target.EmoSad = v.GetInt32();
        if (ext.TryGetValue("EmoContext", out v) && v.ValueKind == JsonValueKind.Number)
            target.EmoContext = v.GetInt32();
        if (ext.TryGetValue("Seed", out v) && v.ValueKind == JsonValueKind.Number)
            target.Seed = v.GetInt32();

        Settings.ExtensionData = null;
        Context.SaveSettingStorage<Settings>();
    }
}

/// <summary>
/// 用户信息查询结果
/// </summary>
public record UserInfoResult(string Id, string Name, string Email, string Avatar, double Credits);
