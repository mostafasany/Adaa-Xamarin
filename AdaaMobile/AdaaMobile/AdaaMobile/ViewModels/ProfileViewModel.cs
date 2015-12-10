using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;
using AdaaMobile.Views;
using Xamarin.Forms;

namespace AdaaMobile.ViewModels
{
    public class ProfileViewModel : BindableBase
    {
        #region Fields
        private string _otherUserId;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IDialogManager _dialogManager;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly INavigationService _navigationService;
        #endregion

        #region Properties

        private UserProfile _userProfile;
        public UserProfile UserProfile
        {
            get { return _userProfile; }
            set { SetProperty(ref _userProfile, value); }
        }

        private UserProfileMode _userProfileMode = UserProfileMode.CurrentUser;//Default is CurrentUser
        /// <summary>
        /// This enum indicates whether this is current profile of user, or other profile.
        /// </summary>
        public UserProfileMode UserProfileMode
        {
            get { return _userProfileMode; }
            set { SetProperty(ref _userProfileMode, value); }
        }

        private string _displayName;
        /// <summary>
        /// It's seperated in property, Becuase in case of other profile, 
        /// Name will be known before loading user profile.
        /// </summary>
        public string DisplayName
        {
            get { return _displayName; }
            set { SetProperty(ref _displayName, value); }
        }


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

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        #endregion

        #region Initialization
        public ProfileViewModel(IDataService dataService, IAppSettings appSettings, IDialogManager dialogManager, IRequestMessageResolver messageResolver, INavigationService navigationService)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _dialogManager = dialogManager;
            _messageResolver = messageResolver;
            _navigationService = navigationService;
            //Default Image for User profile
            Image = new FileImageSource()
            {
				File = Device.OnPlatform("Icon-Small.png", "icon.png", "icon")//TODO:Change to actual default file
            };

            LoadCommand = new AsyncExtendedCommand(LoadAsync);
            UnlockMyAccountCommand = new AsyncExtendedCommand(UnlockMyAccountAsync);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand LoadCommand { get; set; }
        public AsyncExtendedCommand UnlockMyAccountCommand { get; set; }
        #endregion

        #region Methods

        public void SetOtherUserId(string userId, string displayName = "")
        {
            UserProfileMode = UserProfileMode.OtherUser;
            UserProfile = null;
            _otherUserId = userId;
            DisplayName = displayName;
        }

        private async Task LoadAsync()
        {
            try
            {
                LoadCommand.CanExecute = false;
                IsBusy = true;
                ErrorMessage = null;
                ResponseWrapper<UserProfile> resposne = await LoadProfileAsync();

                if (resposne.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    UserProfile = resposne.Result;
                    if (UserProfile != null)
                    {
                        DisplayName = UserProfile.DisplayName;
                        if (!String.IsNullOrWhiteSpace(UserProfile.UserImage64))
                        {
                            Func<Stream> streamFunc = GetStream;
                            Image = ImageSource.FromStream(streamFunc);
                        }
                    }
                    
                }
                else
                {
                    string message = _messageResolver.GetMessage(resposne);
                    await _dialogManager.DisplayAlert(AppResources.Alert, message, AppResources.Ok);

                    if (resposne.ResponseStatus == ResponseStatus.InvalidToken)
                    {
                        _navigationService.SetAppCurrentPage(typeof(LoginPage));
                    }
                }
            }
            catch (Exception)
            {
                ErrorMessage = AppResources.LoadingError;
            }
            finally
            {
                LoadCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        private async Task<ResponseWrapper<UserProfile>> LoadProfileAsync()
        {
            if (string.IsNullOrWhiteSpace(_otherUserId))
            {//Load current profile
                var parameters = new CurrentProfileQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                };

                return await _dataService.GetCurrentUserProfile(parameters);
            }
            else
            {
                //Load other profile
                var parameters = new OtherProfileQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,
                    EmpId = _otherUserId
                };

                return await _dataService.GetOtherUserProfile(parameters);
            }
        }

        /// <summary>
        /// Used With Stream Image Source
        /// </summary>
        /// <returns></returns>
        private Stream GetStream()
        {
            byte[] bytes = Convert.FromBase64String(UserProfile.UserImage64);
            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;//It won't work without resetting position of stream
            return ms;
        }

        private async Task UnlockMyAccountAsync()
        {
            try
            {
                UnlockMyAccountCommand.CanExecute = false;
                IsBusy = true;
                var parameters = new UnlockAccountQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                };
                var response = await _dataService.UnlockAccountAsync(parameters);
                if (response.Result != null && !String.IsNullOrWhiteSpace(response.Result.Message))
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, response.Result.Message, AppResources.Ok);
                }
                else
                {
                    await _dialogManager.DisplayAlert(AppResources.Error, _messageResolver.GetMessage(response), AppResources.Ok);
                    if (response.ResponseStatus == ResponseStatus.InvalidToken)
                    {
                        _navigationService.SetAppCurrentPage(typeof(LoginPage));
                    }
                }
            }
            catch (Exception)
            {
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.Error, AppResources.UnlockAccountError, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                UnlockMyAccountCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        #endregion

    }

    public enum UserProfileMode
    {
        CurrentUser,
        OtherUser
    }
}
