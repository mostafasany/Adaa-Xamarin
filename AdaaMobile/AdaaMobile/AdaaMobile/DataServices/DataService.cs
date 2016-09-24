using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Extensions;
using AdaaMobile.Helpers;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using QueryExtensions;
using AdaaMobile.ViewModels;
using Newtonsoft.Json;
using Xamarin.Forms;
//using System.Security;
//using System.Security.Cryptography;

namespace AdaaMobile.DataServices
{
	public class DataService : IDataService
	{
		//http://10.2.5.252/FLIXAPIADAA/api/SMGateway/GetIncidentsData_SQLMultiLang?AffecteduserName=george.zaki&lang=en&AssignedTouserName=george.zaki
		//http://10.2.5.252/FLIXAPIADAA/api/SMGateway/GetServiceRequests_SQl_multiLang?AffectedUser=george.zaki&lang=en&AssignedToUser=george.zaki
		//http://10.2.5.252/APIFLIX/api/SMGateway/Get_RA_ByReviewerName_MultiLanguage_SQL_Status?ReviewerName=george.zaki&Status=11fc3cef-15e5-bca4-dee0-9c1155ec8d83&lang=en

		// private const string BaseUrl = "http://adaa.getsandbox.com";
		private const string ServiceDiskBaseUrl = "http://196.205.5.99/FLIXAPIADAA/api/SMGateway/";

		private const string BaseUrl = "https://adaamobile.adaa.abudhabi.ae/proxyservice/proxy";
		private const string TimeSheetBaseUrl = "http://adaatime.linkdev.com/api/MobileServices/";
		private const string Server = "adaamobile";
		private const string Sprint2Server = "imach";
		private const string ContenTypeKey = "Content-Type";
		private const string XmlContentType = "application/xml";
		private const string JSONContentType = "application/json";
		private readonly Func<BaseRequest> _requestFactory;
		private readonly IAppSettings _appSettings;

		public DataService(Func<BaseRequest> requestFactory, IAppSettings appSettings)
		{
			_requestFactory = requestFactory;
			_appSettings = appSettings;
		}


		public async Task<ResponseWrapper<bool>> SubmitTimeSheet(int year, int week, List<TimeSheetRequest> bodyParamters, CancellationToken? token = null)
		{
			var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();
			request.ResultContentType = ContentType.Json;
			request.RequestUrl = TimeSheetBaseUrl + string.Format("SaveTimeSheet?encryptedUserName={0}&currentWeek={1}&year={2}", encryptedUserName, week, year);
			var result = JsonConvert.SerializeObject(bodyParamters);
			var stringContent = new StringContent(result, new UTF8Encoding(), JSONContentType);
			return await request.PostAsync<bool>(stringContent);
		}

		public async Task<ResponseWrapper<TimeSheet>> GetTimeSheet(int year, int week, CancellationToken? token = null)
		{
			var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();
			request.RequestUrl = TimeSheetBaseUrl + string.Format("GetTimeSheet?encryptedUserName={0}&weekNo={1}&year={2}", encryptedUserName, week, year);
			request.ResultContentType = ContentType.Json;
			return await request.GetAsync<TimeSheet>(token);
		}

		public async Task<ResponseWrapper<List<Week>>> GetWeeksPerYearAsync(int year, CancellationToken? token = null)
		{
			var request = _requestFactory();
			string language = "";
			if (String.IsNullOrEmpty(Locator.Default.AppSettings.SelectedCultureName) || Locator.Default.AppSettings.SelectedCultureName.StartsWith("en"))
			{
				language = "en";
			}
			else
			{
				language = "ar";
			}
			request.RequestUrl = TimeSheetBaseUrl + string.Format("getWeeksPerYear?year={0}&lang={1}", year, language);
			request.ResultContentType = ContentType.Json;
			return await request.GetAsync<List<Week>>(token);
		}
		public async Task<ResponseWrapper<ServiceDeskCases>> GetServiceDeskCases(CancellationToken? token = null)
		{
			string language = "";
			if (String.IsNullOrEmpty(Locator.Default.AppSettings.SelectedCultureName) || Locator.Default.AppSettings.SelectedCultureName.StartsWith("en"))
			{
				language = "en";
			}
			else
			{
				language = "ar";
			}
			//var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();


			request.RequestUrl = ServiceDiskBaseUrl + "Get_RA_ByReviewerName_MultiLanguage_SQL_Status?ReviewerName=OtOKaLu4Z8I0vG/D9nXUmg==&Status=00000000-0000-0000-0000-000000000000&lang=en";
			request.ResultContentType = ContentType.Json;
			var stringContent = new StringContent("", new UTF8Encoding(), JSONContentType);
			var response = await request.PostAsync<ServiceDeskCases>(stringContent, token);
			return response;
		}

