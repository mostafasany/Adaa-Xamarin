using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Enums;
using AdaaMobile.Models.Response;
using AdaaMobile.Views;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class UserSelectionService
    {
        private TaskCompletionSource<Employee> _completionSource;
        private readonly INavigationService _navigationService;

        public UserSelectionService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<Employee> SelectUserAsync(DirectorySourceType sourceType)
        {
            _completionSource = new TaskCompletionSource<Employee>();
            var directoyPage = new DirectoryPage(sourceType, DirectoryAccessType.Select);
            
            //Wire event of user selection
            EventHandler<Employee> selectionhandler = null;
            selectionhandler = (sender, args) =>
            {
                directoyPage.UserSelected -= selectionhandler;
                _completionSource.TrySetResult(args);
            };
            directoyPage.UserSelected += selectionhandler;

            //Navigate
            if (Application.Current.MainPage is MasterDetailPage)
            {
                await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(directoyPage);
            }
            else
            {
                return null;
            }

            return await _completionSource.Task;
        }
    }
}
