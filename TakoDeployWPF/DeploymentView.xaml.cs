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
            
           
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Context")
                e.Cancel = true;
            if (e.PropertyName == "ConnectionString")
                e.Cancel = true;
            if (e.PropertyName == "ProviderName")
                e.Cancel = true;
            if (e.PropertyName == "Messages")
                e.Cancel = true;
            if (e.PropertyName == "State")
                e.Cancel = true;
        }
    }
}
