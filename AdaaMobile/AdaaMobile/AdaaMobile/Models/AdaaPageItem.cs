using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Views;

namespace AdaaMobile.Models
{
    public class AdaaPageItem
    {
        public string Title { get; set; }

        public string IconSource { get; set; }

        public Type TargetType { get; set; }

        public static List<AdaaPageItem> Pages
        {
            get
            {
                List<AdaaPageItem> data = new List<AdaaPageItem>();

                data.Add(new AdaaPageItem()
                {
                    Title = "Home",
					IconSource = "home.png",
                    TargetType = typeof(HomePage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Profile",
					IconSource = "profile.png",
                    TargetType = typeof(ProfilePage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Attendance",
					IconSource = "attendance_icn.png",
                    TargetType = typeof(AttendancePage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Directory",
					IconSource = "directory.png",
                    TargetType = typeof(DirectoryPage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "My Requests",
						IconSource = "myrequests.png",
                    TargetType = typeof(MyRequestsPage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "My Tasks",
						IconSource = "mytasks.png",
                    TargetType = typeof(MyTasksPage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Settings",
						IconSource = "settings.png",
                    TargetType = typeof(SettingsPage)
                });
                return data;
            }

        }
    }
}