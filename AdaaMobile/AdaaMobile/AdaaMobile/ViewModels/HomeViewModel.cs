using System;
using System.Collections.Generic;
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
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.Views.Authentication;

namespace AdaaMobile.ViewModels
{
    public class HomeViewModel : BindableBase
    {

        #region Fields

        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly IDialogManager _dialogManager;
        #endregion

        #region Properties

        public List<AdaaPageItem> Pages { get; set; }

        private ImageSource _image;

        public ImageSource Image
        {
            get { return _image; }
            set { SetProperty(ref _image, value); }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private UserProfile _userProfile;

        public UserProfile UserProfile
        {
            get { return _userProfile; }
            set { SetProperty(ref _userProfile, value); }
        }



        #endregion

        #region Initialization

        public HomeViewModel(IDataService dataService, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver, IDialogManager dialogManager)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;
            _messageResolver = messageResolver;
            _dialogManager = dialogManager;
            Pages = GetHomePages();
            PageClickedCommand = new ExtendedCommand<AdaaPageItem>(PageItemClicked);
            LoadCommand = new AsyncExtendedCommand(LoadAsync);
            //Default Image for User profile
            Image = new FileImageSource()
            {
                File = Device.OnPlatform("Profilebig.png", "Profilebig.png", "Profilebig.png")//TODO:Change to actual default file
            };
        }

        #endregion

        private List<AdaaPageItem> GetHomePages()
        {
            List<AdaaPageItem> data = new List<AdaaPageItem>();

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.AttendanceAndLeave,
                IconSource = "AdaaMobile.Images.Attendance_icn.svg",
                TargetType = typeof(AttendancePage),
                IsEnabled = true
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.EmployeeDirectory,
                IconSource = "AdaaMobile.Images.Directory.svg",
                TargetType = typeof(DirectoryPage),
                IsEnabled = true
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.EServices,
                IconSource = "AdaaMobile.Images.E-Services.svg",
                TargetType = typeof(Views.EServices.EServicesPage),
                IsEnabled = true
            });


            data.Add(new AdaaPageItem()
            {
                Title = AppResources.AdaaTimesheet,
                IconSource = "AdaaMobile.Images.Timesheet.svg",
                TargetType = typeof(TimesheetPage),
                IsEnabled = true
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.MyRequests,
                IconSource = "AdaaMobile.Images.MyRequests.svg",
                TargetType = typeof(MyRequestsPage),
                IsEnabled = true
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.CorrespondenceTrackingSystem,
                IconSource = "AdaaMobile.Images.cts.svg",
                TargetType = typeof(MyTasksPage),
                IsEnabled = false
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.OracleServices,
                IconSource = "AdaaMobile.Images.Oracle.svg",
                TargetType = typeof(OracleServicesPage),
                IsEnabled = true
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.ITServicesDesk,
                IconSource = "AdaaMobile.Images.ITServiceDesk.svg",
                TargetType = typeof(ServiceDeskHomePage),
                IsEnabled = true
            });

            data.Add(new AdaaPageItem()
            {
                Title = AppResources.UserAccountServices,
                IconSource = "AdaaMobile.Images.account_services_icn.svg",
                TargetType = typeof(UserServicesPage),
                IsEnabled = true
            });
            return data;
        }

        #region Commands

        public ExtendedCommand<AdaaPageItem> PageClickedCommand { get; set; }

        public AsyncExtendedCommand LoadCommand { get; set; }

        #endregion

        #region Methods

        private void PageItemClicked(AdaaPageItem item)
        {
            if (item == null || item.IsEnabled == false)
                return;
            try
            {

                PageClickedCommand.CanExecute = false;
                if (item.TargetType == typeof(OracleServicesPage))
                {
                    //Open the app
                    DependencyService.Get<IPhoneService>().OpenOracleApp();

                    return;
                }
                // _navigationService.SetMasterDetailsPage(item.TargetType);
                _navigationService.NavigateToPage(item.TargetType);
            }
            finally
            {

                PageClickedCommand.CanExecute = true;
            }
        }


        private async Task LoadAsync()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;
                //if (LoggedUserInfo.CurrentUserProfile != null && !String.IsNullOrWhiteSpace(LoggedUserInfo.CurrentUserProfile.UserImage64))
                //{
                //    Func<Stream> streamFunc = GetStream;
                //    Image = ImageSource.FromStream(streamFunc);
                //}
                //else
                var paramters = new CurrentProfileQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                };
                var result = await _dataService.GetCurrentUserProfile(paramters);

                if (result.ResponseStatus == ResponseStatus.SuccessWithResult && result.Result != null)
                {
                    LoggedUserInfo.CurrentUserProfile = result.Result;
                    UserProfile = result.Result;
                    if (!string.IsNullOrEmpty(result.Result.UserImage64))
                    {
                        Func<Stream> streamFunc = GetStream;
                        Image = ImageSource.FromStream(streamFunc);
                    }
                }
                else
                {
                    string message = _messageResolver.GetMessage(result);
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                    if (result.ResponseStatus == ResponseStatus.InvalidToken)
                    {
                        _navigationService.SetAppCurrentPage(typeof(LoginPage));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
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
