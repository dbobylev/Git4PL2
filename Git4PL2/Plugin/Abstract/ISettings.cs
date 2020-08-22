using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Abstract
{
    public interface ISettings
    {
        string GitRepositoryPath { get; }
        int SaveEncodingType { get; }
        bool DiffAddSchema { get; }
        bool DiffChangeCor { get; }
        bool DiffChangeName { get; }
        bool DiffCRLF { get; }
        bool DiffWorkWithSlash { get; }
        bool UnexpectedBranch { get; }
        bool UnexpectedServer { get; }
        string WarnInRegEx { get; }
        string WarnOutRegEx { get; }
        bool ClassicButtonsPosition { get; }
        bool ShowGitBlameProperties { get; }
        string CommitViewURL { get; }
        
        string SQL_DICTI_PARENT_COUNT { get; }
        string SQL_DICTI_PARENT { get; }
        string SQL_DICTI_HIERARCHY { get; }
        string SQL_DICTIISN_BY_CONSTNAME { get; }

        bool DICTI_CHILDREN_LIMIT_ENABLE { get; }
        int DICTI_CHILDREN_LIMIT_VALUE { get; }

       
    }
}
