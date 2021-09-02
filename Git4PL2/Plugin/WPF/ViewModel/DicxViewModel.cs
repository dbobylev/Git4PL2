using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class DicxViewModel: QueryViewModel<Dicx>
    {
        private IIDEProvider _IDEProvider;
        private ISettings _Settings;

        public DicxViewModel(string SelectedText, IIDEProvider IDEProvider, ISettings Settings):
            base(IDEProvider, Settings, Settings.SQL_DICX, Settings.SQL_DICX_COUNT)

        {
            _IDEProvider = IDEProvider;
            _Settings = Settings;

            if (Regex.IsMatch(SelectedText, @"^\d+$"))
                CurrentIsn = long.Parse(SelectedText);
            else
            {
                var query = string.Format(_Settings.SQL_DICTIISN_BY_CONSTNAME, SelectedText);
                CurrentIsn = _IDEProvider.SQLQueryExecute<DummyNumber>(query).First().Value;
            }
        }
    }
}
