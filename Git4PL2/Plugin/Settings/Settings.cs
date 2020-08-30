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
        private readonly IPluginSettingsStorage _PluginSettingsStorage;

        public Settings(IPluginSettingsStorage PluginSettingsStorage)
        {
            _PluginSettingsStorage = PluginSettingsStorage;
        }

        public string GitRepositoryPath => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.GitRepositoryPath);
        public int SaveEncodingType => _PluginSettingsStorage.GetParamValue<int>(ePluginParameterID.SaveEncodingType);
        public bool DiffAddSchema => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.DiffAddSchema);
        public bool DiffChangeCor => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.DiffChangeCor);
        public bool DiffChangeName => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.DiffChangeName);
        public bool DiffCRLF => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.DiffCRLF);
        public bool DiffWorkWithSlash => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.DiffWorkWithSlash);
        public bool UnexpectedBranch => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.UnexpectedBranch);
        public bool UnexpectedServer => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.UnexpectedServer);
        public string WarnInRegEx => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.WarnInRegEx);
        public string WarnOutRegEx => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.WarnOutRegEx);
        public bool ClassicButtonsPosition => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.ClassicButtonsPosition);
        public bool ShowGitBlameProperties => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.ShowGitBlameProperties);
        public string CommitViewURL => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.CommitViewURL);
        public string SQL_DICTI_PARENT_COUNT => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.SQL_DICTI_PARENT_COUNT);
        public string SQL_DICTI_PARENT => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.SQL_DICTI_PARENT);
        public string SQL_DICTI_HIERARCHY => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.SQL_DICTI_HIERARCHY);
        public bool DICTI_CHILDREN_LIMIT_ENABLE => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.DICTI_CHILDREN_LIMIT_ENABLE);
        public int DICTI_CHILDREN_LIMIT_VALUE => _PluginSettingsStorage.GetParamValue<int>(ePluginParameterID.DICTI_CHILDREN_LIMIT_VALUE);
        public string SQL_DICTIISN_BY_CONSTNAME => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.SQL_DICTIISN_BY_CONSTNAME);
        public string SQL_SERVERNAME => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.SQL_SERVERNAME);
        public string SQL_FTOGGLE => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.SQL_FTOGGLE);
        public bool TEAMCODING_ENABLE => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.TEAMCODING_ENABLE);
        public string TEAMCODING_LOGIN => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.TEAMCODING_LOGIN);
        public bool TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT => _PluginSettingsStorage.GetParamValue<bool>(ePluginParameterID.TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT);
        public int TEAMCODING_PROVIDER => _PluginSettingsStorage.GetParamValue<int>(ePluginParameterID.TEAMCODING_PROVIDER);
        public string TEAMCODING_FILEPROVIDER_PATH => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.TEAMCODING_FILEPROVIDER_PATH);
        public string TEAMCODING_SERVERNAME_REGEX => _PluginSettingsStorage.GetParamValue<string>(ePluginParameterID.TEAMCODING_SERVERNAME_REGEX);
    }
}
