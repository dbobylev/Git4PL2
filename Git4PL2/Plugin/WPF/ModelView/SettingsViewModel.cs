using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Settings;
using Git4PL2.Plugin.WPF.ModelView.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git4PL2.Plugin.WPF.ModelView
{
    class SettingsViewModel : PropertyChangedBase
    {
        private IPluginSettingsStorage _SettingStorages;

        public IEnumerable<PluginParameterGroup> Groups { get; private set; }

        private PluginParameterGroup _SelectedGroup;
        public PluginParameterGroup SelectedGroup
        {
            get => _SelectedGroup; 
            set
            {
                _SelectedGroup = value;
                FillParemetersList();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ParameterViewModel> ParametersList { get; private set; }

        public SettingsViewModel(IPluginSettingsStorage settingStorages)
        {
            ParametersList = new ObservableCollection<ParameterViewModel>();
            ParametersList.CollectionChanged += ParametersList_CollectionChanged;

            _SettingStorages = settingStorages;
            Groups = _SettingStorages.GetGroups;
        }

        private void FillParemetersList()
        {
            if (SelectedGroup != null)
            {
                ParametersList.Clear();

                foreach (IPluginParameter item in _SettingStorages.ParametersByGroup(_SelectedGroup.GroupType))
                    ParametersList.Add(new ParameterViewModel(item));
            }
        }

        /// <summary>
        /// Происходит при изменении коллекции с настройками
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Seri.Log.Here().Information(e.Action.ToString());

            if (e.NewItems != null)
            {
                // Каждому новому параметру добавляем слушетеля на изменения его свойств
                foreach (object item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ParameterPropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                // У старых параметров убираем лишний хэндлер
                foreach (object item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ParameterPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Происходит когда значение параметра меняется
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParameterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Так как от булевых параметров у нас зависит вкл/выкл зависимых параметров
            // То при нажатии чекбокса, перезагружаем параметры в окне
            if (e.PropertyName == "ValueBool")
                FillParemetersList();
        }
    }
}
