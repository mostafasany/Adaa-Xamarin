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

        public void RaiseItemTapped(HorizontaListItemTappedEventArgs args)
        {
            if (args == null || args.View == null) return;

            if (ItemTappedCommand != null)
            {
                ItemTappedCommand.Execute(args.Item);
            }

            OnItemTapped(args);
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
