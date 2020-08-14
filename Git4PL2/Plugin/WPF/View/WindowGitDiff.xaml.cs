using Git4PL2.Plugin.Abstract;
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
    /// Interaction logic for WindowGitDiff.xaml
    /// </summary>
    public partial class WindowGitDiff : Window
    {
        public WindowGitDiff(IDbObjectText DbObjectText)
        {
            InitializeComponent();

            var param = NinjectCore.GetParameter("DbObjectText", DbObjectText);
            DataContext = NinjectCore.Get<GitDiffViewModel>(param);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
