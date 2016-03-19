using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public partial class EquipmentItemControl : ContentView
    {
        public EquipmentItemControl()
        {
            InitializeComponent();
        }

        #region Title p


        public static readonly BindableProperty TitleProperty = BindableProperty.Create<EquipmentItemControl, object>(p => p.Title, default(object));

        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region IsSelected
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create<EquipmentItemControl, bool>(p => p.IsSelected, default(bool));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        #endregion

        private void Item_Tapped(object sender, EventArgs e)
        {
            IsSelected = !IsSelected;
        }
    }
}
