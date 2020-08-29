using Git4PL2.Plugin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Abstarct
{
    interface IPluginCommands
    {
        void SaveTextToRepository(TextOperationsParametrs param = null);

        void LoadTextFromRepository(TextOperationsParametrs param = null);

        void ShowGitDiff();

        void ShowSettings();

        void ShowGitBlame();

        void ShowDicti();

        void ShowTeamCoding();

        void CheckOut();

        void CheckIn();
    }
}
