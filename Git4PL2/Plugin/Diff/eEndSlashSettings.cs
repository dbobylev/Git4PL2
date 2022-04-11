using System.ComponentModel;

namespace Git4PL2.Plugin.Diff
{
    public enum eEndSlashSettings
    {
        [Description("Настройка отключена")]
        OptionDisabled = 0,

        [Description("Не изменять '/' при сохранении в репозиторий")]
        DontTouchSlash = 1,

        [Description("Добавлять '/' при сохранении в репозиторий")]
        AlwaysAddSlash = 2
    }
}
