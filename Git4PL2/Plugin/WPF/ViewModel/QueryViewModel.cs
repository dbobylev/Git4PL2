using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Commands;
using Git4PL2.Plugin.WPF.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    abstract class QueryViewModel<T>: PropertyChangedBase where T:new()
    {
        private IIDEProvider _IDEProvider;
        private ISettings _Settings;
        private string _RawDataQuery;
        private string _RawDataCountQuery;

        public ObservableCollection<T> Data { get; private set; }

        public ICommand FetchNextRowsCommand { get; private set; }

        private long _CurrentIsn;
        protected long CurrentIsn
        {
            get => _CurrentIsn;
            set
            {
                _CurrentIsn = value;
                FillViewModel();
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

        private int _CurrentRowLimit;
        public int CurrentRowLimit
        {
            get => _CurrentRowLimit;
            set
            {
                _CurrentRowLimit = value;
                OnPropertyChanged();
            }
        }

        private int _RowCount;
        public int RowCount
        {
            get => _RowCount;
            set
            {
                _RowCount = value;
                OnPropertyChanged();
            }
        }

        private bool _HasRowOverflow;
        public bool HasRowOverflow
        {
            get => _HasRowOverflow;
            set
            {
                _HasRowOverflow = value;
                OnPropertyChanged();
            }
        }

        public QueryViewModel(IIDEProvider IDEProvider, ISettings Settings, string RawDataQuery, string RawDataCountQuery)
        {
            _IDEProvider = IDEProvider;
            _Settings = Settings;
            _RawDataQuery = RawDataQuery;
            _RawDataCountQuery = RawDataCountQuery;

            CurrentRowLimit = _Settings.DICTI_CHILDREN_LIMIT_VALUE;
            FetchNextRowsCommand = new RelayCommand((x) => FetchNextRows(x), (x) => RowsFetched < RowCount);
        }

        protected virtual void FillViewModel()
        {
            RowsFetched = 0;

            // Запрос для дочерних элементов
            var DataQuery = string.Format(_RawDataQuery, _CurrentIsn);

            // Если включено ограничение на кол-во элементов
            if (_Settings.DICTI_CHILDREN_LIMIT_ENABLE)
            {
                // Определяем число дочерних элементов
                var DataCountQuery = string.Format(_RawDataCountQuery, _CurrentIsn);
                RowCount = (int)_IDEProvider.SQLQueryExecute<DummyNumber>(DataCountQuery).First().Value;

                // Проверяем есть ли превышение, отобразиться строка по таблицей
                HasRowOverflow = RowCount > CurrentRowLimit;

                // Если есть ограничиваем выборку
                if (HasRowOverflow)
                {
                    DataQuery += $" fetch first {CurrentRowLimit} rows only";
                    RowsFetched += CurrentRowLimit;
                }
            }

            // Выполняем запрос
            Data = new ObservableCollection<T>(_IDEProvider.SQLQueryExecute<T>(DataQuery));
            OnPropertyChanged("Data");
        }

        /// <summary>
        /// Вызов этого метода происходит при нажатии, загрузить еще, загрузить всё
        /// </summary>
        /// <param name="param"></param>
        public void FetchNextRows(object param)
        {
            bool FetchAll = (bool)param;

            var DataQuery = string.Format(_RawDataQuery, _CurrentIsn);
            DataQuery += $" OFFSET {RowsFetched} ROWS";

            if (!FetchAll)
            {
                DataQuery += $" FETCH NEXT {CurrentRowLimit} ROWS ONLY";
                RowsFetched = Math.Min(RowsFetched + CurrentRowLimit, RowCount);
            }
            else
            {
                RowsFetched = RowCount;
            }

            var FetchedRows = _IDEProvider.SQLQueryExecute<T>(DataQuery);
            foreach (T item in FetchedRows)
                Data.Add(item);
            OnPropertyChanged("Data");
        }
    }
}
