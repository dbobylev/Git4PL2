using Git4PL2.Abstarct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Git4PL2.Plugin.Processes
{
    abstract class PluginCommand : ICommand
    {
        public string Name { get; private set; }

        public PluginCommand(string name)
        {
            Name = name;
        }

        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}
