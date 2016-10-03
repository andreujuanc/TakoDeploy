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
using TakoDeployWPF.Domain;

namespace TakoDeployWPF
{
    /// <summary>
    /// Lógica de interacción para ScriptEditor.xaml
    /// </summary>
    public partial class ScriptEditor : UserControl
    {
        public ScriptEditor()
        {
            InitializeComponent();
            Loaded += ScriptEditor_Loaded;
        }

        private void ScriptEditor_Loaded(object sender, RoutedEventArgs e)
        {
            
            using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("TakoDeployWPF.Resources.sql.xshd"))
            {
                using (var reader = new System.Xml.XmlTextReader(stream))
                {
                    textEditor.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.Xshd.HighlightingLoader.Load(reader,
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance);
                }
            }

            textEditor.Text = ((ScriptEditorViewModel)DataContext)?.Script?.Content;

        }

        private void textEditor_TextChanged(object sender, EventArgs e)
        {
            ((ScriptEditorViewModel)DataContext).Script.Content = textEditor.Text;
        }
    }
}
