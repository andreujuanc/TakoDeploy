using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TakoDeploy.Core;
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
        private static object _lock2 = new object();

        public DeploymentViewViewModel ViewModel { get { return DataContext as DeploymentViewViewModel; } }

        public DeploymentView()
        {
            InitializeComponent();
            this.Loaded += DeploymentView_Loaded;
            //BindingOperations.EnableCollectionSynchronization(Deployment.Targets, _lock);
            DocumentManager.OnNewDocument += DocumentManager_OnNewDocument;
            this.DataContext = new DeploymentViewViewModel();
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            SetUI(); //MEH
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetUI();
        }

        private void DocumentManager_OnNewDocument(object sender, EventArgs e)
        {
            BindingOperations.EnableCollectionSynchronization(DocumentManager.Current?.Deployment?.Targets, _lock);
            dataGrid.DataContext = DocumentManager.Current?.Deployment?.Targets;
            
        }

        private void DeploymentView_Loaded(object sender, RoutedEventArgs e)
        {
                       
        }

        private void grid_rowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((System.Windows.FrameworkElement)sender).DataContext is TakoDeploy.Core.Model.TargetDatabase target && target != null)
            {
                var messages = target.Messages;
                BindingOperations.EnableCollectionSynchronization(messages, _lock2);
                gridMessages.DataContext = messages;
                ViewModel.MessagesState = true;
            }
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var exclude = new string[] { "Context", "ConnectionString", "ProviderName", "Messages", "DeploymentState" };
            e.Cancel =  exclude.Contains(e.PropertyName);
            if (e.PropertyName == "Selected")
                e.Column.DisplayIndex = 0;
        }

        private void gridMessages_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var exclude = new string[] { "SqlError", "Exception" };
            e.Cancel = exclude.Contains(e.PropertyName);            
                
        }

        private void btnMessageState_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MessagesState = !ViewModel.MessagesState;
           
        }

        private void SetUI()
        {
            gridSplitter.IsEnabled = ViewModel.MessagesState;
            if (ViewModel.MessagesState)
            {
                contentgrid.RowDefinitions[0].Height = new GridLength(0.7, GridUnitType.Star);
                contentgrid.RowDefinitions[2].Height = new GridLength(0.3, GridUnitType.Star);
                gridMessages.Visibility = Visibility.Visible;
            }
            else
            {
                contentgrid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                contentgrid.RowDefinitions[2].Height = new GridLength(40, GridUnitType.Pixel);
                gridMessages.Visibility = Visibility.Hidden;
            }
        }
    }

    public class DeploymentViewViewModel: TakoDeploy.Core.Model.Notifier
    {
        public PackIconKind MessageStateIconKind { get { return MessagesState ? PackIconKind.ArrowDown : PackIconKind.ArrowUp; } }
        private bool _messageState;
        public bool MessagesState
        {
            get { return _messageState; }
            set
            {
               
                SetField(ref _messageState, value);
                OnPropertyChanged("MessageStateIconKind");
            }
        }
    }
}
