using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Git4PL2.Plugin.WPF.UserControls
{
    class FlowDocumentScrollViewerExt : FlowDocumentScrollViewer
    {
        public static readonly DependencyProperty ListRunsProperty = 
            DependencyProperty.Register(
              "ListRuns",
              typeof(IEnumerable<Run>),
              typeof(FlowDocumentScrollViewerExt));

        public IEnumerable<Run> ListRuns
        {
            get { return (IEnumerable<Run>)GetValue(ListRunsProperty); }
            set { SetValue(ListRunsProperty, value); }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property.Name == "ListRuns" && e.NewValue is IEnumerable<Run> listRuns)
            {
                Paragraph p = new Paragraph();
                foreach (Run item in listRuns)
                {
                    p.Inlines.Add(item);
                }
                Document.Blocks.Clear();
                Document.Blocks.Add(p);
            }

            base.OnPropertyChanged(e);
        }
    }
}
