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
        Task<ResponseWrapper<LoginResponse>> LoginAsync(string userName, string password);
        Task<ResponseWrapper<UserProfile>> GetCurrentUserProfile(CurrentProfileQParameters paramters, CancellationToken? token = null);
        Task<ResponseWrapper<UserProfile>> GetOtherUserProfile(OtherProfileQParameters paramters, CancellationToken? token = null);
        Task<ResponseWrapper<UnlockAccountResponse>> UnlockAccountAsync(UnlockAccountQParameters paramters, CancellationToken? token = null);

        //Directory
        Task<ResponseWrapper<GetAllEmployeesResponse>> GetEmpolyeesAsync(GetAllEmployeesQParameters parameters, CancellationToken? token = null);

        //Attendance and exception
        Task<ResponseWrapper<Attendance>> GetAttendanceRecordAsync(AttendanceQParameters parameters, CancellationToken? token = null);
        Task<ResponseWrapper<GetExceptionsRepsonse>> GetAttendanceExceptionsAsync(ExceptionsQParameter parameters, CancellationToken? token = null);
        Task<ResponseWrapper<AttendanceException>> GetAttendanceExceptionAsync(ExceptionDetailsQParamters parameters, CancellationToken? token = null);

        //New day pass and pending day passes
        Task<ResponseWrapper<NewDayPassResponse>> NewDayPassAsync(DaypassRequestQParameters qParameters, DaypassRequestBParameters bParamters, CancellationToken? token = null);
        Task<ResponseWrapper<DayPassesResponse>> GetPendingDayPassesAsync(DayPassesQParameters parameters, CancellationToken? token = null);

        //Day pass aprove and list of pending day passes to approve
        Task<ResponseWrapper<DayPassApproveResponse>> DayPassApproveAsync(DaypassApproveQParameters parameters, CancellationToken? token = null);
        Task<ResponseWrapper<DayPassTasksResponse>> DayPassTasksResponseAsync(DayPassTasksQParameters parameters, CancellationToken? token = null);

    }
}
