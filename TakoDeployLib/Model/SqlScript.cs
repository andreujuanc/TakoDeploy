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
    public class SqlScriptFile : SqlParsable
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

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                Parse(_content);
                Subtext.Scripting.ScriptSplitter splitter = new Subtext.Scripting.ScriptSplitter(_content);

                Scripts = new ObservableCollection<SqlScriptContent>();
                int index = 0;
                foreach (var script in splitter)
                {                    
                    Scripts.Add(new SqlScriptContent() { Index =index, Name = Name + (index + 1).ToString(), Content = script });
                    index++;
                }

            }
        }
        public int Size { get { return Content != null ? Content.Length : -1; } }

        public bool IsValid { get
            {
                //return Scripts != null ? Scripts.Where(x => !x.IsValid).Count() == 0 : false;
                if (Scripts == null) return false;
                var invalidFiles = Scripts.Where(x => !x.IsValid);
                if (invalidFiles.Count() > 0) return false;

                return true;
            }
        }
    }
    public class SqlScriptContent : SqlParsable
    {
        private string _content = null;
        public int Index { get; set; }
        public bool IsValid { get { return IsValidScript; } }

        public string Name { get; set; }
        public string Content
        {
            get { return _content; }
            set
            {
                SetField(ref _content, value);
                Parse(_content);
            }
        }
        public int Size { get { return _content != null ? _content.Length : -1; } }
    }

    public class SqlParsable : Notifier
    {
        protected bool IsValidScript { get { return _isValidScript; } private set { SetField(ref _isValidScript, value); } }
        protected bool _isValidScript = false;
        public List<SqlScriptError> ScriptErrors { get; private set; } = new List<SqlScriptError>();
        protected List<string> Parse(string sql)
        {
            var parser = new Microsoft.SqlServer.TransactSql.ScriptDom.TSql130Parser(false);
            ScriptErrors.Clear();
            var resultValid = true;
            IList<ParseError> errors;
            var fragment = parser.Parse(new StringReader(sql), out errors);
            if (errors != null && errors.Count > 0)
            {
                List<string> errorList = new List<string>();
                foreach (var error in errors)
                {
                    ScriptErrors.Add(new SqlScriptError() { ParseError = error });
                }
                resultValid = false;
                return errorList;
            }
            var tsql = fragment as Microsoft.SqlServer.TransactSql.ScriptDom.TSqlScript;
            if (tsql != null && tsql.Batches != null)
            {
                foreach (var batch in tsql.Batches)
                {
                    if (batch.Statements != null) foreach (var statement in batch.Statements)
                        {
                            if (statement is UseStatement)
                                resultValid = false;
                        }
                }
            }
            IsValidScript = resultValid;
            return null;
        }
    }
    public class SqlScriptError
    {
        public ParseError ParseError { get; set; }
        public string Error { get; set; }
        public Exception Exception { get; set; }
    }
    public class SqlScriptFileException : Exception
    {

        public SqlScriptFileException(List<SqlScriptError> scriptErrors)
        {
            this.Errors = scriptErrors;
        }

        public List<SqlScriptError> Errors { get; set; }
    }
}
