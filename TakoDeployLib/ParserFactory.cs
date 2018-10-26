using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TakoDeploy.Core.Data.Context;
using TakoDeploy.Core.Scripts;

namespace TakoDeployLib
{
    public class ParserFactory : TakoDeploy.Core.Scripts.IScriptParserFactory
    {
        public IScriptParser GetParser(ProviderTypes providerType)
        {
            switch (providerType)
            {
                case ProviderTypes.SqlServer:
                    return new SqlServer.SqlParser();
                case ProviderTypes.SqLite:
                    break;
                case ProviderTypes.MySql:
                    break;
                case ProviderTypes.PostgreSql:
                    break;
                  
            }
            return null;
        }
    }
}
