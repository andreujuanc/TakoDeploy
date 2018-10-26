using System;
using System.Collections.Generic;
using System.Text;
using TakoDeploy.Core.Data.Context;

namespace TakoDeploy.Core.Scripts
{
    public interface IScriptParserFactory
    {
        IScriptParser GetParser(ProviderTypes providerType);
    }
}
