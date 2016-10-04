using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployCore.Model
{
    public class SqlScriptFile : Notifier
    {
        public ObservableCollection<SqlScriptContent> Scripts { get; set; }
        public string Name { get; set; }
        private string _content = null;
        private SqlScriptFile treeSelectedItem;

        public SqlScriptFile()
        {


        }

        public SqlScriptFile(SqlScriptFile original) : base()
        {
            CopyFrom(original);
        }

        public void CopyFrom(SqlScriptFile original)
        {
            this.Name = original.Name;
            this.Content = original.Content;
        }

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

        public bool IsValid { get
            {
                return Scripts != null ?  Scripts.Where(x => !x.IsValid).Count() == 0 : false;
            }
        }
    }
    public class SqlScriptContent :Notifier
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int Size { get { return Content != null ? Content.Length : -1; } }

        private bool _isValid = false;
        public bool IsValid { get { return _isValid; } set { SetField(ref _isValid, value); } }

        internal List<string> Validate()
        {
            return Parse(Content);
        }

        private List<string> Parse(string sql)
        {
            var parser = new Microsoft.SqlServer.TransactSql.ScriptDom.TSql130Parser(false);
            IList<ParseError> errors;
            var fragment = parser.Parse(new StringReader(sql), out errors);
            if (errors != null && errors.Count > 0)
            {
                List<string> errorList = new List<string>();
                foreach (var error in errors)
                {
                    errorList.Add(error.Message);
                }
                return errorList;
            }
            return null;
        }
    }
}
