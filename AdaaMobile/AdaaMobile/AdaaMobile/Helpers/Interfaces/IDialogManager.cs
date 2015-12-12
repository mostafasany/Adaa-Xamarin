using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaaMobile.Helpers
{
    public interface  IDialogManager
    {
        Task DisplayAlert(string title, string message, string cancel);
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);
        Task DisplayAlert(string alert, object message, string ok);
    }
}
