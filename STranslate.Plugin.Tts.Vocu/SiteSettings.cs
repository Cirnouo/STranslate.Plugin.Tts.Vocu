namespace STranslate.Plugin.Tts.Vocu;

/// <summary>
/// 单站点的 TTS 配置（Vocu / Wusound 各持有一份）
/// </summary>
public class SiteSettings
{
    // ── API 认证 ──

    /// <summary>
    /// Bearer Token，用于 API 身份验证
    /// </summary>
    public string ApiKey { get; set; } = "";

    // ── 语音配置 ──

    /// <summary>
    /// 语音角色 ID（必填）
    /// </summary>
    public string VoiceId { get; set; } = "";

    /// <summary>
    /// 角色风格 ID
    /// </summary>
    public string PromptId { get; set; } = "default";

    // ── 生成参数 ──

    /// <summary>
    /// 参数预设：creative / balance / stable
    /// </summary>
    public string Preset { get; set; } = "balance";

    /// <summary>
    /// 生成语言：auto / en / zh / ja / fr / es / de / ko / pt / yue
    /// </summary>
    public string Language { get; set; } = "auto";

    /// <summary>
    /// 语速控制（0.5 ~ 2.0）
    /// </summary>
    public double SpeechRate { get; set; } = 1.0;

    // ── 高级选项 ──

    /// <summary>
    /// 偏向文本的情感风格
    /// </summary>
    public bool BreakClone { get; set; } = true;

    /// <summary>
    /// 生动表达模式（仅 V3.0 角色）
    /// </summary>
    public bool Vivid { get; set; }

    /// <summary>
    /// 低延迟模式
    /// </summary>
    public bool Flash { get; set; }

    // ── 情绪控制（0-10，默认 0 = 跟随角色样本） ──

    public int EmoAngry { get; set; }
    public int EmoHappy { get; set; }
    public int EmoNeutral { get; set; }
    public int EmoSad { get; set; }
    public int EmoContext { get; set; }

    // ── 其他 ──

    /// <summary>
    /// 生成种子，-1 为随机
    /// </summary>
    public int Seed { get; set; } = -1;
}
