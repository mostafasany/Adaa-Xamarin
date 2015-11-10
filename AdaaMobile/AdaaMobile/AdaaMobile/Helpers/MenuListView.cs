using AdaaMobile.Models;
using AdaaMobile.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<AdaaMenuItem> data = new List<AdaaMenuItem>();

            data.Add(new AdaaMenuItem()
            {
                Title = "Home",
                IconSource = "icon.png",
                TargetType = typeof(HomePage)
            });

            data.Add(new AdaaMenuItem()
            {
                Title = "Profile",
                IconSource = "icon.png",
                TargetType = typeof(ProfilePage)
            });

            data.Add(new AdaaMenuItem()
            {
                Title = "Attendance",
                IconSource = "icon.png",
                TargetType = typeof(AttendancePage)
            });

            data.Add(new AdaaMenuItem()
            {
                Title = "Directory",
                IconSource = "icon.png",
                TargetType = typeof(DirectoryPage)
            });

            data.Add(new AdaaMenuItem()
            {
                Title = "My Requests",
                IconSource = "icon.png",
                TargetType = typeof(MyRequestsPage)
            });

            data.Add(new AdaaMenuItem()
            {
                Title = "My Tasks",
                IconSource = "icon.png",
                TargetType = typeof(MyTasksPage)
            });

            data.Add(new AdaaMenuItem()
            {
                Title = "Settings",
                IconSource = "icon.png",
                TargetType = typeof(SettingsPage)
            });

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            //SeparatorVisibility = SeparatorVisibility.None;

            var cell = new DataTemplate(typeof(MenuCell));
            cell.SetBinding(MenuCell.TextProperty, "Title");
            cell.SetBinding(MenuCell.ImageSourceProperty, "IconSource");

            ItemTemplate = cell;
        }
    }
}