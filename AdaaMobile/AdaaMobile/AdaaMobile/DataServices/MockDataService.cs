using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
#pragma warning disable 1998

namespace AdaaMobile.DataServices
{
	public class MockDataService : IDataService
    {
	    public Task<ResponseWrapper<UserProfile>> GetCurrentUserProfile(CurrentUserProfileQParameters paramters, CancellationToken? token = null)
	    {
	        throw new NotImplementedException();
	    }

	    public Task<ResponseWrapper<LoginResponse>> LoginAsync(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseWrapper<List<Employee>>> GetEmpolyeesAsync(CancellationToken? token)
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

		public Task<ResponseWrapper<Attendance>> GetAttendanceRecordAsync (AttendanceQParameters parameters, CancellationToken? token)
		{
			throw new NotImplementedException ();
		}

		public Task<ResponseWrapper<AttendanceException>> GetAttendanceExceptionAsync (AttExceptionQParamters parameters, CancellationToken? token)
		{
			throw new NotImplementedException ();
		}

		public Task<ResponseWrapper<NewDayPassResponse>> NewDayPassAsync (DaypassRequestQParameters qParameters, DaypassRequestBParameters bParamters, CancellationToken? token)
		{
			throw new NotImplementedException ();
		}

		public Task<ResponseWrapper<DayPassesResponse>> GetPendingDayPassesAsync (DayPassesQParameters parameters, CancellationToken? token)
		{
			throw new NotImplementedException ();
		}

		public Task<ResponseWrapper<DayPassApproveResponse>> DayPassApproveAsync (DaypassApproveQParameters parameters, CancellationToken? token)
		{
			throw new NotImplementedException ();
		}

		public Task<ResponseWrapper<DayPassTasksResponse>> DayPassTasksResponseAsync (DayPassTasksQParameters parameters, CancellationToken? token)
		{
			throw new NotImplementedException ();
		}
    }
}
