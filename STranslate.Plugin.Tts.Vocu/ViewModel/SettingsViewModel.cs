using CommunityToolkit.Mvvm.ComponentModel;

namespace STranslate.Plugin.Tts.Vocu.ViewModel;

/// <summary>
/// 站点切换协调器，持有两个站点的 ViewModel
/// </summary>
public partial class SettingsViewModel : ObservableObject, IDisposable
{
    private readonly IPluginContext _context;
    private readonly Settings _settings;

    /// <summary>Vocu（国际版）设置</summary>
    public SiteSettingsViewModel VocuSite { get; }

    /// <summary>Wusound（国内版）设置</summary>
    public SiteSettingsViewModel WusoundSite { get; }

    /// <summary>
    /// 当前选中的站点索引（0=Vocu, 1=Wusound）
    /// </summary>
    [ObservableProperty]
    public partial int SelectedTabIndex { get; set; }

    /// <summary>
    /// 当前活跃站点的 ViewModel
    /// </summary>
    public SiteSettingsViewModel ActiveSiteViewModel =>
        SelectedTabIndex == 1 ? WusoundSite : VocuSite;

    public SettingsViewModel(
        IPluginContext context,
        Settings settings,
        Task<UserInfoResult?>? vocuPendingTask,
        Task<UserInfoResult?>? wusoundPendingTask)
    {
        _context = context;
        _settings = settings;
        SelectedTabIndex = settings.ActiveSiteIndex;

        VocuSite = new SiteSettingsViewModel(context, settings.Vocu, SiteType.Vocu, vocuPendingTask);
        WusoundSite = new SiteSettingsViewModel(context, settings.Wusound, SiteType.Wusound, wusoundPendingTask);
    }

    partial void OnSelectedTabIndexChanged(int value)
    {
        OnPropertyChanged(nameof(ActiveSiteViewModel));
        _settings.ActiveSiteIndex = value;
        _context.SaveSettingStorage<Settings>();
    }

    public void Dispose()
    {
        VocuSite.Dispose();
        WusoundSite.Dispose();
    }
}
