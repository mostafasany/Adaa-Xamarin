using System;
using AdaaMobile.Common;
using AdaaMobile.Helpers;
using AdaaMobile.DataServices;
using AdaaMobile.Strings;
using System.Threading.Tasks;
using AdaaMobile.Models.Request;
using AdaaMobile.DataServices.Requests;
using System.Collections.Generic;
using Xamarin.Forms;
using System.IO;

namespace AdaaMobile
{
	public class GreetingCardsViewModel: BindableBase
	{
		#region Fields
		private readonly INavigationService _navigationService;
		private readonly IDataService _dataService;
		private readonly IAppSettings _appSettings;
		private readonly IDialogManager _dialogManager;
		private readonly IRequestMessageResolver _messageResolver;

		#endregion

		#region Properties
		private bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set { SetProperty(ref _isBusy, value); }
		}

		private string _busyMessage;
		public string BusyMessage
		{
			get { return _busyMessage; }
			set { SetProperty(ref _busyMessage, value); }
		}


		private List<GreetingCard> _CardsList = new List<GreetingCard>();
		public List<GreetingCard> CardsList
		{
			get { return _CardsList; }
			set { SetProperty(ref _CardsList, value); }
		}

		private ImageSource _image;
		public ImageSource Image
		{
			get { return _image; }
			set { SetProperty(ref _image, value); }
		}

		private string _SelectedCardName;
		public string SelectedCardName
		{
			get { return _SelectedCardName; }
			set { SetProperty(ref _SelectedCardName, value); }
		}

		#endregion

		#region Initialization
		public GreetingCardsViewModel(IDataService dataService, IDialogManager dialogManager, IAppSettings appSettings, INavigationService navigationService, IRequestMessageResolver messageResolver)
		{
			_dataService = dataService;
			_appSettings = appSettings;
			_navigationService = navigationService;
			_dialogManager = dialogManager;
			_messageResolver = messageResolver;
			LoadCommand = new AsyncExtendedCommand(LoadAsync);
		}

		public async Task GetCards()
		{
			try
			{
				LoadCommand.CanExecute = false;
				IsBusy = true;

				var paramters = new GetCardsQParameters()
				{
					Langid = _appSettings.Language,
					UserToken = _appSettings.UserToken
				};
				var result = await _dataService.GetCardsAsync(paramters);

				if (result.ResponseStatus == ResponseStatus.SuccessWithResult && result.Result != null)
				{
					CardsList = new List<GreetingCard>();
					CardsList.Add(new GreetingCard (){
						Title = result.Result.title,
						Id = result.Result.id,
						Image = result.Result.img
					});
					SelectedCardName = result.Result.title;
					if (!String.IsNullOrWhiteSpace(result.Result.img))
					{
						Func<Stream> streamFunc = GetStream;
						Image = ImageSource.FromStream(streamFunc);
					}
				}
				else
				{
					string message = _messageResolver.GetMessage(result);
					await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
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

		#endregion

		#region Commands
		public AsyncExtendedCommand LoadCommand { get; set; }

		#endregion

		#region Methods

		private async Task LoadAsync()
		{
			try
			{
				LoadCommand.CanExecute = false;
				IsBusy = true;
				await GetCards();
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
			if (CardsList != null && CardsList.Count > 0 && !string.IsNullOrEmpty (CardsList [0].Image)) {
				byte[] bytes = Convert.FromBase64String (CardsList [0].Image);
				MemoryStream ms = new MemoryStream (bytes, 0, bytes.Length);
				ms.Write (bytes, 0, bytes.Length);
				ms.Position = 0;//It won't work without resetting position of stream
				return ms;
			}
			return null;
		}

		#endregion
	}
}
