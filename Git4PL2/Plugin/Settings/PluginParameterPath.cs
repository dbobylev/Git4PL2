using Git4PL2.Plugin.Processes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Git4PL2.Plugin.Settings
{
    class PluginParameterPath : PluginParameter<string>, INotifyPropertyChanged
    {
        public RelayCommand SelectPathCommand { get; set; }

        public PluginParameterPath(ePluginParameterNames parameterName, string defaultValue) : base(parameterName, defaultValue)
        {
            SelectPathCommand = new RelayCommand(SelectFolder);
            ParamterType = ePluginParameterUIType.Path;
        }

        private void SelectFolder(object param)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    SetValue(fbd.SelectedPath);
                    OnPropertyChanged("ValueString");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
