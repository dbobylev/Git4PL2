using Git4PL2.Plugin.WPF.ModelView;
using Git4PL2.Plugin.WPF.UserControls;
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
    /// Interaction logic for WindowGitBlame.xaml
    /// </summary>
    public partial class WindowGitBlame : Window
    {
        private event Action<string> ClickOnFlowDocumnet;

        public WindowGitBlame(IEnumerable<string> lines)
        {
            InitializeComponent();

            var viewModel = new GitBlameViewModel(lines);
            DataContext = viewModel;
            ClickOnFlowDocumnet += viewModel.ClickOnFlowDocumnet;
        }

        private void FlowDocumentScrollViewerExt_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ClickOnFlowDocumnet != null)
            {
                var doc = sender as FlowDocumentScrollViewerExt;
                var textPointer = doc.Selection.Start;
                var LineTextBeforeCursorPostion = textPointer.GetTextInRun(LogicalDirection.Backward);
                var LineTextAfterCursorPostion = textPointer.GetTextInRun(LogicalDirection.Forward);

                ClickOnFlowDocumnet(LineTextBeforeCursorPostion + LineTextAfterCursorPostion);
            }
        }

        private void WindowClose(object sender, RoutedEventArgs e)
        {
            Close();
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
