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
//using System.Security;
//using System.Security.Cryptography;

namespace AdaaMobile.DataServices
{
    public class DataService : IDataService
    {
        // private const string BaseUrl = "http://adaa.getsandbox.com";
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

        public string EncryptString(string toEncrypt)
        {
            //byte[] keyArray;
            //byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //// Get the key from config file
            //string key = "MOEWPasswordMOEWPassword";

            //keyArray = UTF8Encoding.UTF8.GetBytes(key);

            //System.Security.Cryptography.TripleDESCryptoServiceProvider tdes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            ////set the secret key for the tripleDES algorithm
            //tdes.Key = keyArray;
            ////mode of operation. there are other 4 modes. We choose ECB(Electronic code Book)
            //tdes.Mode = System.Security.Cryptography.CipherMode.ECB;
            ////padding mode(if any extra byte added)
            //tdes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

            //System.Security.Cryptography.ICryptoTransform cTransform = tdes.CreateEncryptor();
            ////transform the specified region of bytes array to resultArray
            //byte[] resultArray = cTransform.TransformFinalBlock
            //        (toEncryptArray, 0, toEncryptArray.Length);
            ////Release resources held by TripleDes Encryptor
            //tdes.Clear();
            ////Return the encrypted data into unreadable string format
            //return Convert.ToBase64String(resultArray, 0, resultArray.Length);
             return "mhibnQQwVQQJ3d8gmlnKJg==";
        }


        public async Task<ResponseWrapper<bool>> SubmitTimeSheet(int year, int week, TimeSheetListRequest bodyParamters, CancellationToken? token = null)
        {
            var encryptedUserName = EncryptString(LoggedUserInfo.CurrentUserProfile.DisplayName);
            var request = _requestFactory();
            request.ResultContentType = ContentType.Json;
            request.RequestUrl = TimeSheetBaseUrl + string.Format("SaveTimeSheet?encryptedUserName={0}&currentWeek={1}&year={2}", encryptedUserName, week, year);
            var result = JsonConvert.SerializeObject(bodyParamters);
            var stringContent = new StringContent(result,new UTF8Encoding(),JSONContentType);
            return await request.PostAsync<bool>(stringContent);
        }

        public async Task<ResponseWrapper<TimeSheet>> GetTimeSheet(int year, int week, CancellationToken? token = null)
        {
            var encryptedUserName = EncryptString(LoggedUserInfo.CurrentUserProfile.DisplayName);
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

        public async Task<ResponseWrapper<List<Assignment>>> GetAssignmentAsync(CancellationToken? token = null)
        {
            var encryptedUserName = EncryptString(LoggedUserInfo.CurrentUserProfile.DisplayName);
            var request = _requestFactory();
            request.RequestUrl = TimeSheetBaseUrl + string.Format("GetFilteredAssignments?encryptedUserName={0}", encryptedUserName);
            request.ResultContentType = ContentType.Json;
            return await request.GetAsync<List<Assignment>>(token);
        }

        public async Task<ResponseWrapper<List<PendingTask>>> GetPendingTaskAsync(CancellationToken? token = null)
        {
            var encryptedUserName = EncryptString(LoggedUserInfo.CurrentUserProfile.DisplayName);
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


        #endregion
    }
}
