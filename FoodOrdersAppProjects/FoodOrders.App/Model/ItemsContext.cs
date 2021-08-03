using FoodOrders.Model.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace FoodOrders.Model
{
    public class ItemsContext<TData> : ContextBase
    {
        #region Commands

        private class SelectItemCommandImpl : CommandBase<ItemsContext<TData>>
        {
            public SelectItemCommandImpl(ItemsContext<TData> context)
                : base(context)
            {
            }

            public override bool CanExecute(object parameter)
            {
                return Context.CanExecuteSelectItemCommand(parameter);
            }

            public override void Execute(object parameter)
            {
                Context.ExecuteSelectItemCommand(parameter);
            }
        }

        #endregion

        private ObservableCollection<TData> ItemsCollection { get; } = new ObservableCollection<TData>();

        public int Count => ItemsCollection.Count;

        public bool HasItems => ItemsCollection.Count > 0;

        public bool IsEmpty => !HasItems;

        public TData this[int index] => Items.ToList()[index];

        protected IEnumerable<TData> AllItems { get => ItemsCollection; }

        //note: override to apply e.g. filtering
        public virtual IEnumerable<TData> Items => AllItems;

        private TData selectedItem;
        public TData SelectedItem
        {
            get => selectedItem;
            set
            {
                if (CheckChanged(selectedItem, value))
                {
                    TData oldItem = selectedItem;
                    selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    OnPropertyChanged(nameof(HasSelectedItem));
                    SelectItemCommand.RaiseCanExecuteChanged();
                    OnSelectedItemChanged(oldItem);
                }
            }
        }

        public bool HasSelectedItem => SelectedItem != null;

        public CommandBase SelectItemCommand { get; }

        public ItemsContext()
            : this(new List<TData>())
        {
        }

        public ItemsContext(IEnumerable<TData> items)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            SelectItemCommand = new SelectItemCommandImpl(this);

            foreach (TData obj in items)
                ItemsCollection.Add(obj);

            UpdateModelState();
        }

        public bool Remove(TData data)
        {
            bool clear = ReferenceEquals(SelectedItem, data);
            bool result = ItemsCollection.Remove(data);
            if (clear)
                SelectedItem = default;

            return result;
        }

        public void RemoveFirst()
        {
            TData data = ItemsCollection.First();
            bool clear = ReferenceEquals(SelectedItem, data);
            ItemsCollection.RemoveAt(0);
            if (clear)
                SelectedItem = default;
        }

        public void RemoveLast()
        {
            TData data = ItemsCollection.Last();
            Remove(data);
        }

        public override void Cleanup()
        {
            Debug.WriteLine($"{nameof(Cleanup)}", GetType().Name);
            ItemsCollection.Clear();
            SelectedItem = default;
            UpdateModelState();
            CleanupPostAction();
        }

        protected virtual void CleanupPostAction()
        {
        }

        protected virtual void UpdateItemsPostAction()
        {
        }

        public void UpdateItems(IEnumerable<TData> items)
        {
            Debug.WriteLine($"{nameof(UpdateItems)}:{items}", GetType().Name);

            if (items == null)
                throw new ArgumentNullException(nameof(items));

            if (ItemsCollection.Count > 0)
                Cleanup();

            foreach (TData obj in items)
                AddData(obj, false);

            UpdateItemsPostAction();
            UpdateModelState();
        }

        public void UpdateItems(IEnumerable<TData> items, TData selectedItem)
        {
            UpdateItems(items);
            SelectedItem = selectedItem;
        }

        protected virtual void AddDataPostAction(TData item)
        {
        }

        public void AddData(TData item)
        {
            AddData(item, true);
        }

        private void AddData(TData item, bool updateState)
        {
            ItemsCollection.Add(item);
            AddDataPostAction(item);
            if (updateState)
                UpdateModelState();
        }

        protected virtual void InsertDataPostAction(TData item)
        {
        }

        protected virtual TData FindItem(TData item)
        {
            return Items.FirstOrDefault(o => ReferenceEquals(o, item));
        }

        public void InsertData(TData item)
        {
            InsertData(0, item, true);
        }

        public void InsertData(int index, TData item)
        {
            InsertData(index, item, true);
        }

        protected void InsertData(int index, TData item, bool updateModelState)
        {
            ItemsCollection.Insert(index, item);
            InsertDataPostAction(item);
            if (updateModelState)
                UpdateModelState();
        }

        void UpdateModelState()
        {
            Debug.WriteLine($"{nameof(UpdateModelState)}", GetType().Name);

            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(HasItems));
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(HasSelectedItem));
            OnPropertyChanged(nameof(SelectedItem));

            SelectItemCommand.RaiseCanExecuteChanged();
        }

        protected virtual void OnSelectedItemChanged(TData oldItem)
        {
        }

        protected virtual bool CanExecuteSelectItemCommand(object parameter)
        {
            return parameter != null;
        }

        protected virtual void ExecuteSelectItemCommand(object parameter)
        {
            SelectedItem = (TData)parameter;
            OnSelectItemCommandExecuted();
        }

        protected virtual void OnSelectItemCommandExecuted()
        {
        }
    }

    public class ItemsContext<TData, TContext> : ItemsContext<TData>
       where TData : DataModelObjectBase, new()
       where TContext : ContextBase<TData>, new()
    {
        public TContext SelectedContext
        {
            get  => ItemsContexts.SelectedItem;
            private set
            {
                TContext oldContext = SelectedContext;
                ItemsContexts.SelectedItem = value;
                OnPropertyChanged(nameof(SelectedContext));
                OnSelectedContextChanged(oldContext);
            }
        }

        private ItemsContext<TContext> ItemsContexts { get; }

        public ItemsContext()
        {
            ItemsContexts = new ItemsContext<TContext>();
        }

        public override void Cleanup()
        {
            ItemsContexts.Cleanup();
            base.Cleanup();
        }

        protected override void OnSelectedItemChanged(TData oldItem)
        {
            SelectContext(SelectedItem);
        }

        //todo: revise
        protected virtual int ContextsCashSize { get; } = 100;
        private void SelectContext(TData item)
        {
            TContext context = ItemsContexts.Items.FirstOrDefault(o => item.Equals(o.Data));
            if (context == null)
                context = CreateContext(item);

            if (ItemsContexts.Count > 0
                && ItemsContexts.Count > ContextsCashSize)
                ItemsContexts.RemoveLast();

            ItemsContexts.InsertData(context);
            ItemsContexts.SelectedItem = context;
            SelectedContext = context;
        }

        protected virtual void OnSelectedContextChanged(TContext oldContext)
        {
        }

        protected virtual TContext CreateContext(TData item)
        {
            return new TContext { Data = item };
        }
    }
}
