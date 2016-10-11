using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TakoDeployXUnit.Deployment
{
    public static class ScriptManager
    {
        public static string GetInvalidScript1()
        {
            return Read("InvalidScript1.sql");
        }
        public static string GetNorthwind()
        {
            return Read("Database1Model.sql");
        }
        private static string Read(string filename)
        {
            var result = "";
            var assembly = Assembly.GetExecutingAssembly();
            var manifest = assembly.GetManifestResourceStream($"TakoDeployXUnit.TestDbScripts.{filename}");
            if (manifest == null) return result;
            using (var sr = new StreamReader(manifest))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
