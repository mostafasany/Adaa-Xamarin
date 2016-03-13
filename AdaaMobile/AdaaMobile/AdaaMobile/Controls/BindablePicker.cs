using System;
using System.Collections;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class BindablePicker : Picker
    {

        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<BindablePicker, IEnumerable>(p => p.ItemsSource, default(IEnumerable), propertyChanged:OnItemsSourceChanged);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

      
        public static BindableProperty SelectedItemProperty =
            BindableProperty.Create<BindablePicker, object>(o => o.SelectedItem, default(object), propertyChanged:OnSelectedItemChanged);

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        private static void OnItemsSourceChanged(BindableObject bindable, IEnumerable oldvalue, IEnumerable newvalue)
        {
            var picker = bindable as BindablePicker;
            if (picker != null)
            {
                picker.Items.Clear();
                if (newvalue != null)
                {
                    //now it works like "subscribe once" but you can improve
                    foreach (var item in newvalue)
                    {
                        picker.Items.Add(item.ToString());
                    }
                }
            }
        }

        private void OnSelectedIndexChanged(object sender, EventArgs eventArgs)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Items[SelectedIndex];
            }
        }
        private static void OnSelectedItemChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var picker = bindable as BindablePicker;
            if (newvalue != null)
            {
                if (picker != null) picker.SelectedIndex = picker.Items.IndexOf(newvalue.ToString());
            }
        }
    }
}

