using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.DataServices;
using AdaaMobile.Models;
using Xamarin.Forms;
using AdaaMobile.Helpers;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models.Request;

namespace AdaaMobile.ViewModels
{
    public enum DirectoryType { Directory, Subordinats };

    public class DirectoryViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
        #endregion

        #region Properties

        private DirectoryType _DirectoryType = DirectoryType.Directory;

        public DirectoryType DirectoryType
        {
            get { return _DirectoryType; }
            set { _DirectoryType = value; }
        }


        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

		private bool _GroupingEnabled;

		public bool GroupingEnabled
		{
			get { return _GroupingEnabled; }
			set { SetProperty(ref _GroupingEnabled, value); }
		}


        private string _busyMessage;

        public string BusyMessage
        {
            get { return _busyMessage; }
            set { SetProperty(ref _busyMessage, value); }
        }

        //private readonly ObservableCollection<KeyedCollection<Employee>> _groupedEmployees = new ObservableCollection<KeyedCollection<Employee>>();
        //public ObservableCollection<KeyedCollection<Employee>> GroupedEmployees
        //{
        //    get { return _groupedEmployees; }
        //}
        private ObservableCollection<Grouping<string, Employee>> _groupedEmployees = new ObservableCollection<Grouping<string, Employee>>();
        public ObservableCollection<Grouping<string, Employee>> GroupedEmployees
        {
            get { return _groupedEmployees; }
        }


        private ObservableCollection<Grouping<string, DelegateSubordinate>> _groupedSubordinates = new ObservableCollection<Grouping<string, DelegateSubordinate>>();
        public ObservableCollection<Grouping<string, DelegateSubordinate>> GroupedSubordinates
        {
            get { return _groupedSubordinates; }
        }

		private ObservableCollection<Employee> _notgroupedEmployees = new ObservableCollection<Employee> ();
		public ObservableCollection<Employee> NotGroupedEmployees
		{
			get { return _notgroupedEmployees; }
		}


        private Employee[] allEmployees;
        private DelegateSubordinate[] allSubordinates;
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
                if (DirectoryType == DirectoryType.Directory)
                {
                var response = await _dataService.GetEmpolyeesAsync(new Models.Request.GetAllEmployeesQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                });
                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    allEmployees = response.Result.Employees;

                     GetGroupedEmployees();
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
                        allSubordinates = response.Result.Subordinates;

                        GetGroupedSubordinates();
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

		public async Task GetGroupedEmployees()
        {
            //var sorted = from emp in allEmployees
            //             orderby emp.Name
            //             group emp by emp.NameSort into empGroup
            //             select new KeyedCollection<Employee>(empGroup.Key);


            var sorted2 = from emp in allEmployees
                          orderby emp.Name
                          group emp by emp.NameSort into empGroup
                          select new Grouping<string, Employee>(empGroup.Key, empGroup);

            var EmployeesGrouped = new ObservableCollection<Grouping<string, Employee>>(sorted2);

			GroupingEnabled = true;
            _groupedEmployees = EmployeesGrouped;
            OnPropertyChanged("GroupedEmployees");
        }

        public async void GetGroupedSubordinates()
        {
            //var sorted = from emp in allEmployees
            //             orderby emp.Name
            //             group emp by emp.NameSort into empGroup
            //             select new KeyedCollection<Employee>(empGroup.Key);


            var sorted2 = from emp in allSubordinates
                          orderby emp.UserName
                          group emp by emp.NameSort into empGroup
                          select new Grouping<string, DelegateSubordinate>(empGroup.Key, empGroup);

            var subordinatesGrouped = new ObservableCollection<Grouping<string, DelegateSubordinate>>(sorted2);

            _groupedSubordinates = subordinatesGrouped;
            OnPropertyChanged("GroupedSubordinates");
        }

		public async Task<bool> Search(string filter)
        {

            var list =
				allEmployees.Where(p => p.UserName.ToLower().Contains(filter.ToLower()));
			if (list == null || list.ToList() == null) {
				GetGroupedEmployees ();
				return false;
			} else {
				GroupingEnabled = false;
				_notgroupedEmployees = new ObservableCollection<Employee> ();
				foreach (var item in list.ToList()) {
					_notgroupedEmployees.Add (item);

				}
				OnPropertyChanged("NotGroupedEmployees");
				return true;
			}
        }
        #endregion

    }

    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key; foreach (var item in items) this.Items.Add(item);
        }
    }

}
