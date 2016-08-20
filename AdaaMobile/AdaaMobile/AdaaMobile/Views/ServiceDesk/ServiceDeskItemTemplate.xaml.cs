using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views.ServiceDesk
{
	public partial class ServiceDeskItemTemplate : ContentView
	{
		public ServiceDeskItemTemplate ()
		{
			InitializeComponent ();

			HandleArabicLanguageFlowDirection ();

		}


		void HandleArabicLanguageFlowDirection ()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				
				imgArrow.HorizontalOptions = LayoutOptions.Start;
				Grid.SetColumn (imgArrow, 1);
				imgArrow.RotationY = 180;	

				lblDate.HorizontalOptions = LayoutOptions.Start;
				Grid.SetColumn (lblDate, 1);

				lblTitle.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblTitle, 2);

				lblMessages.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblMessages, 2);

			}
		}

		#region Title p


		public static readonly BindableProperty TitleProperty = BindableProperty.Create<ServiceDeskItemTemplate, object> (
			                                                              p => p.Title, 
			                                                              default(string));

		public string Title {
			get { return (string)GetValue (TitleProperty); }
			set { SetValue (TitleProperty, value); }
		}

		#endregion


		#region Message p

		public static readonly BindableProperty MessageProperty = BindableProperty.Create<ServiceDeskItemTemplate, string> (p => p.Message, default(string));

		public string Message {
			get { return (string)GetValue (MessageProperty); }
			set { 
				SetValue (MessageProperty, value); 
			}
		}

		#endregion

		#region Date p

		public static readonly BindableProperty DateProperty = BindableProperty.Create<ServiceDeskItemTemplate, string> (p => p.Date, default(string));

		public string Date {
			get { return (string)GetValue (DateProperty); }
			set { 
				SetValue (DateProperty, value); 
			}
		}

		#endregion

     
		#region Command

		public static readonly BindableProperty CommandProperty = BindableProperty.Create<ServiceDeskItemTemplate, ICommand> (p => p.Command, default(ICommand));

		public ICommand Command {
			get { return (ICommand)GetValue (CommandProperty); }
			set { SetValue (CommandProperty, value); }
		}

		#endregion

		#region Command Parameter

		public static readonly BindableProperty CommandParamterProperty = BindableProperty.Create<ServiceDeskItemTemplate, object> (p => p.CommandParamter, default(object));

		public object CommandParamter {
			get { return (object)GetValue (CommandParamterProperty); }
			set { SetValue (CommandParamterProperty, value); }
		}

		#endregion

		/// <summary>
		/// This will listen for button tapped in the underline button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_OnClicked (object sender, EventArgs e)
		{
			OnTapped ();

			if (Command != null) {
				if (Command.CanExecute (CommandParamter)) {
					Command.Execute (CommandParamter);
				}
			}
		}


		#region Tapped Event

		public event EventHandler Tapped;

		protected virtual void OnTapped ()
		{
			var handler = Tapped;
			if (handler != null)
				handler (this, EventArgs.Empty);
		}

		#endregion
     
	}
}
