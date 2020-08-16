﻿using Git4PL2.Plugin.WPF.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.Processes
{
    class PluginCommandShowSettings :PluginCommand
    {
        public PluginCommandShowSettings() :base("PluginCommandShowSettings")
        {

        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            WindowSettings ws = new WindowSettings();
            ws.Show();
        }
    }
}
