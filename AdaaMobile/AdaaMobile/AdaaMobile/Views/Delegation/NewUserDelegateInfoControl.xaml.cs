using Xamarin.Forms;

namespace AdaaMobile.Views.Delegation
{
    public partial class NewUserDelegateInfoControl : ContentView
    {
        public NewUserDelegateInfoControl()
        {
            InitializeComponent();
        }

        #region Title property


        public static readonly BindableProperty TitleProperty = BindableProperty.Create<ProfilePropertyControl, object>(p => p.Title, default(object));

        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region Value property


        public static readonly BindableProperty ValueProperty = BindableProperty.Create<ProfilePropertyControl, object>(p => p.Value, default(object));

        public object Value
        {
            get { return (object)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion
    }
}
