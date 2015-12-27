using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.Models;
using AdaaMobile.Helpers;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models.Request;
using AdaaMobile.Enums;
using AdaaMobile.Models.Response;

namespace AdaaMobile.ViewModels
{


    public class DirectoryViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        private Employee[] _allEmployees;
        #endregion

        #region Properties

        private DirectorySourceType _directorySourceType = DirectorySourceType.Directory;

        public DirectorySourceType DirectorySourceType
        {
            get { return _directorySourceType; }
            set { _directorySourceType = value; }
        }


        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        private bool _groupingEnabled;

        public bool GroupingEnabled
        {
            get { return _groupingEnabled; }
            set { SetProperty(ref _groupingEnabled, value); }
        }


        private string _busyMessage;

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        private ObservableCollection<Grouping<string, Employee>> _groupedEmployees = new ObservableCollection<Grouping<string, Employee>>();
        public ObservableCollection<Grouping<string, Employee>> GroupedEmployees
        {
            get { return _groupedEmployees; }
        }

        private ObservableCollection<Employee> _notgroupedEmployees = new ObservableCollection<Employee>();
        public ObservableCollection<Employee> NotGroupedEmployees
        {
            get { return _notgroupedEmployees; }
        }


        #endregion

        #region Initialization
        public DirectoryViewModel(IDataService dataService, IAppSettings appSettings, INavigationService navigationService)
        {
            _dataService = dataService;
            _appSettings = appSettings;
            _navigationService = navigationService;

            LoadEmployeesCommand = new AsyncExtendedCommand(LoadEmployeesAsync);

        }

        #endregion

        #region Commands

        public AsyncExtendedCommand LoadEmployeesCommand { get; set; }
        #endregion

        #region Methods
        private async Task LoadEmployeesAsync()
        {

            try
            {
                IsBusy = true;
                LoadEmployeesCommand.CanExecute = false;
                if (DirectorySourceType == DirectorySourceType.Directory)
                {
                    var response = await _dataService.GetEmpolyeesAsync(new GetAllEmployeesQParameters()
                    {
                        Langid = _appSettings.Language,
                        UserToken = _appSettings.UserToken
                    });
                    if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                    {
                        _allEmployees = response.Result.Employees;

                        GroupEmployees();
                    }
                }
                else
                {
                    var response = await _dataService.GetDelegationSubordinatesResponseAsync(new delegationSubordinatesQParamters()
                    {
                        Langid = _appSettings.Language,
                        UserToken = _appSettings.UserToken
                    });
                    if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                    {
                        _allEmployees = response.Result.Subordinates;

                        GroupEmployees();
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                LoadEmployeesCommand.CanExecute = true;
                IsBusy = false;
            }

        }

        public void GroupEmployees()
        {

            var sorted = from emp in _allEmployees
                         orderby emp.UserName
                         group emp by emp.NameSort into empGroup
                         select new Grouping<string, Employee>(empGroup.Key, empGroup);

            var employeesGrouped = new ObservableCollection<Grouping<string, Employee>>(sorted);

            GroupingEnabled = true;
            _groupedEmployees = employeesGrouped;
            OnPropertyChanged("GroupedEmployees");
        }

        //public async void GetGroupedSubordinates()
        //{
        //    var sorted2 = from emp in allSubordinates
        //                  orderby emp.UserName
        //                  group emp by emp.NameSort into empGroup
        //                  select new Grouping<string, DelegateSubordinate>(empGroup.Key, empGroup);

        //    var subordinatesGrouped = new ObservableCollection<Grouping<string, DelegateSubordinate>>(sorted2);

        //    _groupedSubordinates = subordinatesGrouped;
        //    OnPropertyChanged("GroupedSubordinates");
        //}

        public bool Search(string filter)
        {
            if (_allEmployees == null) return false;
            if (string.IsNullOrWhiteSpace(filter))
            {
                GroupEmployees();
                return false;
            }
            else
            {
                GroupingEnabled = false;
                var list = _allEmployees.Where(p => p.UserName.ToLower().Contains(filter.ToLower()));

                _notgroupedEmployees = new ObservableCollection<Employee>();
                foreach (var item in list.ToList())
                {
                    _notgroupedEmployees.Add(item);

                }
                OnPropertyChanged("NotGroupedEmployees");
                return true;
            }
        }
        #endregion

    }

    public class Grouping<TK, T> : ObservableCollection<T>
    {
        public TK Key { get; private set; }
        public Grouping(TK key, IEnumerable<T> items)
        {
            Key = key; foreach (var item in items) Items.Add(item);
        }
    }

}
