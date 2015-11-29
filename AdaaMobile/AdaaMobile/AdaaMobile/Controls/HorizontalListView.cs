using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class HorizontalListView : ContentView, IVisualElementController, IElementController
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

        public void SetValueFromRenderer(BindableProperty property, object value)
        {
            //throw new NotImplementedException();
        }

        public void SetValueFromRenderer(BindablePropertyKey propertyKey, object value)
        {
            //throw new NotImplementedException();
        }

        public void NativeSizeChanged()
        {
            //throw new NotImplementedException();
        }


    }
}
