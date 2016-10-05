using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using AdaaMobile.Models.Response.ServiceDesk;

namespace AdaaMobile.DataServices
{
    public interface IDataService
    {
        //Accounts module
        Task<ResponseWrapper<LoginResponse>> LoginAsync(string userName, string password);
        Task<ResponseWrapper<UserProfile>> GetCurrentUserProfile(CurrentProfileQParameters paramters, CancellationToken? token = null);
        Task<ResponseWrapper<UserProfile>> GetOtherUserProfile(OtherProfileQParameters paramters, CancellationToken? token = null);
        Task<ResponseWrapper<UnlockAccountResponse>> UnlockAccountAsync(UnlockAccountQParameters paramters, CancellationToken? token = null);


        Task<ResponseWrapper<ChangePasswordResponse>> ChangePasswordAsync(string password, ChangePasswordQParameters paramters, CancellationToken? token = null);

        Task<ResponseWrapper<PasswordStatusResponse>> GetPasswordStatusAsync(PasswordStatusQParameters paramters, CancellationToken? token = null);
        Task<ResponseWrapper<AccountStatusResponse>> GetAccountStatusAsync(AccountStatusQParameters paramters, CancellationToken? token = null);


        //Directory
        Task<ResponseWrapper<GetAllEmployeesResponse>> GetEmpolyeesAsync(GetAllEmployeesQParameters parameters, CancellationToken? token = null);

        //Attendance and exception
        Task<ResponseWrapper<Attendance>> GetAttendanceRecordAsync(AttendanceQParameters parameters, CancellationToken? token = null);
        Task<ResponseWrapper<GetExceptionsRepsonse>> GetAttendanceExceptionsAsync(ExceptionsQParameter parameters, CancellationToken? token = null);

        //New day pass and pending day passes
        Task<ResponseWrapper<NewDayPassResponse>> NewDayPassAsync(DaypassRequestQParameters qParameters, DaypassRequestBParameters bParamters, CancellationToken? token = null);
        Task<ResponseWrapper<DayPassesResponse>> GetPendingDayPassesAsync(DayPassesQParameters parameters, CancellationToken? token = null);

        //Day pass aprove and list of pending day passes to approve
        Task<ResponseWrapper<DayPassApproveResponse>> DayPassApproveAsync(DaypassApproveQParameters parameters, DaypassApproveBParameters bParamters, CancellationToken? token = null);
        Task<ResponseWrapper<DayPassTasksResponse>> GetDayPassTasksResponseAsync(DayPassTasksQParameters parameters, CancellationToken? token = null);


        //Delegation
        Task<ResponseWrapper<DelegationSubordinatesResponse>> GetDelegationSubordinatesResponseAsync(delegationSubordinatesQParamters parameters, CancellationToken? token = null);

        Task<ResponseWrapper<DelegationsResponse>> GetAllDelegationsResponseAsync(DelegationsQParamters parameters, CancellationToken? token = null);

        Task<ResponseWrapper<NewDelegationResponse>> NewDelegationAsync(NewDelegationQParameter parameters, CancellationToken? token = null);

        Task<ResponseWrapper<RemoveDelegationsResponse>> RemoveDelegationAsync(RemoveDelegationQParameter parameters, CancellationToken? token = null);



        //Sprint2
        Task<ResponseWrapper<GetClientsResponse>> GetClientsAsync(GetClientsQParameters parameters, CancellationToken? token = null);

        Task<ResponseWrapper<GetCardsResponse>> GetCardsAsync(GetCardsQParameters parameters, CancellationToken? token = null);

        Task<ResponseWrapper<GetAllRequestsResponse>> GetAllSharepointRequestsAsync(GetAllRequestsQParameters parameters, CancellationToken? token = null);


        Task<ResponseWrapper<SaveOfficeMaintenanceResponse>> SaveOfficeMaintenanceAsync(SaveOfficeMaintenanceRequestQParameters qParameters, SaveOfficeMaintenanceRequestBParameters bParamters, CancellationToken? token = null);

        Task<ResponseWrapper<GetRoomsResponse>> GetRoomsAsync(GetRoomsQParameters parameters, CancellationToken? token = null);


        Task<ResponseWrapper<GetOfficeLocationsResponse>> GetOfficeLocationsAsync(GetOfficeLocationsQParameters parameters, CancellationToken? token = null);

        Task<ResponseWrapper<GetEquipmentsResponse>> GetEquipmentsAsync(GetEquipmentsQParameters parameters, CancellationToken? token = null);


        Task<ResponseWrapper<SaveDriverRequestResponse>> SaveDriverRequestAsync(SaveDriverRequestQParameters qParameters, SaveDriverRequestBParameters bParamters, CancellationToken? token = null);


        #region TimeSheet

        Task<ResponseWrapper<bool>> SubmitTimeSheet(int year, int week, List<TimeSheetRequest> bodyParamters, CancellationToken? token = null);

        Task<ResponseWrapper<List<Assignment>>> GetAssignmentAsync(CancellationToken? token = null);

        Task<ResponseWrapper<List<PendingTask>>> GetPendingTaskAsync(CancellationToken? token = null);

        Task<ResponseWrapper<List<AttendanceTask>>> GetTaskByAssignment(int assginmentID, CancellationToken? token = null);

        Task<ResponseWrapper<List<Week>>> GetWeeksPerYearAsync(int year, CancellationToken? token = null);

        Task<ResponseWrapper<TimeSheet>> GetTimeSheet(int year, int week, CancellationToken? token = null);

        #endregion


        #region ServiceDisk
        Task<ResponseWrapper<ServiceDeskRequests>> GetServiceDeskRequests(bool incidents, CancellationToken? token = null);
        Task<ResponseWrapper<AcceptAndReject>> CancelServiceDeskRequests(ServiceDeskRequest serviceRequest, CancellationToken? token = null);
        Task<ResponseWrapper<ServiceDeskCases>> GetServiceDeskCases(CancellationToken? token = null);
        Task<ResponseWrapper<AcceptAndReject>> AcceptServiceDeskCases(ServiceDeskCase serviceDesk, CancellationToken? token = null);
        Task<ResponseWrapper<AcceptAndReject>> RejectServiceDeskCases(ServiceDeskCase serviceDesk, CancellationToken? token = null);
        Task<ResponseWrapper<ServiceDeskCases>> GetServiceDeskCasesDetails(string caseId, CancellationToken? token = null);

        Task<ResponseWrapper<OnBehalfResult>> GetOnBelfUsers(CancellationToken? token = null);
        Task<ResponseWrapper<ParentCategoryResult>> GetParentCategories(string mouduleName, CancellationToken? token = null);
        Task<ResponseWrapper<ChildCategoryResult>> GetChildCategories(string mouduleName, string categoryId, CancellationToken? token = null);
        Task<ResponseWrapper<CategoryTemplateResult>> GetTemplateId(string categoryId, CancellationToken? token = null);
        Task<ResponseWrapper<TemplateExtensionResult>> GetTemplateExtension(string templateId, CancellationToken? token = null);
        Task<ResponseWrapper<AcceptAndReject>> LogIncident(LogIncidentRequest request, CancellationToken? token = null);
        Task<ResponseWrapper<string>> NewServiceRequest(NewServiceRequest request, CancellationToken? token = null);
        #endregion
    }
}
