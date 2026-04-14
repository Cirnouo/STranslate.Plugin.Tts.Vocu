using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;
using System.Windows.Media;

namespace STranslate.Plugin.Tts.Vocu.ViewModel;

/// <summary>
/// 单站点的设置 ViewModel（Vocu / Wusound 各一个实例）
/// </summary>
public partial class SiteSettingsViewModel : ObservableObject, IDisposable
{
    private readonly IPluginContext _context;
    private readonly SiteSettings _siteSettings;

    // ── 延迟阈值（毫秒） ──
    private const long LatencyGoodMs = 300;
    private const long LatencyFairMs = 800;

    private static readonly SolidColorBrush BrushGood = new(Color.FromRgb(0x4C, 0xAF, 0x50)); // 绿
    private static readonly SolidColorBrush BrushFair = new(Color.FromRgb(0xFF, 0x98, 0x00)); // 黄
    private static readonly SolidColorBrush BrushPoor = new(Color.FromRgb(0xF4, 0x43, 0x36)); // 红

    // ── 站点标识（只读） ──

    public SiteType SiteType { get; }
    public string SiteApiBaseUrl { get; }
    public string SiteWebUrl { get; }
    public string SiteName { get; }

    // ── API 连接 ──

    [ObservableProperty]
    public partial string ApiKey { get; set; }

