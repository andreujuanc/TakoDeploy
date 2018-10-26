using System;
using System.Collections.Generic;
using System.Text;

namespace TakoDeploy.Core.Scripts
{
    public interface IScriptParser
    {
        bool Parse(string content);
    }
}
