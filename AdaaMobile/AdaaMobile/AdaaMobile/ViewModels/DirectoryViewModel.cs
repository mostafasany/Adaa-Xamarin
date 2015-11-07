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

namespace AdaaMobile.ViewModels
{
    public class DirectoryViewModel : BindableBase
    {

        #region Fields
        private readonly IDataService _dataService;
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

        private readonly ObservableCollection<KeyedCollection<Employee>> _groupedEmployees = new ObservableCollection<KeyedCollection<Employee>>();
        public ObservableCollection<KeyedCollection<Employee>> GroupedEmployees
        {
            get { return _groupedEmployees; }
        }

        #endregion

        #region Initialization
        public DirectoryViewModel(IDataService dataService)
        {
            _dataService = dataService;

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
                //var response = _dataService.LoginAsync()
#if DEBUG
                Device.BeginInvokeOnMainThread(() =>
                {
                    // using of add function in this manner works.

                    GroupedEmployees.Add(new KeyedCollection<Employee>("a")
                {
                    new Employee() {Name = "A Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                    new Employee() {Name = "A2 Employee"},
                });
                    GroupedEmployees.Add(new KeyedCollection<Employee>("b")
                {
                    new Employee() {Name="B Employee"},
                    new Employee() {Name="B2 Employee"},
                }
                    );
                });
#endif
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
        #endregion

    }
}
