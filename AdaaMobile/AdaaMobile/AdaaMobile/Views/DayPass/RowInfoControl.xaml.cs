using AdaaMobile.Controls;
using Xamarin.Forms;

namespace AdaaMobile.Views.DayPass
{
    public partial class RowInfoControl : GestureContentView
    {
        public RowInfoControl()
        {
            InitializeComponent();
        }

        #region Title property


        public static readonly BindableProperty TitleProperty = BindableProperty.Create<RowInfoControl, object>(p => p.Title, default(object));

        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region Value property


        public static readonly BindableProperty ValueProperty = BindableProperty.Create<RowInfoControl, object>(p => p.Value, default(object));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion


    }
}
