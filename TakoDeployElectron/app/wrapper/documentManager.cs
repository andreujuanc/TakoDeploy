using System;
using System.Data;
using System.Threading.Tasks;
using TakoDeployCore;
using TakoDeployLib;
using TakoDeployCore.Model;
using TakoDeployLib.Model;

public class Startup
{
    public async Task<object> Invoke(object input)
    {
        DocumentManager.Current  = new DocumentManager();
        return DocumentManager.Current ;
    }

    public async Task<object> AddSource(object input)
    {
        var Source = new SourceDatabase();
        Source.ConnectionString = "Data Source=abismo-net.com, 28760;Initial Catalog=WgmControl ;User ID=WgmControl ;Password=w2JWYJKtd6S#R4p;MultipleActiveResultSets=True; Max Pool Size=100;Connection Timeout=20";
        Source.ProviderName = "System.Data.SqlClient";
        Source.Type = SourceType.DataSource;
        Source.Name = "Production Server";
        //if(!await Source.TryConnect())
        //    return false;
       // Source.PopulateTargets();
        
        DocumentManager.Current.Deployment.Sources.Add(Source);
        return DocumentManager.Current;
    }

    public async Task<object> Validate(dynamic input)
    {
        //new Progress();
        var progress = new Progress<ProgressEventArgs>((a)=>{
            input.event_handler(a);
        });
        
        await DocumentManager.Current.Deployment.ValidateAsync(progress);
        return DocumentManager.Current;
    }
}
