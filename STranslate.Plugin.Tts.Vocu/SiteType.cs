namespace STranslate.Plugin.Tts.Vocu;

/// <summary>
/// 支持的 TTS 服务站点
/// </summary>
public enum SiteType
{
    /// <summary>vocu.ai 国际版</summary>
    Vocu = 0,

    /// <summary>悟声 国内版</summary>
    Wusound = 1
}

/// <summary>
/// 站点常量映射
/// </summary>
public static class SiteInfo
{
    /// <summary>TTS API 基础地址</summary>
    public static string GetApiBaseUrl(SiteType site) => site switch
    {
        SiteType.Wusound => "https://v1.wusound.cn",
        _ => "https://v1.vocu.ai"
    };

    /// <summary>站点主页（用于超链接）</summary>
    public static string GetWebUrl(SiteType site) => site switch
    {
        SiteType.Wusound => "https://wusound.cn",
        _ => "https://vocu.ai"
    };

    /// <summary>站点显示名</summary>
    public static string GetDisplayName(SiteType site) => site switch
    {
        SiteType.Wusound => "wusound.cn",
        _ => "vocu.ai"
    };
}
