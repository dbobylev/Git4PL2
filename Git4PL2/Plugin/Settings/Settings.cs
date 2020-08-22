using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Settings
{
    class Settings :ISettings
    {
        private IPluginSettingsStorage _PluginSettingsStorage;

        public Settings(IPluginSettingsStorage PluginSettingsStorage)
        {
            _PluginSettingsStorage = PluginSettingsStorage;
        }

        public string GitRepositoryPath => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.GitRepositoryPath);
        public int SaveEncodingType => _PluginSettingsStorage.GetParamValue<int>(ePluginParameterNames.SaveEncodingType);
        public bool DiffAddSchema => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.DiffAddSchema);
        public bool DiffChangeCor => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.DiffChangeCor);
        public bool DiffChangeName => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.DiffChangeName);
        public bool DiffCRLF => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.DiffCRLF);
        public bool DiffWorkWithSlash => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.DiffWorkWithSlash);
        public bool UnexpectedBranch => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.UnexpectedBranch);
        public bool UnexpectedServer => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.UnexpectedServer);
        public string WarnInRegEx => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.WarnInRegEx);
        public string WarnOutRegEx => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.WarnOutRegEx);
        public bool ClassicButtonsPosition => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.ClassicButtonsPosition);
        public bool ShowGitBlameProperties => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.ShowGitBlameProperties);
        public string CommitViewURL => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.CommitViewURL);

        public string SQL_DICTI_PARENT_COUNT => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.SQL_DICTI_PARENT_COUNT);
        public string SQL_DICTI_PARENT => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.SQL_DICTI_PARENT);
        public string SQL_DICTI_HIERARCHY => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.SQL_DICTI_HIERARCHY);

        public bool DICTI_CHILDREN_LIMIT_ENABLE => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterNames.DICTI_CHILDREN_LIMIT_ENABLE);
        public int DICTI_CHILDREN_LIMIT_VALUE => _PluginSettingsStorage.GetParamValue<int>(ePluginParameterNames.DICTI_CHILDREN_LIMIT_VALUE);
        public string SQL_DICTIISN_BY_CONSTNAME => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterNames.SQL_DICTIISN_BY_CONSTNAME);
    }
}