		async Task<ResponseWrapper<ServiceDeskRequests>> IDataService.GetServiceDeskRequests(bool incidents, CancellationToken? token)
		{

			string language = "";
			if (String.IsNullOrEmpty(Locator.Default.AppSettings.SelectedCultureName) || Locator.Default.AppSettings.SelectedCultureName.StartsWith("en"))
			{
				language = "en";
			}
			else
			{
				language = "ar";
			}
			//var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();
			if (incidents)
			{
				request.RequestUrl = ServiceDiskBaseUrl + "GetIncidentsData_SQLMultiLang?AffecteduserName=OtOKaLu4Z8I0vG/D9nXUmg==&lang=en&AssignedTouserName=OtOKaLu4Z8I0vG/D9nXUmg ==";

			}
			else
			{
				request.RequestUrl = ServiceDiskBaseUrl + "GetServiceRequests_SQl_multiLang?AffecteduserName=OtOKaLu4Z8I0vG/D9nXUmg==&lang=en&AssignedTouserName=OtOKaLu4Z8I0vG/D9nXUmg ==";
			}
			request.ResultContentType = ContentType.Json;
			var stringContent = new StringContent("", new UTF8Encoding(), JSONContentType);
			var response = await request.PostAsync<ServiceDeskRequests>(stringContent, token);
			return response;
		}
		async Task<ResponseWrapper<ServiceDeskCases>> IDataService.GetServiceDeskCasesDetails(string caseId, CancellationToken? token)
		{
			string language = "";
			if (String.IsNullOrEmpty(Locator.Default.AppSettings.SelectedCultureName) || Locator.Default.AppSettings.SelectedCultureName.StartsWith("en"))
			{
				language = "en";
			}
			else
			{
				language = "ar";
			}
			//var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();


			request.RequestUrl = ServiceDiskBaseUrl + string.Format("GetActivityDetailsByID_Security?id={0}&UserName=george.zaki&UserDomain=dev", caseId);
			request.ResultContentType = ContentType.Json;

			var stringContent = new StringContent("", new UTF8Encoding(), JSONContentType);
			var response = await request.PostAsync<ServiceDeskCases>(stringContent, token);
			return response;
		}


		public async Task<ResponseWrapper<List<Assignment>>> GetAssignmentAsync(CancellationToken? token = null)
		{
			var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();
			request.RequestUrl = TimeSheetBaseUrl + string.Format("GetFilteredAssignments?encryptedUserName={0}", encryptedUserName);
			request.ResultContentType = ContentType.Json;
			return await request.GetAsync<List<Assignment>>(token);
		}

		public async Task<ResponseWrapper<List<PendingTask>>> GetPendingTaskAsync(CancellationToken? token = null)
		{
			var encryptedUserName = DependencyService.Get<ICryptoGraphyService>().Encrypt(LoggedUserInfo.CurrentUserProfile.DisplayName);
			var request = _requestFactory();
			request.RequestUrl = TimeSheetBaseUrl + string.Format("GetUserTasks?encryptedUserName={0}", encryptedUserName);
			request.ResultContentType = ContentType.Json;
			return await request.GetAsync<List<PendingTask>>(token);
		}

		public async Task<ResponseWrapper<List<AttendanceTask>>> GetTaskByAssignment(int assginmentID, CancellationToken? token = null)
		{
			var request = _requestFactory();
			request.RequestUrl = TimeSheetBaseUrl + string.Format("GetTasksByAssignmentID?AssignmentID={0}&lang={1}", assginmentID, Locator.Default.AppSettings.SelectedCultureName);
			request.ResultContentType = ContentType.Json;
			return await request.GetAsync<List<AttendanceTask>>(token);
		}


		#region Accounts module

