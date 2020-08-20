using Git4PL2.Plugin.WPF.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Git4PL2.Plugin.WPF.View
{
    /// <summary>
    /// Interaction logic for WindowGitBlameProperties.xaml
    /// </summary>
    public partial class WindowGitBlameProperties : Window
    {
        public int OutputValMinus { get; set; }

        public int OutputValPlus { get; set; }

        public WindowGitBlameProperties(string line)
        {
            InitializeComponent();

            DataContext = new GitBlamePropertiesViewModel(line, SetDialogResultOk);
        }

        private void SetDialogResultOk()
        {
            DialogResult = true;
        }


        protected override void OnClosed(EventArgs e)
        {
            var ViewModel = DataContext as GitBlamePropertiesViewModel;
            OutputValMinus = ViewModel.ValMinus;
            OutputValPlus = ViewModel.ValPlus;
            base.OnClosed(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
