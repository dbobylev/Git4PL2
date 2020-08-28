using Git4PL2.Plugin.WPF.ViewModel;
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
    /// Interaction logic for WindowTeamCoding.xaml
    /// </summary>
    public partial class WindowTeamCoding : Window
    {
        private TeamCodingViewModel _viewmodel;

        public WindowTeamCoding()
        {
            InitializeComponent();

            _viewmodel = NinjectCore.Get<TeamCodingViewModel>();
            DataContext = _viewmodel;
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
