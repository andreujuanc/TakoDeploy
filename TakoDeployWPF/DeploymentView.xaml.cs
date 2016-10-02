using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TakoDeployCore;
using TakoDeployCore.Model;
using TakoDeployLib.Model;

namespace TakoDeployWPF
{
    /// <summary>
    /// Lógica de interacción para DeploymentView.xaml
    /// </summary>
    public partial class DeploymentView : UserControl
    {
        public Deployment Deployment { get; set; }
        private static object _lock = new object();

        public DeploymentView()
        {
            InitializeComponent();
            this.Loaded += DeploymentView_Loaded;
            //BindingOperations.EnableCollectionSynchronization(Deployment.Targets, _lock);
            DocumentManager.OnNewDocument += DocumentManager_OnNewDocument;
            
        }

        private void DocumentManager_OnNewDocument(object sender, EventArgs e)
        {
            BindingOperations.EnableCollectionSynchronization(DocumentManager.Current?.Deployment?.Targets, _lock);
            dataGrid.DataContext = DocumentManager.Current?.Deployment?.Targets;
        }

        private void DeploymentView_Loaded(object sender, RoutedEventArgs e)
        {
            
            
            //Deployment.Sources.Add(new TakoDeployCore.Model.SourceDatabase()
            //{
            //    ConnectionString = @"Data Source=des.wgm.es\DES_2014;Initial Catalog=Pruebas_1 ;User ID=wgm ;Password=cafeina;",
            //    Name = "Pruebas1",
            //    ProviderName = "System.Data.SqlClient",
            //    Type = SourceType.DataSource
            //});
           
        }
    }
}
