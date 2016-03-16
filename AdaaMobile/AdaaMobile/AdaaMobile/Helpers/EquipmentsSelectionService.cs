using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models.Response;
using AdaaMobile.Views.EServices;
using Xamarin.Forms;

namespace AdaaMobile.Helpers
{
    public class EquipmentsSelectionService
    {
        private TaskCompletionSource<List<Equipment>> _completionSource;
        public async Task<List<Equipment>> SelectEquipmentsAsync(Equipment[] allEquipments)
        {
            var page = new EquipmentsPage(allEquipments);
            _completionSource = new TaskCompletionSource<List<Equipment>>();

            //Wire event of Equipment selection
            EventHandler<List<Equipment>> selectionhandler = null;
            selectionhandler = (sender, args) =>
            {
                page.EquipmentsSelected -= selectionhandler;
                _completionSource.TrySetResult(args);
            };
            page.EquipmentsSelected += selectionhandler;

            
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
    }
}
