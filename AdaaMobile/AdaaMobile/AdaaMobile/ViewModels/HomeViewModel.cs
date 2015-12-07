﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Views;
using Xamarin.Forms;
using System.IO;
using AdaaMobile.Models.Request;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.DataServices;

namespace AdaaMobile.ViewModels
{
	public class HomeViewModel : BindableBase
	{

		#region Fields

		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;
		private readonly IAppSettings _appSettings;
		#endregion

		#region Properties

		public List<AdaaPageItem> Pages { get; set; }

		private ImageSource _image;

		public ImageSource Image {
			get { return _image; }
			set { SetProperty (ref _image, value); }
		}

		private bool _isBusy;

		public bool IsBusy {
			get { return _isBusy; }
			set { SetProperty (ref _isBusy, value); }
		}

		#endregion

		#region Initialization

		public HomeViewModel (IDataService dataService, IAppSettings appSettings, INavigationService navigationService)
		{
			_dataService = dataService;
			_appSettings = appSettings;
			_navigationService = navigationService;
			Pages = GetHomePages ();
			PageClickedCommand = new ExtendedCommand<AdaaPageItem> (PageItemClicked);
			LoadCommand = new AsyncExtendedCommand (LoadAsync);
			//Default Image for User profile
				Image = new FileImageSource()
			{
				File = Device.OnPlatform("Icon-Small.png", "icon.png", "icon.png")
			};
		}

		#endregion

		private List<AdaaPageItem> GetHomePages ()
		{
			List<AdaaPageItem> data = new List<AdaaPageItem> ();

			data.Add (new AdaaPageItem () {
				Title = "Adaa Attendance",
				IconSource = "AdaaMobile.Images.Attendance_icn.svg",
				TargetType = typeof(AttendancePage)
			});

			data.Add (new AdaaPageItem () {
				Title = "Employee directory",
				IconSource = "AdaaMobile.Images.Directory.svg",
				TargetType = typeof(DirectoryPage)
			});
			
			data.Add (new AdaaPageItem () {
				Title = "E-Services",
				IconSource = "AdaaMobile.Images.E-Services.svg",
				TargetType = typeof(EServicesPage)
			});


			data.Add (new AdaaPageItem () {
				Title = "Adaa timesheet",
				IconSource = "AdaaMobile.Images.Timesheet.svg",
				TargetType = typeof(TimesheetPage)
			});

			data.Add (new AdaaPageItem () {
				Title = "My requests",
				IconSource = "AdaaMobile.Images.MyRequests.svg",
				TargetType = typeof(MyRequestsPage)
			});

			data.Add (new AdaaPageItem () {
				Title = "My tasks",
				IconSource = "AdaaMobile.Images.MyTasks.svg",
				TargetType = typeof(MyTasksPage)
			});

			data.Add (new AdaaPageItem () {
				Title = "Oracle services",
				IconSource = "AdaaMobile.Images.Oracle.svg",
				TargetType = typeof(OracleServicesPage)
			});

			data.Add (new AdaaPageItem () {
				Title = "IT services desk",
				IconSource = "AdaaMobile.Images.ITServiceDesk.svg",
				TargetType = typeof(ITServiesPage)
			});

			data.Add (new AdaaPageItem () {
				Title = "User account services",
				IconSource = "AdaaMobile.Images.Profile.svg",
				TargetType = typeof(UserServicesPage)
			});
			return data;
		}

		#region Commands

		public ExtendedCommand<AdaaPageItem> PageClickedCommand { get; set; }

		public AsyncExtendedCommand LoadCommand { get; set; }

		#endregion

		#region Methods

		private void PageItemClicked (AdaaPageItem item)
		{
			if (item == null)
				return;
			_navigationService.SetMasterDetailsPage (item.TargetType);
		}


		private async Task LoadAsync ()
		{
			try {
				LoadCommand.CanExecute = false;
				IsBusy = true;

				if (LoggedUserInfo.CurrentUserProfile != null && !String.IsNullOrWhiteSpace (LoggedUserInfo.CurrentUserProfile.UserImage64)) {
					Func<Stream> streamFunc = GetStream;
					Image = ImageSource.FromStream (streamFunc);
				}
				else
				{
					var paramters = new CurrentProfileQParameters()
					{
						Langid = _appSettings.Language,
						UserToken = _appSettings.UserToken
					};
					var result = await _dataService.GetCurrentUserProfile(paramters);

					if (result.ResponseStatus == ResponseStatus.SuccessWithResult) {
						LoggedUserInfo.CurrentUserProfile = result.Result;
						Func<Stream> streamFunc = GetStream;
						Image = ImageSource.FromStream (streamFunc);
					}
				}
					
					
			} catch (Exception ex) {
				
			} finally {
				LoadCommand.CanExecute = true;
				IsBusy = false;
			}

		}

		/// <summary>
		/// Used With Stream Image Source
		/// </summary>
		/// <returns></returns>
		private Stream GetStream()
		{
			byte[] bytes = Convert.FromBase64String(LoggedUserInfo.CurrentUserProfile.UserImage64);
			MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
			ms.Write(bytes, 0, bytes.Length);
			ms.Position = 0;//It won't work without resetting position of stream
			return ms;
		}

		#endregion

	}
}
