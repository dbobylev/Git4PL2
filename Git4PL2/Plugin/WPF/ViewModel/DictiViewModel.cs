using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Processes;
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
    class DictiViewModel : PropertyChangedBase
    {
        private IIDEProvider _IDEProvider;
        private ISettings _Settings;
        public ObservableCollection<Dicti> Hierarchy { get; private set; }
        public ObservableCollection<Dicti> Childrens { get; private set; }

        public ICommand FetchNextRowsCommand { get; private set; }

        private long _CurrentIsn;
        private long CurrentIsn 
        {
            get => _CurrentIsn; 
            set
            {
                _CurrentIsn = value;
                FillViewModel();
            } 
        }

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

        private int _RowsFetched = -1;
        public int RowsFetched
        {
            get => _RowsFetched;
            set
            {
                _RowsFetched = value;
                OnPropertyChanged();
            }
        }

        private int _CurrentChildrenLimit;
        public int CurrentChildrenLimit
        {
            get => _CurrentChildrenLimit;
            set
            {
                _CurrentChildrenLimit = value;
                OnPropertyChanged();
            }
        }

        private int _ChildrenCount;
        public int ChildrenCount
        {
            get => _ChildrenCount;
            set
            {
                _ChildrenCount = value;
                OnPropertyChanged();
            }
        }

        private bool _HasChildrenOverflow;
        public bool HasChildrenOverflow
        {
            get => _HasChildrenOverflow;
            set
            {
                _HasChildrenOverflow = value;
                OnPropertyChanged();
            }
        }


        public DictiViewModel(string SelectedText, IIDEProvider IDEProvider, ISettings Settings)
        {
            _IDEProvider = IDEProvider;
            _Settings = Settings;
            CurrentChildrenLimit = _Settings.DICTI_CHILDREN_LIMIT_VALUE;
            FetchNextRowsCommand = new RelayCommand((x) => FetchNextRows(x), (x) => RowsFetched < ChildrenCount);

            if (Regex.IsMatch(SelectedText, @"^\d+$"))
                CurrentIsn = long.Parse(SelectedText);
            else
            {
                var query = string.Format(_Settings.SQL_DICTIISN_BY_CONSTNAME, SelectedText);
                CurrentIsn = _IDEProvider.SQLQueryExecute<DummyNumber>(query).First().Value;
            }
        }

        private void FillViewModel()
        {
            RowsFetched = 0;

            // Делаем иерархический запрос
            var HierarchyQuery = string.Format(_Settings.SQL_DICTI_HIERARCHY, _CurrentIsn);
            Hierarchy = new ObservableCollection<Dicti>(_IDEProvider.SQLQueryExecute<Dicti>(HierarchyQuery));
            OnPropertyChanged("Hierarchy");

            // Запрос для дочерних элементов
            var ChildrensQuery = string.Format(_Settings.SQL_DICTI_PARENT, _CurrentIsn);

            // Если включено ограничение на кол-во элементов
            if (_Settings.DICTI_CHILDREN_LIMIT_ENABLE)
            {
                // Определяем число дочерних элементов
                var ChildrenCountQuery = string.Format(_Settings.SQL_DICTI_PARENT_COUNT, _CurrentIsn);
                ChildrenCount = (int)_IDEProvider.SQLQueryExecute<DummyNumber>(ChildrenCountQuery).First().Value;

                // Проверяем есть ли превышение, отобразиться строка по таблицей
                HasChildrenOverflow = ChildrenCount > CurrentChildrenLimit;

                // Если есть ограничиваем выборку
                if (HasChildrenOverflow)
                {
                    ChildrensQuery += $" fetch first {CurrentChildrenLimit} rows only";
                    RowsFetched += CurrentChildrenLimit;
                }
            }

            // Выполняем запрос
            Childrens = new ObservableCollection<Dicti>(_IDEProvider.SQLQueryExecute<Dicti>(ChildrensQuery));
            OnPropertyChanged("Childrens");
        }

        /// <summary>
        /// В дочерних элементах уже есть загруженные строки
        /// Вызов этого метода происходит при нажатии, загрузить еще, загрузить всё
        /// </summary>
        /// <param name="param"></param>
        public void FetchNextRows(object param)
        {
            bool FetchAll = (bool)param;

            // Запрос для дочерних элементов
            var ChildrensQuery = string.Format(_Settings.SQL_DICTI_PARENT, _CurrentIsn);
            ChildrensQuery += $" OFFSET {RowsFetched} ROWS";

            if (!FetchAll)
            {
                ChildrensQuery += $" FETCH NEXT {CurrentChildrenLimit} ROWS ONLY";
                RowsFetched = Math.Min(RowsFetched + CurrentChildrenLimit, ChildrenCount);
            }
            else
            {
                RowsFetched = ChildrenCount;
            }

            var FetchedRows = _IDEProvider.SQLQueryExecute<Dicti>(ChildrensQuery);
            foreach (Dicti item in FetchedRows)
                Childrens.Add(item);
            OnPropertyChanged("Childrens");
        }
    }
}
