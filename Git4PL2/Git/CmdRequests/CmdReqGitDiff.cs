using Git4PL2.Git.CmdReaders;
using Git4PL2.Plugin.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Git.CmdRequests
{
    class CmdReqGitDiff :CmdBuilderGIT
    {
        private readonly IDbObjectText _dbObj;
        private string tmpPath;

        public CmdReqGitDiff(IDbObjectText dbObj, ICmdReader reader = null) : base(reader ?? new CmdReadDiffText())
        {
            _dbObj = dbObj;
        }

        private void DeleteTempFile()
        {
            if (File.Exists(tmpPath))
                File.Delete(tmpPath);
            Seri.Log.Here().Debug($"Временный файл удалён. path: {tmpPath}");
        }

        protected override void BeforeProcess()
        {
            tmpPath = $"{_dbObj.GetRawFilePath()}.tmp";

            Seri.Log.Here().Debug($"Создаём временный файл. path: {tmpPath}");
            File.WriteAllText(tmpPath, _dbObj.Text, _dbObj.GetSaveEncoding());
            Seri.Log.Here().Debug("Временный файл создан");

            AddArgument("diff");
            AddArgumentCrAtEol();
            AddArgumentSpaceAtEol();
            AddArgument("--no-index");
            AddArgument($"\"{_dbObj.GetRawFilePath()}\"");
            AddArgument($"\"{tmpPath}\"");

            base.BeforeProcess();
        }
        protected override void AfterProcess()
        {
            base.AfterProcess();

            DeleteTempFile();
        }
        protected override void OnErrorOccurred()
        {
            base.OnErrorOccurred();

            DeleteTempFile();
        }
    }
}
