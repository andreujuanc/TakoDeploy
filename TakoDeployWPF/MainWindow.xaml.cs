using Squirrel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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
using TakoDeployWPF.Domain;
using TakoDeployWPF.Misc;

namespace TakoDeployWPF
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel DataContextModel { get { return DataContext as MainViewModel; } }
        public readonly Updater updater = new Updater();
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit; ;
            this.DataContext = new MainViewModel();
            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += MainWindow_SizeChanged;
           
            Console.WriteLine("Main Window Ctor");

        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            updater.DisposeUpdateManager(sender, e);
        }


        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DataContextModel.WindowSize = e.NewSize;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await updater.Start(onFirstRun: () => DataContextModel.ShowTheWelcomeWizard = true);

            // ItemToContextMenuConverter.FirstLevelContextMenu = this.Resources["FirstLevelContextMenu"] as ContextMenu;
            //  ItemToContextMenuConverter.SecondLevelContextMenu = this.Resources["SecondLevelContextMenu"] as ContextMenu;
        }

        private void TreeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataContextModel.TreeSelectedItem = MainTreeView.SelectedItem;
        }

        private void MainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DataContextModel.TreeSelectedItem = MainTreeView.SelectedItem;
        }
    }
    public class MainViewModel : Notifier
    {
        private List<object> treeViewData;
        public string DeploymentMessage { get; set; }
        public MainViewModel()
        {
            treeViewData = new List<object>() {
                    new TreeItemSourcesDataContext(),
                    new TreeItemScriptDataContext()
                };
            DocumentManager.OnNewDocument += DocumentManager_OnNewDocument;
        }

        private void DocumentManager_OnNewDocument(object sender, EventArgs e)
        {
            DocumentManager.Current.DeploymentEvent += Current_DeploymentEvent;
            OnPropertyChanged("TreeViewData");
        }

        private void Current_DeploymentEvent(object sender, TakoDeployLib.Model.ProgressEventArgs e)
        {
            if (e.Exception != null)
            {
                var ex2 = e.Exception;
                if (ex2.InnerException != null)
                    ex2 = ex2.InnerException;
                MessageBox.Show(ex2.Message, "TakoDeploy Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                (Microsoft.HockeyApp.HockeyClient.Current as Microsoft.HockeyApp.HockeyClient).HandleException(ex2);
            }
            if(e.Message != null)
                DeploymentMessage = e.Message;
            OnPropertyChanged(null);
        }

        public ICommand RunNewSourceDialogCommand => new ButtonCommand(ExecuteRunDialog, CanExecuteGenericButton);
        public ICommand RunNewScriptDialogCommand => new ButtonCommand(ExecuteNewScriptDialog, CanExecuteGenericButton);
        public ICommand RunValidateCommand => new ButtonCommand(ExecuteValidate, CanExecuteValidation);
        public ICommand RunDeployCommand => new ButtonCommand(ExecuteRunDeployCommand, CanExecuteDeployment);

        public ICommand RunEditSelectedItemCommand => new ButtonCommand(ExecuteEditSelectedItemCommand, IsTreeItemSelected);
        public ICommand RunDeleteSelectedItemCommand => new ButtonCommand(ExecuteDeleteSelectedItemCommand, IsTreeItemSelected);

        public ICommand RunNewDocumentCommand => new ButtonCommand(ExecuteNewDocumentCommand);
        public ICommand RunOpenDocumentCommand => new ButtonCommand(ExecuteOpenDocumentCommand);
        public ICommand RunSaveDocumentCommand => new ButtonCommand(ExecuteSaveDocumentCommand, CanExecuteGenericButton);

        public List<object> TreeViewData
        {
            get
            {
                return treeViewData;
            }
        }

        public object TreeSelectedItem
        {
            get;
            set;
        }

        private Size _windowSize;
        public Size WindowSize
        {
            get
            {
                return _windowSize;
            }
            internal set
            {
                _windowSize = value;
                OnPropertyChanged("WindowSize");
            }
        }

        public bool ShowTheWelcomeWizard
        {
            set
            {
                if (!value) return;
                var view = new WelcomeScreen();
                var result = MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            }
        }

        private async void ExecuteEditSelectedItemCommand(object o)
        {
            UserControl view = null;
            object editedObject = null;
            if (TreeSelectedItem is SourceDatabase)
            {
                editedObject = new SourceDatabase((SourceDatabase)TreeSelectedItem);
                view = new Domain.SourceEditorDialog
                {
                    DataContext = new SourceEditorViewModel() { Source = (SourceDatabase)editedObject }
                };
                view.Width = 480;
                view.Height = 600;

            }
            else if (TreeSelectedItem is SqlScriptFile)
            {
                editedObject = new SqlScriptFile((SqlScriptFile)TreeSelectedItem);
                view = new ScriptEditor
                {
                    DataContext = new ScriptEditorViewModel() { Script = (SqlScriptFile)editedObject }
                };
                view.Width = WindowSize.Width * 0.80;
                view.Height = WindowSize.Height * 0.75;
            }
            
            if (view == null) return;
           
          

            //show the dialog
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog");
            if (result is bool)
            {
                if ((bool)result)
                {

                    if (TreeSelectedItem is SourceDatabase)
                    {
                        ((SourceDatabase)TreeSelectedItem).CopyFrom(editedObject as SourceDatabase);
                    }
                    else if (TreeSelectedItem is SqlScriptFile)
                    {
                        ((SqlScriptFile)TreeSelectedItem).CopyFrom(editedObject as SqlScriptFile);
                    }
                }
            }
        }
        private async void ExecuteDeleteSelectedItemCommand(object o)
        {
            var result = MessageBox.Show("Are you sure you want to delete this source?", "TakoDeploy", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;
            if (TreeSelectedItem is SourceDatabase)
            {
                DocumentManager.Current.Deployment.Sources.Remove((SourceDatabase)TreeSelectedItem);
            }
        }


        private async void ExecuteRunDialog(object o)
        {
            var source = new SourceDatabase();
            
            var view = new Domain.SourceEditorDialog
            {
                DataContext = new SourceEditorViewModel() { Source = source }
            };
            view.Width = 480;
            view.Height = 600;

            //show the dialog
            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            if (result is bool)
            {
                if ((bool)result)
                {
                    DocumentManager.Current.Deployment.Sources.Add(source);
                }

                //check the result...
                Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            }
        }

        private async void ExecuteNewScriptDialog(object o)
        {
            var script = new SqlScriptFile();
            var basename = "SqlScript";
            script.Name = basename + (DocumentManager.Current.Deployment.ScriptFiles.Where(x => x.Name.StartsWith(basename)).Count() + 1).ToString();
            var view = new ScriptEditor
            {
                DataContext = new ScriptEditorViewModel() { Script = script }
            };
            view.Width = WindowSize.Width * 0.80;
            view.Height = WindowSize.Height * 0.75;

            var result = await MaterialDesignThemes.Wpf.DialogHost.Show(view, "RootDialog", ClosingEventHandler);
            if (result is bool)
            {
                if ((bool)result)
                {
                    DocumentManager.Current.Deployment.ScriptFiles.Add(script);
                }

                //check the result...
                Console.WriteLine("Dialog was closed, the CommandParameter used to close it was: " + (result ?? "NULL"));
            }
        }

        private async void ExecuteNewDocumentCommand(object o)
        {
            if (DocumentManager.Current != null && DocumentManager.Current.IsModified)
            {
                var result = MessageBox.Show("There are changes in your current deployment document. Do you want yo save them?", "TakoDeploy", MessageBoxButton.YesNoCancel);
                switch (result)
                {

                    case MessageBoxResult.Yes:
                        ExecuteSaveDocumentCommand(o);
                        break;
                    case MessageBoxResult.No:
                        break;
                    default:
                        return;//cancel commmand
                }
            }
            DocumentManager.Current = new DocumentManager();
        }

        private async void ExecuteOpenDocumentCommand(object o)
        {
            var saveDialog = new Microsoft.Win32.OpenFileDialog();
            saveDialog.Filter = "TakoDeploy Document (*.tdd)|*.tdd|All files (*.*)|*.*";
            var result = saveDialog.ShowDialog();
            if (!result.HasValue || (result.HasValue && !result.Value)) return;
            if (!System.IO.File.Exists(saveDialog.FileName)) return;
            //using (var streaem = new MemoryStream())
            using (var stream = new System.IO.StreamReader(saveDialog.OpenFile()))
            {
                var data = Newtonsoft.Json.Linq.JObject.Parse(stream.ReadToEnd());
                var deployment = data.ToObject<Deployment>();
                if (deployment != null)
                {
                    DocumentManager.Open(deployment, saveDialog.SafeFileName);
                }
                else
                {
                    throw new Exception("potato");
                }
            }
        }

        private async void ExecuteSaveDocumentCommand(object o)
        {
            var saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.Filter = "TakoDeploy Document (*.tdd)|*.tdd|All files (*.*)|*.*";
            var result = saveDialog.ShowDialog();
            if (!result.HasValue || (result.HasValue && !result.Value)) return;
            var name = saveDialog.SafeFileName;
            if (!name.EndsWith(".tdd")) name += ".tdd";
            //using (var streaem = new MemoryStream())
            using (var stream = new System.IO.StreamWriter(saveDialog.OpenFile()))
            {
                var data = Newtonsoft.Json.Linq.JObject.FromObject(DocumentManager.Current.Deployment);
                stream.Write(data.ToString());
                DocumentManager.Current.CurrentFileName = saveDialog.SafeFileName;
            }
            //DocumentManager.Save();
        }

        private async void ExecuteValidate(object o)
        {
            await DocumentManager.Current.Validate();
        }

        private void ClosingEventHandler(object sender, MaterialDesignThemes.Wpf.DialogClosingEventArgs eventArgs)
        {
            //if ((bool)eventArgs.Parameter)
            //{
            //    var source = ((eventArgs.Session.Content as System.Windows.FrameworkElement)?.DataContext as SourceEditorViewModel)?.Source;
            //    DocumentManager.Current.Deployment.Sources.Add(source);
            //    DocumentManager.Current.Deployment.CallPropertyChanges();
            //}
        }

        private async void ExecuteRunDeployCommand(object o)
        {
            await DocumentManager.Current.Deploy();
        }

        private bool IsTreeItemSelected(object o)
        {
            if (!DocumentIsPresent) return false;
            if (DeploymentIsRunning) return false;
            if ((TreeSelectedItem as SourceDatabase) == null)
            {
                if ((TreeSelectedItem as SqlScriptFile) == null)
                {
                    return false;
                }
            }

            return true;
        }
        private bool CanExecuteDeployment(object o)
        {
            if (!DocumentIsPresent) return false;
            if (!DocumentManager.Current.IsModified) return false;
            if (DeploymentIsRunning) return false;
            return true;
        }

        private bool CanExecuteValidation(object o)
        {
            if (!DocumentIsPresent) return false;
            if (DeploymentIsRunning) return false;
            return true;
        }
        private bool CanExecuteGenericButton(object o)
        {
            if (!DocumentIsPresent) return false;
            if (DeploymentIsRunning) return false;
            return true;
        }

        private bool DocumentIsPresent => (DocumentManager.Current != null);
        private bool DeploymentIsRunning => (DocumentManager.Current != null && DocumentManager.Current.Deployment != null && DocumentManager.Current.Deployment.Status == DeploymentStatus.Running);

    }

    public class TreeItemSourceDatabaseDataContext
    {
        public IEnumerable DataContext { get; set; }
        public string HeaderText { get; set; }
    }

    public abstract class TreeItemDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _headerText = "";
        public string HeaderText { get { return _headerText; } set { _headerText = value; OnPropertyChanged(); } }

    }

    public class TreeItemScriptDataContext : TreeItemDataContext
    {

        public TreeItemScriptDataContext()
        {
            HeaderText = "Scripts";
            DocumentManager.OnNewDocument += DocumentManager_OnNewDocument;
        }

        private void DocumentManager_OnNewDocument(object sender, EventArgs e)
        {
            DocumentManager.Current.Deployment.PropertyChanged += Deployment_PropertyChanged; ;
            DocumentManager.Current.Deployment.ScriptFiles.CollectionChanged += Scripts_CollectionChanged;
        }

        private void Deployment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("SubElements");
        }

        private void Scripts_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("SubElements");
        }

        public IEnumerable<SqlScriptFile> SubElements
        {
            get
            {
                if (DocumentManager.Current == null) return new List<SqlScriptFile>();
                if (DocumentManager.Current.Deployment == null) return new List<SqlScriptFile>();
                if (DocumentManager.Current.Deployment.ScriptFiles == null) return new List<SqlScriptFile>();
                return DocumentManager.Current?.Deployment?.ScriptFiles;
            }
            set { }
        }
    }

    public class TreeItemSourcesDataContext : TreeItemDataContext
    {

        public TreeItemSourcesDataContext()
        {
            HeaderText = "Sources";
            DocumentManager.OnNewDocument += DocumentManager_OnNewDocument;
        }

        private void DocumentManager_OnNewDocument(object sender, EventArgs e)
        {
            DocumentManager.Current.Deployment.PropertyChanged += Deployment_PropertyChanged; ;
            DocumentManager.Current.Deployment.Sources.CollectionChanged += Sources_CollectionChanged;
        }

        private void Deployment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("SubElements");
        }

        private void Sources_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("SubElements");
        }

        public IEnumerable<SourceDatabase> SubElements
        {
            get
            {
                if (DocumentManager.Current == null) return new List<SourceDatabase>();
                if (DocumentManager.Current.Deployment == null) return new List<SourceDatabase>();
                if (DocumentManager.Current.Deployment.Sources == null) return new List<SourceDatabase>();
                return DocumentManager.Current?.Deployment?.Sources;
            }
            set { }
        }
    }

    public class ButtonCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public ButtonCommand(Action<object> execute) : this(execute, null)
        {
            Console.WriteLine("ButtonCommand Ctor");
        }

        public ButtonCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null) throw new ArgumentNullException(nameof(execute));

            _execute = execute;
            _canExecute = canExecute ?? (x => true);
        }

        public bool CanExecute(object parameter)
        {
            ;
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Refresh()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    //public class TreeViewElement : INotifyPropertyChanged
    //{
    //    public event EventHandler<string> OnCommand;
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public string ImageLocation { get; set; }
    //    public string HeaderText { get { return  } }
    //    public string BackgroundColor { get; set; }
    //    public int Level { get; set; }
    //    public List<TreeViewElement> SubElements
    //    {
    //        get
    //        {
    //            var result = new List<TreeViewElement>();
    //            if (DataContext == null) return result;
    //            //if (!(DataContext is IEnumerable)) return result;

    //            foreach (var item in DataContext.DataContext as IEnumerable)
    //            {
    //                result.Add(new TreeViewElement() { });
    //            }
    //            return result;
    //        }
    //    }

    //    private MainViewModel.TreeItemDataContext _dataContext;
    //    public MainViewModel.TreeItemDataContext DataContext
    //    {
    //        get { return _dataContext; }
    //        set
    //        {
    //            if (_dataContext == value) return;
    //            if(_dataContext != null)
    //                _dataContext.PropertyChanged -= _dataContext_PropertyChanged;

    //            _dataContext = value;

    //            if (_dataContext != null)
    //                _dataContext.PropertyChanged += _dataContext_PropertyChanged;
    //        }
    //    }

    //    private void _dataContext_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    //    }

    //    private ICommand _ItemCommand;

    //    public ICommand ItemCommand
    //    {
    //        get
    //        {
    //            if (_ItemCommand == null)
    //            {
    //                _ItemCommand = new RelayCommand((o) =>
    //                {
    //                    OnCommand?.Invoke(this, "");
    //                });
    //            }
    //            return _ItemCommand;
    //        }
    //    }

    //}

    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion // Fields

        #region Constructors
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors
        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }

    //[ValueConversion(typeof(object), typeof(ContextMenu))]
    //public class ItemToContextMenuConverter : IValueConverter
    //{
    //    public static ContextMenu FirstLevelContextMenu;
    //    public static ContextMenu SecondLevelContextMenu;

    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        var item = value as TreeViewElement;
    //        if (item == null) return null;

    //        return item.Level == 0 ? FirstLevelContextMenu : SecondLevelContextMenu;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new Exception("The method or operation is not implemented.");
    //    }
    //}
}
