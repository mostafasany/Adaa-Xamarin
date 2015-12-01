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

namespace AdaaMobile.DataServices
{
    public interface IDataService
    {
        //Accounts module
        Task<ResponseWrapper<UserProfile>> GetCurrentUserProfile(CurrentUserProfileQParameters paramters, CancellationToken? token = null);


        Task<ResponseWrapper<LoginResponse>> LoginAsync(string userName, string password);
		Task<ResponseWrapper<GetAllEmployeesResponse>> GetEmpolyeesAsync(GetAllEmployeesQParameters parameters,  CancellationToken? token);

        //Attendance and exception
        Task<ResponseWrapper<Attendance>> GetAttendanceRecordAsync(AttendanceQParameters parameters, CancellationToken? token);
        Task<ResponseWrapper<AttendanceException>> GetAttendanceExceptionAsync(AttExceptionQParamters parameters, CancellationToken? token);

        //New day pass and pending day passes
        Task<ResponseWrapper<NewDayPassResponse>> NewDayPassAsync(DaypassRequestQParameters qParameters, DaypassRequestBParameters bParamters, CancellationToken? token);
        Task<ResponseWrapper<DayPassesResponse>> GetPendingDayPassesAsync(DayPassesQParameters parameters, CancellationToken? token);

        //Day pass aprove and list of pending day passes to approve
        Task<ResponseWrapper<DayPassApproveResponse>> DayPassApproveAsync(DaypassApproveQParameters parameters, CancellationToken? token);
        Task<ResponseWrapper<DayPassTasksResponse>> DayPassTasksResponseAsync(DayPassTasksQParameters parameters, CancellationToken? token);

    }
}
