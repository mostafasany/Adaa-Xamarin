using System;
using System.Reflection;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public partial class SideMenuItemControl : ContentView
    {
        public SideMenuItemControl()
        {
            InitializeComponent();
            //ListenForTap();
        }

        //private void ListenForTap()
        //{
        //    var tapListener = new TapGestureRecognizer() { NumberOfTapsRequired = 1};
        //    Tapped += (sender, args) => OnTapped();
        //    ContentGrid.GestureRecognizers.Add(tapListener);
        //}

        #region Title p


        public static readonly BindableProperty TitleProperty = BindableProperty.Create<ProfilePropertyControl, object>(p => p.Title, default(object));

        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region Svg


        public static readonly BindableProperty SvgProperty = BindableProperty.Create<SideMenuItemControl, string>(p => p.Svg, default(string));

        public string Svg
        {
            get { return (string)GetValue(SvgProperty); }
            set { SetValue(SvgProperty, value); }
        }
        #endregion

        #region IsPageSupported
        public static readonly BindableProperty IsPageSupportedProperty = BindableProperty.Create<SideMenuItemControl, bool>(p => p.IsPageSupported, default(bool));

        public bool IsPageSupported
        {
            get { return (bool)GetValue(IsPageSupportedProperty); }
            set { SetValue(IsPageSupportedProperty, value); }
        }
        #endregion

        #region IsSelected
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create<SideMenuItemControl, bool>(p => p.IsSelected, default(bool));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        #endregion

        public event EventHandler Tapped;

        protected virtual void OnTapped()
        {
            var handler = Tapped;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private void OnGridTapped(object sender, EventArgs e)
        {
            OnTapped();
        }
    }
}
