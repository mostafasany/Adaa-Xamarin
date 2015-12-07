using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Common;
using AdaaMobile.Helpers;
using AdaaMobile.Models;
using AdaaMobile.Views;

namespace AdaaMobile.ViewModels
{
    public class HomeViewModel : BindableBase
    {

        #region Fields
        private readonly INavigationService _navigationService;
        #endregion

        #region Properties
        public List<AdaaPageItem> Pages { get; set; }
        #endregion

        #region Initialization
        public HomeViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
			Pages = GetHomePages ();
            PageClickedCommand = new ExtendedCommand<AdaaPageItem>(PageItemClicked);
        }
        #endregion

		private List<AdaaPageItem> GetHomePages ()
		{
			List<AdaaPageItem> data = new List<AdaaPageItem>();

			data.Add(new AdaaPageItem()
				{
					Title = "Adaa Attendance",
					IconSource = "AdaaMobile.Images.Attendance_icn.svg",
					TargetType = typeof(AttendancePage)
				});

			data.Add(new AdaaPageItem()
				{
					Title = "Employee directory",
					IconSource = "AdaaMobile.Images.Directory.svg",
					TargetType = typeof(DirectoryPage)
				});
			
			data.Add(new AdaaPageItem()
				{
					Title = "E-Services",
					IconSource = "AdaaMobile.Images.E-Services.svg",
					TargetType = typeof(EServicesPage)
				});


			data.Add(new AdaaPageItem()
				{
					Title = "Adaa timesheet",
					IconSource = "AdaaMobile.Images.Timesheet.svg",
					TargetType = typeof(TimesheetPage)
				});

			data.Add(new AdaaPageItem()
				{
					Title = "My requests",
					IconSource = "AdaaMobile.Images.MyRequests.svg",
					TargetType = typeof(MyRequestsPage)
				});

			data.Add(new AdaaPageItem()
				{
					Title = "My tasks",
					IconSource = "AdaaMobile.Images.MyTasks.svg",
					TargetType = typeof(MyTasksPage)
				});

			data.Add(new AdaaPageItem()
				{
					Title = "Oracle services",
					IconSource = "AdaaMobile.Images.Oracle.svg",
					TargetType = typeof(OracleServicesPage)
				});

			data.Add(new AdaaPageItem()
				{
					Title = "IT services desk",
					IconSource = "AdaaMobile.Images.ITServiceDesk.svg",
					TargetType = typeof(ITServiesPage)
				});

			data.Add(new AdaaPageItem()
				{
					Title = "User account services",
					IconSource = "AdaaMobile.Images.Profile.svg",
					TargetType = typeof(UserServicesPage)
				});
			return data;
		}

        #region Commands

        public ExtendedCommand<AdaaPageItem> PageClickedCommand { get; set; }
        #endregion

        #region Methods

        private void PageItemClicked(AdaaPageItem item)
        {
            if (item == null) return;
            _navigationService.SetMasterDetailsPage(item.TargetType);
        }

        #endregion

    }
}
