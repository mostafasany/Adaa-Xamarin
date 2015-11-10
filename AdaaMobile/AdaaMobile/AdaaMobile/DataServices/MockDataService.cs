using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models;
using AdaaMobile.Models.Response;

namespace AdaaMobile.DataServices
{
    public class MockDataService : IDataService
    {
        public Task<User> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseWrapper<List<Employee>>> GetEmpolyees(CancellationToken? token)
        {
            var response = new ResponseWrapper<List<Employee>>();
            response.ResponseStatus = ResponseStatus.SuccessWithResult;
            response.Result = new List<Employee>()
            {
            new Employee() {Name = "Employee 1"},
            new Employee() {Name = "Employee 2"},
            new Employee() {Name = "Manager 1"},
            new Employee() {Name = "Manager 2"},
            };
            return response;
        }

        Task<ResponseWrapper<LoginResponse>> IDataService.LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
