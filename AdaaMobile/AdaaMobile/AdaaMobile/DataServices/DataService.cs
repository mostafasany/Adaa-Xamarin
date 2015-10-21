using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.Models;

namespace AdaaMobile.DataServices
{
    public class DataService : IDataService
    {
        public Task<User> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
