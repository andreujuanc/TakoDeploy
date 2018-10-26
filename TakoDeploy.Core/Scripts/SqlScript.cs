using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeploy.Core.Model;
using TakoDeploy.Core.Scripts;

namespace TakoDeploy.Core.Scripts
{
    public class ScriptFile   : Parsable
    {
        public ObservableCollection<ScriptContent> Scripts { get; set; }
        public string Name { get; set; }
        private string _content = null;
        private ScriptFile treeSelectedItem;

        public ScriptFile() : base(null)
        {

        }

        public ScriptFile(IScriptParser parser) : base(parser)
        {
            
        }

        public ScriptFile(IScriptParser parser, ScriptFile original) : this(parser)
        {
            CopyFrom(original);
        }

        public void CopyFrom(ScriptFile original)
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

                Scripts = new ObservableCollection<ScriptContent>();
                int index = 0;
                foreach (var script in splitter)
                {                    
                    Scripts.Add(new ScriptContent(Parser) { Index =index, Name = Name + (index + 1).ToString(), Content = script });
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
    public class ScriptContent : Parsable
    {
        public ScriptContent(IScriptParser parser) : base(parser)
        {
            
        }

        public ScriptContent(IScriptParser parser, string content) : this(parser)
        {
            
        }

        private string _content = null;
        public int Index { get; set; }

        protected bool _isValid = false;
        public bool IsValid { get => _isValid; set { SetField(ref _isValid, value); } } //TODO: Notifier

        public string Name { get; set; }

        public string Content
        {
            get { return _content; }
            set
            {
                SetField(ref _content, value);
                IsValid = Parse(_content);
            }
        }
        public int Size { get { return _content != null ? _content.Length : -1; } }
    }

    public class Parsable : Notifier
    {
        protected readonly IScriptParser Parser;
        public Parsable(IScriptParser parser)
        {
            Parser = parser;
        }

        protected bool IsValidScript { get { return _isValidScript; } private set { SetField(ref _isValidScript, value); } }
        protected bool _isValidScript = false;
        public List<ScriptError> ScriptErrors { get; private set; } = new List<ScriptError>();
        protected bool Parse(string content) => IsValidScript = Parser?.Parse(content) ?? true;
    }
    public class ScriptError
    {
        //public ParseError ParseError { get; set; }
        public string Error { get; set; }
        public Exception Exception { get; set; }
    }
    public class ScriptFileException : Exception
    {

        public ScriptFileException(List<ScriptError> scriptErrors)
        {
            this.Errors = scriptErrors;
        }

        public List<ScriptError> Errors { get; set; }
    }
}