		public async Task<ResponseWrapper<UserProfile>> GetCurrentUserProfile(CurrentProfileQParameters paramters, CancellationToken? token = null)
		{
			paramters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(paramters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<UserProfile>(token);
		}

		public async Task<ResponseWrapper<UserProfile>> GetOtherUserProfile(OtherProfileQParameters paramters, CancellationToken? token = null)
		{
			paramters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(paramters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<UserProfile>(token);
		}

		public async Task<ResponseWrapper<UnlockAccountResponse>> UnlockAccountAsync(UnlockAccountQParameters paramters, CancellationToken? token = null)
		{
			paramters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(paramters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<UnlockAccountResponse>(token);
		}

		//TODO:Check Http Client multiple requests issue of Pcl and If we have to switch


		public async Task<ResponseWrapper<LoginResponse>> LoginAsync(string userName, string password)
		{
			var qParamters = new LoginQParameters()
			{
				Server = Server,
				Langid = _appSettings.Language,
			};

			var request = _requestFactory();
			request.ResultContentType = ContentType.Xml;
			request.RequestUrl = BaseUrl.AppendQueryString(qParamters);

			var loginParamters = new LoginBodyParamters()
			{
				UserName = userName,
				Password = password
			};
			var stringContent = new StringContent(loginParamters.SerializeXml(), new UTF8Encoding(), XmlContentType);
			var response = await request.PostAsync<LoginResponse>(stringContent);
			return response;
		}

		public async Task<ResponseWrapper<ChangePasswordResponse>> ChangePasswordAsync(string password, ChangePasswordQParameters paramters, CancellationToken? token = default(CancellationToken?))
		{
			paramters.Server = Server;
			var request = _requestFactory();
			request.ResultContentType = ContentType.Xml;
			request.RequestUrl = BaseUrl.AppendQueryString(paramters);

			var loginParamters = new ChangePasswordBodyParamters()
			{
				Password = password
			};
			var stringContent = new StringContent(loginParamters.SerializeXml(), new UTF8Encoding(), XmlContentType);
			return await request.PostAsync<ChangePasswordResponse>(stringContent);
		}

		public async Task<ResponseWrapper<PasswordStatusResponse>> GetPasswordStatusAsync(PasswordStatusQParameters paramters, CancellationToken? token = default(CancellationToken?))
		{
			paramters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(paramters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<PasswordStatusResponse>(token);
		}

		public async Task<ResponseWrapper<AccountStatusResponse>> GetAccountStatusAsync(AccountStatusQParameters paramters, CancellationToken? token = default(CancellationToken?))
		{
			paramters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(paramters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<AccountStatusResponse>(token);
		}

		#endregion

		public async Task<ResponseWrapper<GetAllEmployeesResponse>> GetEmpolyeesAsync(GetAllEmployeesQParameters parameters, CancellationToken? token = null)
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetAllEmployeesResponse>(token);

		}

		#region Attendance

		public async Task<ResponseWrapper<Attendance>> GetAttendanceRecordAsync(AttendanceQParameters parameters, CancellationToken? token = null)
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<Attendance>(token);
		}

		public async Task<ResponseWrapper<GetExceptionsRepsonse>> GetAttendanceExceptionsAsync(ExceptionsQParameter parameters, CancellationToken? token = null)
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetExceptionsRepsonse>(token);
		}

		#endregion

		#region DayPass

		public async Task<ResponseWrapper<NewDayPassResponse>> NewDayPassAsync(DaypassRequestQParameters qParameters, DaypassRequestBParameters bodyParamters, CancellationToken? token = null)
		{
			qParameters.Server = Server;
			var request = _requestFactory();
			request.ResultContentType = ContentType.Xml;
			request.RequestUrl = BaseUrl.AppendQueryString(qParameters);

			var stringContent = new StringContent(bodyParamters.SerializeXml(), new UTF8Encoding(), XmlContentType);
			return await request.PostAsync<NewDayPassResponse>(stringContent);
		}

		public async Task<ResponseWrapper<DayPassesResponse>> GetPendingDayPassesAsync(DayPassesQParameters parameters, CancellationToken? token = null)
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<DayPassesResponse>(token);
		}

		public async Task<ResponseWrapper<DayPassTasksResponse>> GetDayPassTasksResponseAsync(DayPassTasksQParameters parameters, CancellationToken? token = null)
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<DayPassTasksResponse>(token);
		}

		public async Task<ResponseWrapper<DayPassApproveResponse>> DayPassApproveAsync(DaypassApproveQParameters qParameters, DaypassApproveBParameters bParamters, CancellationToken? token = null)
		{
			qParameters.Server = Server;
			var request = _requestFactory();
			request.ResultContentType = ContentType.Xml;
			request.RequestUrl = BaseUrl.AppendQueryString(qParameters);

			var stringContent = new StringContent(bParamters.SerializeXml(), new UTF8Encoding(), XmlContentType);
			return await request.PostAsync<DayPassApproveResponse>(stringContent);
		}


		#endregion

		#region Delegation

		public async Task<ResponseWrapper<DelegationSubordinatesResponse>> GetDelegationSubordinatesResponseAsync(delegationSubordinatesQParamters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<DelegationSubordinatesResponse>(token);
		}

		public async Task<ResponseWrapper<DelegationsResponse>> GetAllDelegationsResponseAsync(DelegationsQParamters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<DelegationsResponse>(token);
		}

		public async Task<ResponseWrapper<NewDelegationResponse>> NewDelegationAsync(NewDelegationQParameter parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<NewDelegationResponse>(token);
		}

		public async Task<ResponseWrapper<RemoveDelegationsResponse>> RemoveDelegationAsync(RemoveDelegationQParameter parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<RemoveDelegationsResponse>(token);
		}




		#endregion

		#region Sprint 2

