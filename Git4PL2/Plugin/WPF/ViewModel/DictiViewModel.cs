using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Commands;
using Git4PL2.Plugin.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class DictiViewModel : QueryViewModel<Dicti>
    {
        private IIDEProvider _IDEProvider;
        private ISettings _Settings;
        public ObservableCollection<Dicti> Hierarchy { get; private set; }

        // Сюда приходит значение, если нажата любая кнопка "Перейти"
        public long? SelectedIsn
        {
            set
            {
                if (value != null && (long)value != CurrentIsn)
                {
                    CurrentIsn = (long)value;
                }
            }
        }

        public DictiViewModel(string SelectedText, IIDEProvider IDEProvider, ISettings Settings) 
            :base(IDEProvider, Settings, Settings.SQL_DICTI_PARENT, Settings.SQL_DICTI_PARENT_COUNT)
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

        protected override void FillViewModel()
        {
            // Делаем иерархический запрос
            var HierarchyQuery = string.Format(_Settings.SQL_DICTI_HIERARCHY, CurrentIsn);
            Hierarchy = new ObservableCollection<Dicti>(_IDEProvider.SQLQueryExecute<Dicti>(HierarchyQuery));
            OnPropertyChanged("Hierarchy");

            base.FillViewModel();
        }
    }
}
