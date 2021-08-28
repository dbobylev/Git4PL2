using Git4PL2.Plugin.Abstract;
using Git4PL2.Plugin.WPF.UserControls;
using Git4PL2.Plugin.WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public WindowGitDiff(IIDEProvider IDEProvider)
        {
            InitializeComponent();

            IDbObjectText DbObjectText = IDEProvider.GetDbObject<IDbObjectText>();

            var param = NinjectCore.GetParameter("DbObjectText", DbObjectText);
            DataContext = NinjectCore.Get<GitDiffViewModel>(param);
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
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

        protected override void OnPreviewKeyUp(KeyEventArgs e)
        {
            try
            {
                // При копировании текста, обрезаем информацию с номерами строк
                if (e.Key == Key.C && Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    string[] lines = Clipboard.GetText().Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                    Regex regex = new Regex(@"^\d*\s+\d*\s*\|\s[\-\+\s]");
                    for (int i = 0; i < lines.Length; i++)
                    {
                        Match match = regex.Match(lines[i], 0);
                        if (match.Success)
                        {
                            lines[i] = lines[i].Substring(match.Value.Length);
                        }
                    }

                    Clipboard.SetText(string.Join("\r\n", lines));
                }
            }
            catch(Exception ex)
            {
                Seri.Log.Error(ex, "GitDiff: Ошибка при копирование текста в буфер");
            }
            base.OnPreviewKeyUp(e);
        }

        private void FlowDocumentScrollViewerExt_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var doc = sender as FlowDocumentScrollViewerExt;

            try
            {
                var textPointer = doc.Selection.Start;
                if (textPointer.GetLineStartPosition(1) == null)
                {
                    // Кликнули в самом низу за пределами текста
                    return;
                }

                TextRange textrange = new TextRange(textPointer.GetLineStartPosition(0), textPointer.GetLineStartPosition(1));

                if ((DataContext as GitDiffViewModel).ClickOnFlowDocumnet(textrange.Text))
                    Close();

            }
            catch(Exception ex)
            {
                Seri.Log.Error(ex, "Ошибка при клике в окне gitdiff");
            }
        }
    }
}
