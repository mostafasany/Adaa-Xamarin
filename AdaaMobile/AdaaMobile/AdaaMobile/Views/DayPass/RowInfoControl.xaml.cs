using AdaaMobile.Controls;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views.DayPass
{
    public partial class RowInfoControl : GestureContentView
    {
        public RowInfoControl()
        {
            InitializeComponent();
			HandleArabicLanguageFlowDirection ();
        }

		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				lblTitle.HorizontalOptions = LayoutOptions.End;
				lblValue.HorizontalOptions=LayoutOptions.End;

			}
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
