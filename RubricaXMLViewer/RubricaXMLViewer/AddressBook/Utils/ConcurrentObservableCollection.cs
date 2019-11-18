using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace RubricaXMLViewer.AddressBook.Utils
{

    public class ConcurrentObservableCollection<T> : IList<T>, INotifyCollectionChanged
    {
        private List<T> internalList;

        private readonly object lockList = new object();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public ConcurrentObservableCollection()
        {
            internalList = new List<T>();
        }

        public ConcurrentObservableCollection(IEnumerable<T> initialCollection)
        {
            internalList = new List<T>(initialCollection);
        }

        public ConcurrentObservableCollection(IList<T> initialList)
        {
            internalList = new List<T>(initialList);
        }

        // Other Elements of IList implementation

        public IEnumerator<T> GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Clone().GetEnumerator();
        }

        public List<T> Clone()
        {
            ThreadLocal<List<T>> threadClonedList = new ThreadLocal<List<T>>();

            lock (lockList)
            {
                internalList.ForEach(element => { threadClonedList.Value.Add(element); });
            }
            threadClonedList.Dispose();
            return (threadClonedList.Value);
        }

        public void Add(T item)
        {
            lock (lockList)
            {
                internalList.Add(item);
            }
            NotifyPropertyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public bool Remove(T item)
        {
            bool isRemoved;

            lock (lockList)
            {
                isRemoved = internalList.Remove(item);
            }

            NotifyPropertyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove));
            return (isRemoved);
        }

        public void Clear()
        {
            lock (lockList)
            {
                internalList.Clear();
                NotifyPropertyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public bool Contains(T item)
        {
            bool containsItem;

            lock (lockList)
            {
                containsItem = internalList.Contains(item);
            }

            return (containsItem);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (lockList)
            {
                internalList.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                int count;

                lock ((lockList))
                {
                    count = internalList.Count;
                }

                return (count);
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(T item)
        {
            int itemIndex;

            lock ((lockList))
            {
                itemIndex = internalList.IndexOf(item);
            }

            return (itemIndex);
        }

        public void Insert(int index, T item)
        {
            lock ((lockList))
            {
                internalList.Insert(index, item);
                NotifyPropertyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }
        }

        public void RemoveAt(int index)
        {
            lock ((lockList))
            {
                internalList.RemoveAt(index);
                NotifyPropertyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, index));
            }
        }

        public T this[int index]
        {
            get
            {
                lock ((lockList))
                {
                    return internalList[index];
                }
            }
            set
            {
                lock ((lockList))
                {
                    internalList[index] = value;
                    NotifyPropertyChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value));
                }
            }
        }

        private void NotifyPropertyChanged(NotifyCollectionChangedEventArgs e)
        {
            //Application.Current.Dispatcher.Invoke(()=>
            CollectionChanged?.Invoke(this, e);
            //);
        }
    }
}
