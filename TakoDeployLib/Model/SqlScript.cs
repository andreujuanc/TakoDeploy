using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployCore.Model
{
    public class SqlScriptFile : INotifyPropertyChanged
    {
        public ObservableCollection<SqlScriptContent> Scripts { get; set; }
        public string Name { get; set; }
        private string _content = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Content {
            get { return _content; }
            set
            {
                _content = value;
                Subtext.Scripting.ScriptSplitter splitter = new Subtext.Scripting.ScriptSplitter(_content);

                Scripts = new ObservableCollection<SqlScriptContent>();
                int counter = 0;
                foreach (var script in splitter)
                {
                    counter++;
                    Scripts.Add(new SqlScriptContent() { Name = Name + counter.ToString(), Content = script });
                }

            }
        }
        public int Size { get { return Content != null ? Content.Length : -1; } }

    }
    public class SqlScriptContent
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int Size { get { return Content != null ? Content.Length : -1; } }
    }
}
