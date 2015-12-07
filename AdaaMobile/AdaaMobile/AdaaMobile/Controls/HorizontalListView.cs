using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class HorizontalListView : ContentView
    {

        #region ItemTemplate
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create<HorizontalListView, DataTemplate>(p => p.ItemTemplate, default(DataTemplate));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        #endregion

        #region ItemSource
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create<HorizontalListView, IEnumerable>(p => p.ItemsSource, default(IEnumerable));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region ItemTappedCommand   
        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create<HorizontalListView, ICommand>(p => p.ItemTappedCommand, default(ICommand));
        public ICommand ItemTappedCommand
        {
            get { return (ICommand)GetValue(ItemTappedCommandProperty); }
            set { SetValue(ItemTappedCommandProperty, value); }
        }
        #endregion

        #region ItemWidth
        public static readonly BindableProperty ItemWidthProperty = BindableProperty.Create<HorizontalListView, double>(p => p.ItemWidth, default(double));

        public double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }
        #endregion

        #region ItemHeight
        public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create<HorizontalListView, double>(p => p.ItemHeight, default(double));
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        #endregion

        #region ColumnSpacing
        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create<HorizontalListView, double>(p => p.ColumnSpacing, default(double));

        public double ColumnSpacing
        {
            get { return (double)GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }
        #endregion

        #region HasFixedItemSize
        public static readonly BindableProperty HasFixedItemSizeProperty = BindableProperty.Create<HorizontalListView, bool>(p => p.HasFixedItemSize, default(bool));

        public bool HasFixedItemSize
        {
            get { return (bool)GetValue(HasFixedItemSizeProperty); }
            set { SetValue(HasFixedItemSizeProperty, value); }
        }
        #endregion

        public void RaiseItemTapped(HorizontaListItemTappedEventArgs args)
        {
            if (args == null || args.View == null) return;

            if (ItemTappedCommand != null)
            {
                ItemTappedCommand.Execute(args.Item);
            }

            OnItemTapped(args);
        }

        public void UpdateLayout()
        {
            InvalidateMeasure();
            InvalidateLayout();
        }

        #region Custom Events

        public event EventHandler<HorizontaListItemTappedEventArgs> ItemTapped;

        protected virtual void OnItemTapped(HorizontaListItemTappedEventArgs e)
        {
            var handler = ItemTapped;
            if (handler != null) handler.Invoke(this, e);
        }

        #endregion
    }

    public class HorizontaListItemTappedEventArgs : EventArgs
    {
        public View View { get; private set; }
        public object Item { get; private set; }

        public HorizontaListItemTappedEventArgs(View view, object item)
        {
            View = view;
            Item = item;
        }
    }
}
