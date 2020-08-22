using Git4PL2.Plugin.WPF.Model;
using System;
using System.Collections;
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

namespace Git4PL2.Plugin.WPF.UserControls
{
    /// <summary>
    /// Interaction logic for DataGridDicti.xaml
    /// </summary>
    public partial class DataGridDicti : UserControl
    {
        public static DependencyProperty SelectedIsnProperty = DependencyProperty.Register("SelectedIsn", typeof(long?), typeof(DataGridDicti));
        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataGridDicti));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public long? SelectedIsn
        {
            get { return (long?)GetValue(SelectedIsnProperty); }
            set { SetValue(SelectedIsnProperty, value); }
        }

        public bool FirstRowBold { get; set; } = false;

        public DataGridDicti()
        {
            InitializeComponent();
        }

        private void SelectDictiRow_Click(object sender, RoutedEventArgs e)
        {
            SelectedIsn = ((sender as FrameworkElement).DataContext as Dicti).Isn;
        }

        private void DataGrid_Main_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (FirstRowBold && e.Row.GetIndex() == 0)
            {
                e.Row.FontWeight = FontWeights.Bold;
            }
        }
    }
}
