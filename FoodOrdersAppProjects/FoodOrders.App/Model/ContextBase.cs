using FoodOrders.Model.Data;
using System.ComponentModel;

namespace FoodOrders.Model
{
    public class ContextBase : INotifyPropertyChanged
    {
        protected static object syncRoot = new object();

        public event PropertyChangedEventHandler PropertyChanged;

        private bool isReadOnly;

        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                if (CheckChanged(isReadOnly, value))
                {
                    isReadOnly = value;
                    OnPropertyChanged(nameof(IsReadOnly));
                }
            }
        }

        protected static bool CheckChanged<T>(T value, T newValue)
        {
            if (value == null
                    && newValue == null
                    || ReferenceEquals(value, newValue)
                    || value?.Equals(newValue) == true)
                return false;
            else
                return true;
        }

        public virtual void Cleanup()
        {
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            h?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ContextBase<TData>
        : ContextBase
        where TData : DataModelObjectBase, new()
    {
        private TData data;

        public TData Data
        {
            get { return data; }
            set
            {
                if (CheckChanged(data, value))
                {
                    TData oldValue = Data;
                    data = value;
                    OnPropertyChanged(nameof(Data));
                    OnPropertyChanged(nameof(HasData));
                    OnDataChanged(oldValue);
                }
            }
        }

        public bool HasData => Data != null;

        protected virtual void OnDataChanged(TData oldValue)
        {
        }
    }
}
