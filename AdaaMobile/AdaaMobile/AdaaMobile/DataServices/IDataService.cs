using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;

namespace AdaaMobile.DataServices
{
    public interface IDataService
    {
        Task<User> LoginAsync(string userName, string password);
    }
}
