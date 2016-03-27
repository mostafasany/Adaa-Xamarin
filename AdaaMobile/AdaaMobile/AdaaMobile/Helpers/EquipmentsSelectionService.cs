using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Constants;
using AdaaMobile.Models.Response;
using AdaaMobile.Views.EServices;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class EquipmentsSelectionService
    {
        private TaskCompletionSource<List<Equipment>> _completionSource;
        public async Task<List<Equipment>> SelectEquipmentsAsync(Equipment[] allEquipments, List<Equipment> previousSelction)
        {
            var page = new EquipmentsPage(allEquipments, previousSelction);
            _completionSource = new TaskCompletionSource<List<Equipment>>();

            MessagingCenter.Subscribe<Object, List<Equipment>>(this, MessagingConstants.SelectedEquipments, OnEquipmentsSelected);

            //Navigate
            if (Application.Current.MainPage is MasterDetailPage)
            {
                await (Application.Current.MainPage as MasterDetailPage).Detail.Navigation.PushAsync(page);
            }
            else
            {
                return null;
            }
            return await _completionSource.Task;
        }

        private void OnEquipmentsSelected(Object sender, List<Equipment> args)
        {
            MessagingCenter.Unsubscribe<Page>(this, MessagingConstants.SelectedEquipments);

            _completionSource.TrySetResult(args);
        }
    }
}
