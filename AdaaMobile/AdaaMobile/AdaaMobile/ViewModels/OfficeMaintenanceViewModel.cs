using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Strings;

namespace AdaaMobile.ViewModels
{
    public class OfficeMaintenanceViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private readonly IRequestMessageResolver _messageResolver;
        private readonly IDialogManager _dialogManager;
        private readonly EquipmentsSelectionService _equipmentsSelectionService;

        #endregion

        #region Properties
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public bool IsLoaded { get; set; }

        private OfficeLocation[] _locations;
        public OfficeLocation[] Locations
        {
            get { return _locations; }
            set { SetProperty(ref _locations, value); }
        }

        private OfficeLocation _selectedLocation;
        public OfficeLocation SelectedLocation
        {
            get { return _selectedLocation; }
            set {
                if (SetProperty(ref _selectedLocation, value))
                {
                    if (value == null)
                    {
                        Rooms = null;
                        SelectedRoom = null;
                    }
                    else
                    {
                        LoadRoomsCommand.Execute(null);
                    }
                }
            }
        }

        public string LocationPlaceHolder
        {
            get { return SelectedLocation == null ? AppResources.Select : SelectedLocation.Title; }
        }

        private Room[] _rooms;
        public Room[] Rooms
        {
            get { return _rooms; }
            set { SetProperty(ref _rooms, value); }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set { SetProperty(ref _selectedRoom, value); }
        }

        public string RoomPlaceHolder
        {
            get { return SelectedRoom == null ? AppResources.Select : SelectedRoom.Title; }
        }

        private Equipment[] _equipments;
        public Equipment[] Equipments
        {
            get { return _equipments; }
            set { SetProperty(ref _equipments, value); }
        }

        private List<Equipment> _selectedEquipments;
        public List<Equipment> SelectedEquipments
        {
            get { return _selectedEquipments; }
            set
            {
                if (SetProperty(ref _selectedEquipments, value))
                {
                    OnPropertyChanged("EquipmentNamesLiteral");
                }
            }
        }


        public string EquipmentNamesLiteral
        {
            get
            {
                var equipments = SelectedEquipments;
                if (equipments == null || equipments.Count == 0) return AppResources.PressToSelectEquipments;
                StringBuilder builder = new StringBuilder();
                foreach (var equipment in equipments)
                {
                    if (builder.Length != 0) builder.Append(' ').Append(',');
                    builder.Append(equipment.Title);
                }
                return builder.ToString();
            }
        }


        private MaintenancePriority[] _priorities;
        public MaintenancePriority[] Priorities
        {
            get { return _priorities; }
            private set { SetProperty(ref _priorities, value); }
        }

        private MaintenancePriority _selectedPriority;
        public MaintenancePriority SelectedPriority
        {
            get { return _selectedPriority; }
            set { SetProperty(ref _selectedPriority, value); }
        }

        private string _serviceDetails;
        public string ServiceDetails
        {
            get { return _serviceDetails; }
            set { SetProperty(ref _serviceDetails, value); }
        }

        private string _additionalComments;
        public string AdditionalComments
        {
            get { return _additionalComments; }
            set { SetProperty(ref _additionalComments, value); }
        }
        #endregion

        #region Initialization
        public OfficeMaintenanceViewModel(INavigationService navigationService, IDataService dataService, IAppSettings appSettings, IRequestMessageResolver messageResolver, IDialogManager dialogManager, EquipmentsSelectionService equipmentsSelectionService)
        {
            _navigationService = navigationService;
            _dataService = dataService;
            _appSettings = appSettings;
            _messageResolver = messageResolver;
            _dialogManager = dialogManager;
            _equipmentsSelectionService = equipmentsSelectionService;

            Priorities = new MaintenancePriority[]
            {
                new MaintenancePriority() {Title = AppResources.PriorityNormal,Id = @"1"},
                new MaintenancePriority() {Title = AppResources.PriorityMedium,Id = @"2"},
                new MaintenancePriority() {Title = AppResources.PriorityUrgent,Id = @"3"},
            };

            SelectedPriority = Priorities[0];

            LoadFieldsCommand = new AsyncExtendedCommand(LoadFieldsAsync);
            LoadRoomsCommand = new AsyncExtendedCommand(LoadRoomsAsync);
            SubmitRequestCommand = new AsyncExtendedCommand(SubmitRequestAsync);
            SelectEquipmentsCommand = new AsyncExtendedCommand(SelectEquipmentsAsync);
        }

        #endregion

        #region Commands

        public AsyncExtendedCommand LoadFieldsCommand { get; set; }
        public AsyncExtendedCommand LoadRoomsCommand { get; set; }
        public AsyncExtendedCommand SubmitRequestCommand { get; set; }
        public AsyncExtendedCommand SelectEquipmentsCommand { get; set; }
        #endregion

        #region Methods

