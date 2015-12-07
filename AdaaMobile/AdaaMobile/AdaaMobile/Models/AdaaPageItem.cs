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
						IconSource = "Icon-Small",
                    TargetType = typeof(HomePage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Profile",
                    IconSource = "icon.png",
                    TargetType = typeof(ProfilePage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Attendance",
                    IconSource = "icon.png",
                    TargetType = typeof(AttendancePage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Directory",
                    IconSource = "icon.png",
                    TargetType = typeof(DirectoryPage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "My Requests",
                    IconSource = "icon.png",
                    TargetType = typeof(MyRequestsPage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "My Tasks",
                    IconSource = "icon.png",
                    TargetType = typeof(MyTasksPage)
                });

                data.Add(new AdaaPageItem()
                {
                    Title = "Settings",
                    IconSource = "icon.png",
                    TargetType = typeof(SettingsPage)
                });
                return data;
            }

        }
    }
}