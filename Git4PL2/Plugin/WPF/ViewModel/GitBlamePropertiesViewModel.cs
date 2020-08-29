using Git4PL2.Plugin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class GitBlamePropertiesViewModel :PropertyChangedBase
    {
        public ICommand Perform { get; set; }

        public ICommand PlusFive { get; set; }

        public string Line { get; set; }

        public int ValMinus { get; set; }

        public int ValPlus { get; set; }

        public GitBlamePropertiesViewModel(string line, Action DoOnPerform)
        {
            ValMinus = 5;
            ValPlus = 5;
            Line = line;

            PlusFive = new RelayCommand((x) => { ValPlus += 5; ValMinus += 5; });
            Perform = new RelayCommand((x) => DoOnPerform());
        }
    }
}
