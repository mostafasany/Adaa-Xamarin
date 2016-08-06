using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;
using AdaaMobile.ViewModels;

namespace AdaaMobile.Views.MasterView
{
	public partial class TimeSheetRecordItem : ContentView
	{
		public TimeSheetRecordItem ()
		{
			InitializeComponent ();

			HandleArabicLanguageFlowDirection ();

		}


		void HandleArabicLanguageFlowDirection ()
		{
			if (Locator.Default.AppSettings.SelectedCultureName.Contains ("ar")) {
				
				imgGrid.HorizontalOptions = LayoutOptions.Center;
				Grid.SetColumn (imgGrid, 0);
				imgArrow.RotationY = 180;	

				lblTitle.HorizontalOptions = LayoutOptions.End;
				Grid.SetColumn (lblTitle, 2);

				lblHours.HorizontalOptions = LayoutOptions.Start;
				Grid.SetColumn (lblHours, 1);

			}
		}

		#region Title p


		public static readonly BindableProperty TitleProperty = BindableProperty.Create<TimeSheetRecordItem, object> (
			                                                              p => p.Title, 
			                                                              default(object));

		public object Title {
			get { return (object)GetValue (TitleProperty); }
			set { SetValue (TitleProperty, value); }
		}

		#endregion


		#region Message p

		public static readonly BindableProperty MessageProperty = BindableProperty.Create<TimeSheetRecordItem, string> (p => p.Message, default(string));

		public string Message {
			get { return (string)GetValue (MessageProperty); }
			set { 
				SetValue (MessageProperty, value); 
			}
		}

		#endregion

		#region Date p

		public static readonly BindableProperty DateProperty = BindableProperty.Create<TimeSheetRecordItem, string> (p => p.Date, default(string));

		public string Date {
			get { return (string)GetValue (DateProperty); }
			set { 
				SetValue (DateProperty, value); 
			}
		}

		#endregion

		#region Date p

		public static readonly BindableProperty EditVisibleProperty = BindableProperty.Create<TimeSheetRecordItem, bool> (p => p.EditVisible, default(bool));

		public bool EditVisible {
			get { return (bool)GetValue (EditVisibleProperty); }
			set { 
				SetValue (EditVisibleProperty, value); 
				imgGrid.IsVisible = value;
			}
		}

		#endregion

     
		#region Command

		public static readonly BindableProperty CommandProperty = BindableProperty.Create<TimeSheetRecordItem, ICommand> (p => p.Command, default(ICommand));

		public ICommand Command {
			get { return (ICommand)GetValue (CommandProperty); }
			set { SetValue (CommandProperty, value); }
		}

		#endregion

		#region Command Parameter

		public static readonly BindableProperty CommandParamterProperty = BindableProperty.Create<TimeSheetRecordItem, object> (p => p.CommandParamter, default(object));

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
