using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Settings
{
    public enum ePluginParameterID
    {
        NULL,
        GitRepositoryPath,
        SaveEncodingType,
        DiffAddSchema,
        DiffChangeCor,
        DiffChangeName,
        DiffCRLF,
        DiffEndSpace,
        DiffWorkWithSlash,
        UnexpectedBranch,
        UnexpectedServer,
        WarnInRegEx,
        WarnOutRegEx,
        ClassicButtonsPosition,
        ShowGitBlameProperties,
        CommitViewURL,
        SQL_DICTI_PARENT_COUNT,
        SQL_DICTI_PARENT,
        SQL_DICTI_HIERARCHY,
        SQL_DICTIISN_BY_CONSTNAME,
        SQL_SERVERNAME,
        SQL_FTOGGLE,
        DICTI_CHILDREN_LIMIT_ENABLE,
        DICTI_CHILDREN_LIMIT_VALUE,
        TEAMCODING_ENABLE,
        TEAMCODING_LOGIN,
        TEAMCODING_RESTRICT_COMPILE_WITHOUT_CHECKOUT,
        TEAMCODING_PROVIDER,
        TEAMCODING_FILEPROVIDER_PATH,
        TEAMCODING_SERVERNAME_REGEX,
        HelpLink,
        GoToLine
    }
}