using AdaaMobile.Models;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public class MenuListView : ListView
    {
        public MenuListView()
        {

            ItemsSource = AdaaPageItem.Pages;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            //SeparatorVisibility = SeparatorVisibility.None;
			BackgroundColor = (Color)App.Current.Resources["AppBackgroundDark"];
            var cell = new DataTemplate(typeof(MenuCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
			RowHeight = 90;
            ItemTemplate = cell;
			SeparatorColor = Color.Transparent;
				//(Color)App.Current.Resources ["YellowAccent"];
        }
    }
}