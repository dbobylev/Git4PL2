﻿using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    public class FtoggleViewModel : PropertyChangedBase
    {

        public ObservableCollection<Ftoggle> FtoggleList { get; private set; }

        public FtoggleViewModel(ISettings Settings, IIDEProvider IDEProvider, string SelectedText)
        {
            var FtoggleQuery = string.Format(Settings.SQL_FTOGGLE, SelectedText);
            var CurrentToggle = IDEProvider.SQLQueryExecute<Ftoggle>(FtoggleQuery);
            FtoggleList = new ObservableCollection<Ftoggle>(CurrentToggle);
        }
    }
}
