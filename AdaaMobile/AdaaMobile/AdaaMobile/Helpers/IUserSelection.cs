using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models.Response;

namespace AdaaMobile.Helpers
{
    public interface IUserSelection
    {
        event EventHandler<Employee> UserSelected;
    }
}
