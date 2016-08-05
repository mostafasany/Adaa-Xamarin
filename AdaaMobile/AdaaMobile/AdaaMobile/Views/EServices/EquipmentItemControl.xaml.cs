using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views.MasterView
{
	public partial class EquipmentItemControl : ContentView
	{
		public EquipmentItemControl ()
		{
			InitializeComponent ();
			HandleArabicLanguageFlowDirection ();
		}

		void HandleArabicLanguageFlowDirection ()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				//Message
//				lblTitle.HorizontalOptions = LayoutOptions.End;
//				Grid.SetColumn (lblTitle, 1);
//
//				imgArrow.HorizontalOptions = LayoutOptions.Start;
//				Grid.SetColumn (imgArrow, 0);
		

			}
		}


		#region Title p


		public static readonly BindableProperty TitleProperty = BindableProperty.Create<EquipmentItemControl, object> (p => p.Title, default(object));

		public object Title {
			get { return (object)GetValue (TitleProperty); }
			set { SetValue (TitleProperty, value); }
		}

		#endregion

		#region IsSelected

		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create<EquipmentItemControl, bool> (p => p.IsSelected, default(bool));

		public bool IsSelected {
			get { return (bool)GetValue (IsSelectedProperty); }
			set { SetValue (IsSelectedProperty, value); }
		}

		#endregion

		private void Item_Tapped (object sender, EventArgs e)
		{
			IsSelected = !IsSelected;
		}
	}
}
