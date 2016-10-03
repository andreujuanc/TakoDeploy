using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TakoDeployCore.Model;

namespace TakoDeployCore
{
    public class ScriptLoader
    {
        public async Task<ObservableCollection<SqlScriptFile>> LoadFromFiles(params string[] files)
        {
            var result = new ObservableCollection<SqlScriptFile>();
            foreach (var filePath in files)
            {
                string fileContent = null;
                var finfo = new FileInfo(filePath);
                string fileName = finfo.Name.Replace(finfo.Extension, "");
                using (StreamReader reader = new StreamReader(filePath))
                {
                    fileContent = await reader.ReadToEndAsync();
                }

                result.Add(new SqlScriptFile() { Name = fileName, Content = fileContent });
            }
            return result;
        }
    }
}
