using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.Abstract
{
    interface IGitAPI
    {
        string GetCurrentBranch();

        IDiffText GitDiff(IDbObjectText dbObject);

        string GitGetLastCommit();

        T GitBlame<T>(string FileName, int beginLine, int endline = -1, bool ShowEmail = false, ICmdReader CustomReader = null);
    }
}
