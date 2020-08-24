using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.Processes;
using Git4PL2.Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Git4PL2.Plugin.WPF.ModelView.Converters
{
    class ParameterViewModel : PropertyChangedBase
    {
        private IPluginParameter _model;

        public ParameterViewModel(IPluginParameter model)
        {
            _model = model;
            SelectPathCommand = new RelayCommand(SelectFolder);
        }

        public ePluginParameterNames ID => _model.ID;

        public ePluginParameterUIType ParamterUIType => _model.ParamterUIType;

        public string Description => _model.Description;

        public string DescriptionExt => _model.DescriptionExt;

        public string ValueString
        {
            get
            {
                return _model.GetValue<string>();
            }
            set
            {
                _model.SetValue(value);
                OnPropertyChanged();
            }
        }

        public bool ValueBool
        {
            get
            {
                return _model.GetValue<bool>();
            }
            set
            {
                _model.SetValue(value);
                OnPropertyChanged();
            }
        }

        public int ValueInt
        {
            get
            {
                return _model.GetValue<int>();
            }
            set
            {
                _model.SetValue(value);
                OnPropertyChanged();
            }
        }

        public ePluginParameterNames ParentParameter => _model.ParentParameter;

        public IEnumerable<string> ListItems => (_model as PluginParameterList).ListItems;

        public RelayCommand SelectPathCommand { get; private set; }

        private void SelectFolder(object param)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    ValueString = fbd.SelectedPath;
            }
        }
    }
}
