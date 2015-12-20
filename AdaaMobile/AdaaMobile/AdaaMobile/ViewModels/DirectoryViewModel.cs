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

namespace AdaaMobile.ViewModels
{
    public class DirectoryViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private readonly IAppSettings _appSettings;
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

        private Employee[] allEmployees;
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

                var response = await _dataService.GetEmpolyeesAsync(new Models.Request.GetAllEmployeesQParameters()
                {
                    Langid = _appSettings.Language,
                    UserToken = _appSettings.UserToken
                });

                if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                {
                    allEmployees = response.Result.Employees;

                    await GetGroupedEmployees();
                }
                //#if DEBUG
                //                    Device.BeginInvokeOnMainThread(() =>
                //                {
                //                    // using of add function in this manner works.

                //                    GroupedEmployees.Add(new KeyedCollection<Employee>("a")
                //                {
                //                    new Employee() {Name = "A Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                    new Employee() {Name = "A2 Employee"},
                //                });
                //                    GroupedEmployees.Add(new KeyedCollection<Employee>("b")
                //                {
                //                    new Employee() {Name="B Employee"},
                //                    new Employee() {Name="B2 Employee"},
                //                }
                //                    );
                //                });
                //#endif
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

        public async void GetGroupedEmployees()
        {
            var sorted = from emp in allEmployees
                         orderby emp.Name
                         group emp by emp.NameSort into empGroup
                         select new KeyedCollection<Employee>(empGroup.Key);


            var sorted2 = from emp in allEmployees
                          orderby emp.Name
                          group emp by emp.NameSort into empGroup
                          select new Grouping<string, Employee>(empGroup.Key, empGroup);

            var EmployeesGrouped = new ObservableCollection<Grouping<string, Employee>>(sorted2);

            _groupedEmployees = EmployeesGrouped;
            OnPropertyChanged("GroupedEmployees");
        }

        public void Search(string filter)
        {
            var list =
                    GroupedEmployees.Where(o => o.Any(p => p.Name.ToLower().Contains(filter.ToLower())));
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