        private async Task LoadFieldsAsync(CancellationToken token)
        {
            try
            {
                LoadFieldsCommand.CanExecute = false;
                bool firstSuccess = await LoadAllEquipmentsAsync(token);
                bool secondSuccess = false;
                //dont load if the initial request failed
                if (firstSuccess) secondSuccess = await LoadLocationsAsync(token);
                if (firstSuccess && secondSuccess) IsLoaded = true;
            }
            finally
            {
                LoadFieldsCommand.CanExecute = true;
            }
        }

        private async Task<bool> LoadAllEquipmentsAsync(CancellationToken token)
        {
            try
            {
                IsBusy = true;
                SelectedEquipments = null;
                var equipmentsParamters = new GetEquipmentsQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,
                };
                var roomsResponseWrraper = await _dataService.GetEquipmentsAsync(equipmentsParamters, token);
                if (token.IsCancellationRequested) return false;
                if (roomsResponseWrraper.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    Equipments = roomsResponseWrraper.Result.Equipments;
                    return true;
                }
                else
                {
                    string message = _messageResolver.GetMessage(roomsResponseWrraper);
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                }

            }
            catch (Exception)
            {
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
            return false;
        }

        private async Task<bool> LoadLocationsAsync(CancellationToken token)
        {
            try
            {
                IsBusy = true;
                var locationsParamters = new GetOfficeLocationsQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,

                };
                var locationsResponseWrraper = await _dataService.GetOfficeLocationsAsync(locationsParamters, token);
                if (token.IsCancellationRequested) return false;
                if (locationsResponseWrraper.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    Locations = locationsResponseWrraper.Result.Locations;
                    return true;
                }
                else
                {
                    string message = _messageResolver.GetMessage(locationsResponseWrraper);
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                }
            }
            catch (Exception)
            {
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }
            return false;
        }

        private async Task SelectEquipmentsAsync()
        {
            if (Equipments == null) return;
            var selectedEquipments = await _equipmentsSelectionService.SelectEquipmentsAsync(Equipments);
            SelectedEquipments = selectedEquipments;
        }

        private async Task LoadRoomsAsync(CancellationToken token)
        {
            if (SelectedLocation == null) return;
            try
            {
                IsBusy = true;
                var roomsParamters = new GetRoomsQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken,
                    LocationId = SelectedLocation.Id
                };
                var roomsResponseWrraper = await _dataService.GetRoomsAsync(roomsParamters, token);
                if (token.IsCancellationRequested) return;
                if (roomsResponseWrraper.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    Rooms = roomsResponseWrraper.Result.Rooms;
                }
                else
                {
                    string message = _messageResolver.GetMessage(roomsResponseWrraper);
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                }
            }
            catch (Exception)
            {
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsBusy = false;
                }
            }

        }

        private async Task SubmitRequestAsync()
        {
            //Validations

            if (SelectedEquipments == null || SelectedEquipments.Count == 0)
            {
                await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SpecifyEquipments, AppResources.Ok);
                return;
            }

            if (SelectedLocation == null)
            {
                await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.ChooseOfficeLocation, AppResources.Ok);
                return;
            }

            if (SelectedRoom == null)
            {
                await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SpecifyRoom, AppResources.Ok);
                return;
            }

            if (ServiceDetails == null)
            {
                await _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.EnterServiceDetails, AppResources.Ok);
                return;
            }

            try
            {
                SubmitRequestCommand.CanExecute = false;
                IsBusy = true;

                var queryParamters = new SaveOfficeMaintenanceRequestQParameters()
                {
                    UserToken = _appSettings.UserToken,
                    Langid = _appSettings.Language
                };

                var bodyParameters = new SaveOfficeMaintenanceRequestBParameters()
                {
                    Equipments = EquipmentNamesLiteral,//TODO:Check request sample
                    Room = SelectedRoom.Id,
                    Location = SelectedLocation.Id,
                    Priority = SelectedPriority.Id,
                    Requestcomments = AdditionalComments,
                    Servicedetails = ServiceDetails
                };

                var responseWrapper = await _dataService.SaveOfficeMaintenanceAsync(queryParamters, bodyParameters);
                if (responseWrapper.ResponseStatus == ResponseStatus.SuccessWithResult)
                {
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, responseWrapper.Result.Message, AppResources.Ok);
                }
                else
                {
                    string message = _messageResolver.GetMessage(responseWrapper);
                    await _dialogManager.DisplayAlert(AppResources.ApplicationName, message, AppResources.Ok);
                }
            }
            catch (Exception)
            {
#pragma warning disable 4014
                _dialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.SomethingErrorHappened, AppResources.Ok);
#pragma warning restore 4014
            }
            finally
            {
                IsBusy = false;
                SubmitRequestCommand.CanExecute = true;
            }

        }

        #endregion

    }
}
