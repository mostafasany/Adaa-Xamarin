using AdaaMobile.Controls;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views.Delegation
{
    public partial class NewDelegateInfoControl : GestureContentView
    {
        public NewDelegateInfoControl()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");
			HandleArabicLanguageFlowDirection ();
        }

		void HandleArabicLanguageFlowDirection()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
			{
				lblTitle.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblTitle, 1);
				lblValue.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblValue, 1);

				imgValue.HorizontalOptions = LayoutOptions.Start;
				Grid.SetColumn (imgValue, 0);
				imgValue.RotationY = 180;
			}
		}

        #region Title property


        public static readonly BindableProperty TitleProperty = BindableProperty.Create<NewDelegateInfoControl, object>(p => p.Title, default(object));

        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region Value property


        public static readonly BindableProperty ValueProperty = BindableProperty.Create<NewDelegateInfoControl, object>(p => p.Value, default(object));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion


        #region Tap Command
        #endregion
    }
}
