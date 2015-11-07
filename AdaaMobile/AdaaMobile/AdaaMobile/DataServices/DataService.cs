using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models;

namespace AdaaMobile.DataServices
{
    public class DataService : IDataService
    {
        public Task<User> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWrapper<List<Employee>>> GetEmpolyees(CancellationToken? token)
        {
            throw  new NotImplementedException();
        }
    }
}
