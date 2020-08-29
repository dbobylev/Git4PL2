using Git4PL2.Plugin.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Git4PL2.Plugin.WPF.ViewModel
{
    class GitBlameViewModel : PropertyChangedBase
    {
        private static readonly Random random = new Random();
        private const byte MIN_RGBVAL = 160;
        private const byte MAX_RGBVAL = 255;
        private Func<byte> GenRGBVal = () => (byte)random.Next(MIN_RGBVAL, MAX_RGBVAL);

        public ICommand ShowCommitCommand { get; private set; }

        public string CommitSha { get; private set; }

        public List<Run> ListRuns { get; private set; }

        public GitBlameViewModel(IEnumerable<string> lines)
        {
            ShowCommitCommand = NinjectCore.Get<CommandOpenCommitByURL>();

            ListRuns = new List<Run>();

            var ColorsByCommit = new Dictionary<string, SolidColorBrush>();

            foreach (string line in lines)
            {
                string sha = GetShaFromLine(line);

                SolidColorBrush brush;
                if (ColorsByCommit.ContainsKey(sha))
                    brush = ColorsByCommit[sha];
                else
                {
                    int pos255 = random.Next(0, 3);
                    byte colorR = pos255 == 0 ? (byte)255 : GenRGBVal();
                    byte colorG = pos255 == 1 ? (byte)255 : GenRGBVal();
                    byte colorB = pos255 == 2 ? (byte)255 : GenRGBVal();

                    brush = new SolidColorBrush(Color.FromRgb(colorR, colorG, colorB));
                    ColorsByCommit.Add(sha, brush);
                }

                ListRuns.Add(new Run(line + "\r\n")
                {
                    Background = brush
                });
            }
        }
        private string GetShaFromLine(string line)
        {
            return line.Substring(0, line.IndexOf(' '));
        }

        public void ClickOnFlowDocumnet(string line)
        {
            CommitSha = GetShaFromLine(line);
            OnPropertyChanged("CommitSha");
        }
    }
}