    // ── 用户信息（只读显示） ──

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(RefreshUserInfoCommand))]
    public partial bool IsLoadingUserInfo { get; set; }

    [ObservableProperty]
    public partial string UserName { get; set; }

    [ObservableProperty]
    public partial string UserCredits { get; set; }

    [ObservableProperty]
    public partial string? AvatarUrl { get; set; }

    // ── 延迟反馈 ──

    [ObservableProperty]
    public partial string LatencyText { get; set; }

    [ObservableProperty]
    public partial SolidColorBrush? LatencyBrush { get; set; }

    // ── 语音配置 ──

    [ObservableProperty]
    public partial string VoiceId { get; set; }

    [ObservableProperty]
    public partial string PromptId { get; set; }

    // ── 生成参数 ──

    [ObservableProperty]
    public partial string SelectedPreset { get; set; }

    [ObservableProperty]
    public partial string SelectedLanguage { get; set; }

    [ObservableProperty]
    public partial double SpeechRate { get; set; }

    // ── 高级选项 ──

    [ObservableProperty]
    public partial bool BreakClone { get; set; }

    [ObservableProperty]
    public partial bool Vivid { get; set; }

    [ObservableProperty]
    public partial bool Flash { get; set; }

    // ── 情绪控制 ──

    [ObservableProperty]
    public partial int EmoAngry { get; set; }

    [ObservableProperty]
    public partial int EmoHappy { get; set; }

    [ObservableProperty]
    public partial int EmoNeutral { get; set; }

    [ObservableProperty]
    public partial int EmoSad { get; set; }

    [ObservableProperty]
    public partial int EmoContext { get; set; }

    // ── 其他 ──

    [ObservableProperty]
    public partial string Seed { get; set; }

    // ── 静态选项列表 ──

    public static IReadOnlyList<string> Presets { get; } = ["creative", "balance", "stable"];

    public static IReadOnlyList<string> Languages { get; } =
        ["auto", "zh", "en", "ja", "ko", "fr", "es", "de", "pt", "yue"];

    // ── 构造 ──

    public SiteSettingsViewModel(
        IPluginContext context,
        SiteSettings siteSettings,
        SiteType siteType,
        Task<UserInfoResult?>? pendingUserInfoTask)
    {
        _context = context;
        _siteSettings = siteSettings;

        // 站点标识
        SiteType = siteType;
        SiteApiBaseUrl = SiteInfo.GetApiBaseUrl(siteType);
        SiteWebUrl = SiteInfo.GetWebUrl(siteType);
        SiteName = SiteInfo.GetDisplayName(siteType);

        // 从持久化配置加载
        ApiKey = siteSettings.ApiKey;
        VoiceId = siteSettings.VoiceId;
        PromptId = siteSettings.PromptId;
        SelectedPreset = siteSettings.Preset;
        SelectedLanguage = siteSettings.Language;
        SpeechRate = siteSettings.SpeechRate;
        BreakClone = siteSettings.BreakClone;
        Vivid = siteSettings.Vivid;
        Flash = siteSettings.Flash;
        EmoAngry = siteSettings.EmoAngry;
        EmoHappy = siteSettings.EmoHappy;
        EmoNeutral = siteSettings.EmoNeutral;
        EmoSad = siteSettings.EmoSad;
        EmoContext = siteSettings.EmoContext;
        Seed = siteSettings.Seed.ToString();

        // 用户信息默认值
        UserName = "";
        UserCredits = "";
        LatencyText = "";

        PropertyChanged += OnPropertyChanged;

        // 消费 Init 阶段启动的用户信息拉取任务
        if (pendingUserInfoTask is not null)
            _ = ApplyPendingUserInfoAsync(pendingUserInfoTask);
    }

    // ── 命令 ──

    private bool CanRefreshUserInfo => !IsLoadingUserInfo;

    [RelayCommand(CanExecute = nameof(CanRefreshUserInfo))]
    private async Task RefreshUserInfoAsync()
    {
        await FetchUserInfoAsync(showError: true, showLatency: true);
    }

    /// <summary>
    /// TTS 播放成功后调用，静默刷新用户信息（不显示延迟，仅错误时报错）
    /// </summary>
    internal async Task RefreshUserInfoSilentlyAsync()
    {
        await FetchUserInfoAsync(showError: false, showLatency: false);
    }

    // ── 用户信息拉取 ──

    private async Task ApplyPendingUserInfoAsync(Task<UserInfoResult?> task)
    {
        try
        {
            var result = await task;
            ApplyUserInfo(result);
        }
        catch
        {
            // 初始化阶段静默忽略
        }
    }

    private async Task FetchUserInfoAsync(bool showError, bool showLatency)
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            if (showError)
                _context.Snackbar.ShowError(_context.GetTranslation("STranslate_Plugin_Tts_Vocu_ApiKey_Empty"));
            return;
        }

        IsLoadingUserInfo = true;
        if (showLatency)
        {
            LatencyText = "";
            LatencyBrush = null;
        }

        var sw = Stopwatch.StartNew();
        try
        {
            var option = new Options
            {
                Headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Bearer {ApiKey}"
                }
            };

            var url = $"{SiteApiBaseUrl}/api/account/info";
            var response = await _context.HttpService.GetAsync(url, option, CancellationToken.None);
            sw.Stop();

            using var doc = JsonDocument.Parse(response);
            var user = doc.RootElement.GetProperty("user");

            ApplyUserInfo(new UserInfoResult(
                user.TryGetProperty("id", out var id) && id.ValueKind == JsonValueKind.String ? id.GetString() ?? "" : "",
                user.TryGetProperty("name", out var nm) && nm.ValueKind == JsonValueKind.String ? nm.GetString() ?? "" : "",
                user.TryGetProperty("email", out var em) && em.ValueKind == JsonValueKind.String ? em.GetString() ?? "" : "",
                user.TryGetProperty("avatar", out var av) ? av.GetString() ?? "" : "",
                ReadCredit(user)
            ));

            if (showLatency)
                ApplyLatency(sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            sw.Stop();
            if (showError)
                _context.Snackbar.ShowError(ex.Message);
            if (showLatency)
            {
                LatencyText = "";
                LatencyBrush = null;
            }
        }
        finally
        {
            IsLoadingUserInfo = false;
        }
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

    private void ApplyUserInfo(UserInfoResult? result)
    {
        if (result is null) return;

        // 回滚链: name → email(非占位) → id
        if (!string.IsNullOrEmpty(result.Name))
            UserName = result.Name;
        else if (!string.IsNullOrEmpty(result.Email) && !IsPlaceholderEmail(result.Id, result.Email))
            UserName = result.Email;
        else
            UserName = result.Id;

        UserCredits = result.Credits.ToString("N0");
        AvatarUrl = !string.IsNullOrEmpty(result.Avatar) ? result.Avatar : null;
    }

    /// <summary>
    /// 判断 email 是否为占位地址：{id}@noreply.id.user.passport.voc.ink
    /// </summary>
    private static bool IsPlaceholderEmail(string id, string email)
    {
        return !string.IsNullOrEmpty(id)
            && email.StartsWith($"{id}@", StringComparison.OrdinalIgnoreCase);
    }

    private void ApplyLatency(long ms)
    {
        LatencyText = $"{ms} ms";
        LatencyBrush = ms switch
        {
            <= LatencyGoodMs => BrushGood,
            <= LatencyFairMs => BrushFair,
            _ => BrushPoor
        };
    }

    // ── 属性变更 → 自动保存 ──

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(ApiKey):          _siteSettings.ApiKey = ApiKey; break;
            case nameof(VoiceId):         _siteSettings.VoiceId = VoiceId; break;
            case nameof(PromptId):        _siteSettings.PromptId = PromptId; break;
            case nameof(SelectedPreset):  _siteSettings.Preset = SelectedPreset; break;
            case nameof(SelectedLanguage): _siteSettings.Language = SelectedLanguage; break;
            case nameof(SpeechRate):      _siteSettings.SpeechRate = SpeechRate; break;
            case nameof(BreakClone):      _siteSettings.BreakClone = BreakClone; break;
            case nameof(Vivid):           _siteSettings.Vivid = Vivid; break;
            case nameof(Flash):           _siteSettings.Flash = Flash; break;
            case nameof(EmoAngry):        _siteSettings.EmoAngry = EmoAngry; break;
            case nameof(EmoHappy):        _siteSettings.EmoHappy = EmoHappy; break;
            case nameof(EmoNeutral):      _siteSettings.EmoNeutral = EmoNeutral; break;
            case nameof(EmoSad):          _siteSettings.EmoSad = EmoSad; break;
            case nameof(EmoContext):       _siteSettings.EmoContext = EmoContext; break;
            case nameof(Seed):
                _siteSettings.Seed = int.TryParse(Seed, out var s) ? s : -1;
                break;
            // 显示属性不需要持久化
            default: return;
        }

        _context.SaveSettingStorage<Settings>();
    }

    public void Dispose()
    {
        PropertyChanged -= OnPropertyChanged;
    }
}
