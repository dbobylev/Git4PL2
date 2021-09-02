#define IDESTUB
using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF.View;
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


namespace Git4PL2.IDEStub
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var x = NinjectCore.Get<IPluginSettingsStorage>();

            var p = x.GetParam(Plugin.Settings.ePluginParameterID.TEAMCODING_FILEPROVIDER_PATH);
            p.SetValue("D:\\");
        }

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            WindowSettings ws = new WindowSettings();
            ws.Show();

            Close();
        }

        private void ButtonTeamCoding_Click(object sender, RoutedEventArgs e)
        {
            WindowTeamCoding wtg = new WindowTeamCoding();
            wtg.Show();

            Close();
        }

        private void ButtonDicti_Click(object sender, RoutedEventArgs e)
        {
            WindowDicti d = new WindowDicti("123");
            d.Show();
        }

        private void ButtonDicx_Click(object sender, RoutedEventArgs e)
        {
            WindowDicx d = new WindowDicx("123");
            d.Show();
        }
    }
}
