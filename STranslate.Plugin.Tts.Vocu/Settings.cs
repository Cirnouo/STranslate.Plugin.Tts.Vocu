using System.Text.Json;
using System.Text.Json.Serialization;

namespace STranslate.Plugin.Tts.Vocu;

/// <summary>
/// 插件顶层配置，包含双站点独立配置
/// </summary>
public class Settings
{
    /// <summary>
    /// 当前活跃站点索引（0=Vocu, 1=Wusound）
    /// </summary>
    public int ActiveSiteIndex { get; set; }

    /// <summary>
    /// Vocu（国际版）配置
    /// </summary>
    public SiteSettings Vocu { get; set; } = new();

    /// <summary>
    /// Wusound（国内版）配置
    /// </summary>
    public SiteSettings Wusound { get; set; } = new();

    /// <summary>
    /// 捕获 v1 扁平格式的遗留字段，用于自动迁移。
    /// 迁移完成后置 null。
    /// </summary>
    [JsonExtensionData]
    public Dictionary<string, JsonElement>? ExtensionData { get; set; }
}
