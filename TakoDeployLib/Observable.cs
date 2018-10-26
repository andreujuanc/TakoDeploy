using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace TakoDeployLib.Model
{
    public class ObservableCollectionEx<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        // this collection also reacts to changes in its components' properties

        //public ObservableCollectionEx() : base()
        //{
        //    this.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ObservableCollectionEx_CollectionChanged);
        //}

        //void ObservableCollectionEx_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        //{
        //    if (e.Action == NotifyCollectionChangedAction.Remove)
        //    {
        //        foreach (T item in e.OldItems)
        //        {
        //            //Removed items
        //            item.PropertyChanged -= EntityViewModelPropertyChanged;
        //        }
        //    }
        //    else if (e.Action == NotifyCollectionChangedAction.Add)
        //    {
        //        foreach (T item in e.NewItems)
        //        {
        //            //Added items
        //            item.PropertyChanged += EntityViewModelPropertyChanged;
        //        }
        //    }
        //}

        //public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    //This will get called when the property of an object inside the collection changes - note you must make it a 'reset' - dunno why
        //    NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, this);
        //    OnCollectionChanged(args);
        //}


        //SECOND OPTION



        //private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        //public ObservableCollectionEx()
        //{
        //}

        //public ObservableCollectionEx(IEnumerable<T> list)
        //: base(list)
        //{
        //}

        //protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        //{
        //    if (SynchronizationContext.Current == _synchronizationContext)
        //    {
        //        // Execute the CollectionChanged event on the current thread
        //        RaiseCollectionChanged(e);
        //    }
        //    else
        //    {
        //        // Raises the CollectionChanged event on the creator thread
        //        _synchronizationContext.Send(RaiseCollectionChanged, e);
        //    }
        //}

        //private void RaiseCollectionChanged(object param)
        //{
        //    // We are in the creator thread, call the base implementation directly
        //    base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        //}

        //protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        //{
        //    if (SynchronizationContext.Current == _synchronizationContext)
        //    {
        //        // Execute the PropertyChanged event on the current thread
        //        RaisePropertyChanged(e);
        //    }
        //    else
        //    {
        //        // Raises the PropertyChanged event on the creator thread
        //        _synchronizationContext.Send(RaisePropertyChanged, e);
        //    }
        //}

        //private void RaisePropertyChanged(object param)
        //{
        //    // We are in the creator thread, call the base implementation directly
        //    base.OnPropertyChanged((PropertyChangedEventArgs)param);
        //}




//THIRD OPTION


        public override event NotifyCollectionChangedEventHandler CollectionChanged;
        private static object _syncLock = new object();

        public ObservableCollectionEx()
        {
            enableCollectionSynchronization(this, _syncLock);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            using (BlockReentrancy())
            {
                var eh = CollectionChanged;
                if (eh == null) return;

                var dispatcher = (from NotifyCollectionChangedEventHandler nh in eh.GetInvocationList()
                                  let dpo = nh.Target as DispatcherObject
                                  where dpo != null
                                  select dpo.Dispatcher).FirstOrDefault();

                if (dispatcher != null && dispatcher.CheckAccess() == false)
                {
                    dispatcher.Invoke(DispatcherPriority.DataBind, (Action)(() => OnCollectionChanged(e)));
                }
                else
                {
                    foreach (NotifyCollectionChangedEventHandler nh in eh.GetInvocationList())
                        nh.Invoke(this, e);
                }
            }
        }

        private static void enableCollectionSynchronization(IEnumerable collection, object lockObject)
        {
            var method = typeof(BindingOperations).GetMethod("EnableCollectionSynchronization",
                                    new Type[] { typeof(IEnumerable), typeof(object) });
            if (method != null)
            {
                // It's .NET 4.5
                method.Invoke(null, new object[] { collection, lockObject });
            }
        }

    }
}