		public async Task<ResponseWrapper<GetClientsResponse>> GetClientsAsync(GetClientsQParameters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetClientsResponse>(token);
		}

		public async Task<ResponseWrapper<GetCardsResponse>> GetCardsAsync(GetCardsQParameters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetCardsResponse>(token);
		}

		public async Task<ResponseWrapper<GetAllRequestsResponse>> GetAllSharepointRequestsAsync(GetAllRequestsQParameters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetAllRequestsResponse>(token);
		}

		public async Task<ResponseWrapper<SaveOfficeMaintenanceResponse>> SaveOfficeMaintenanceAsync(SaveOfficeMaintenanceRequestQParameters qParameters, SaveOfficeMaintenanceRequestBParameters bodyParamters, CancellationToken? token = default(CancellationToken?))
		{
			qParameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.ResultContentType = ContentType.Xml;
			request.RequestUrl = BaseUrl.AppendQueryString(qParameters);
			string content = bodyParamters.SerializeXml();
			var stringContent = new StringContent(content, new UTF8Encoding(), XmlContentType);
			return await request.PostAsync<SaveOfficeMaintenanceResponse>(stringContent);
		}

		public async Task<ResponseWrapper<SaveDriverRequestResponse>> SaveDriverRequestAsync(SaveDriverRequestQParameters qParameters, SaveDriverRequestBParameters bParamters, CancellationToken? token = null)
		{
			qParameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.ResultContentType = ContentType.Xml;
			request.RequestUrl = BaseUrl.AppendQueryString(qParameters);

			var stringContent = new StringContent(bParamters.SerializeXml(), new UTF8Encoding(), XmlContentType);
			return await request.PostAsync<SaveDriverRequestResponse>(stringContent);
		}


		public async Task<ResponseWrapper<GetRoomsResponse>> GetRoomsAsync(GetRoomsQParameters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetRoomsResponse>(token);
		}

		public async Task<ResponseWrapper<GetOfficeLocationsResponse>> GetOfficeLocationsAsync(GetOfficeLocationsQParameters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetOfficeLocationsResponse>(token);
		}

		public async Task<ResponseWrapper<GetEquipmentsResponse>> GetEquipmentsAsync(GetEquipmentsQParameters parameters, CancellationToken? token = default(CancellationToken?))
		{
			parameters.Server = Sprint2Server;
			var request = _requestFactory();
			request.RequestUrl = BaseUrl.AppendQueryString(parameters);
			request.ResultContentType = ContentType.Xml;
			return await request.GetAsync<GetEquipmentsResponse>(token);
		}

		public async Task<ResponseWrapper<ServiceDeskRequests>> CancelServiceDeskRequests(ServiceDeskRequest serviceRequest, CancellationToken? token = default(CancellationToken?))
		{
			var request = _requestFactory();

			if (serviceRequest.Type == "Incidents")
			{
				request.RequestUrl = ServiceDiskBaseUrl + string.Format("UpdateIncidentStatus?ID={0}&status=canceled&comment=null&ActionBy=9wHHq0pN/U3QAtg5NxDpZQ==", serviceRequest.ID);
			}
			else
			{
				request.RequestUrl = ServiceDiskBaseUrl + string.Format("UpdateServiceRequestStatus?ID={0}&status=canceled&comment=null&ActionBy=9wHHq0pN/U3QAtg5NxDpZQ==", serviceRequest.ID);

			}
			request.ResultContentType = ContentType.Json;
			var stringContent = new StringContent("", new UTF8Encoding(), JSONContentType);
			var response = await request.PostAsync<ServiceDeskRequests>(stringContent, token);
			return response;
		}

		public async Task<ResponseWrapper<ServiceDeskCases>> AcceptServiceDeskCases(string caseId, CancellationToken? token = null)
		{
			var request = _requestFactory();
			request.RequestUrl = ServiceDiskBaseUrl + string.Format("Reviewer_ApproveWithRAID?ReviewerId=323&comment=georgecomment&RAID={0}", caseId);
			request.ResultContentType = ContentType.Json;
			var stringContent = new StringContent("", new UTF8Encoding(), JSONContentType);
			var response = await request.PostAsync<ServiceDeskCases>(stringContent, token);
			return response;

		}

		public async Task<ResponseWrapper<ServiceDeskCases>> RejectServiceDeskCases(string caseId, CancellationToken? token = null)
		{
			var request = _requestFactory();
			request.RequestUrl = ServiceDiskBaseUrl + string.Format("Reviewer_RejectWithRAID?ReviewerId=323&comment=georgecomment&RAID={0} ", caseId);
			request.ResultContentType = ContentType.Json;
			var stringContent = new StringContent("", new UTF8Encoding(), JSONContentType);
			var response = await request.PostAsync<ServiceDeskCases>(stringContent, token);
			return response;

		}




		#endregion
	}
}
