using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views
{
	public partial class ProfilePropertyControl : ContentView
	{
		public ProfilePropertyControl ()
		{
			InitializeComponent ();
			HandleArabicLanguageFlowDirection ();
		}

		#region Title p


		void HandleArabicLanguageFlowDirection ()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				//Title
				lblTitle.HorizontalOptions = LayoutOptions.End;

			}
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create<ProfilePropertyControl, object> (p => p.Title, default(object));

		public object Title {
			get { return (object)GetValue (TitleProperty); }
			set { SetValue (TitleProperty, value); }
		}

		#endregion

		#region Value p


		public static readonly BindableProperty ValueProperty = BindableProperty.Create<ProfilePropertyControl, object> (p => p.Value, default(object));

		public object Value {
			get { return (object)GetValue (ValueProperty); }
			set { SetValue (ValueProperty, value); }
		}

		#endregion

		#region Is underline

		public static readonly BindableProperty IsUnderLineProperty = BindableProperty.Create<ProfilePropertyControl, bool> (p => p.IsUnderLine, false);

		public bool IsUnderLine {
			get { return (bool)GetValue (IsUnderLineProperty); }
			set { SetValue (IsUnderLineProperty, value); }
		}

		#endregion
	}
}
