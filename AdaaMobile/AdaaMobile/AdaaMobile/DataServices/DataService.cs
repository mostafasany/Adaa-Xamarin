
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Extensions;
using AdaaMobile.Models;
using AdaaMobile.Models.Request;
using AdaaMobile.Models.Response;
using QueryExtensions;

namespace AdaaMobile.DataServices
{
    public class DataService : IDataService
    {
        // private const string BaseUrl = "http://adaa.getsandbox.com";
        private const string BaseUrl = "https://adaamobile.adaa.abudhabi.ae/proxyservice/proxy";
        private const string Server = "adaamobile";
        private const string ContenTypeKey = "Content-Type";
        private const string XmlContentType = "application/xml";
        private readonly Func<BaseRequest> RequestFactory;
        public DataService(Func<BaseRequest> requestFactory)
        {
            RequestFactory = requestFactory;
        }

        public async Task<ResponseWrapper<UserProfile>> GetCurrentUserProfile(CurrentUserProfileQParameters paramters, CancellationToken? token = null)
        {
            var request = RequestFactory();
            request.RequestUrl = BaseUrl.AppendQueryString(paramters);
            request.ResultContentType = ContentType.Xml;
            return await request.GetAsync<UserProfile>(token);
        }

        public async Task<ResponseWrapper<LoginResponse>> LoginAsync(string userName, string password)
        {
            var request = RequestFactory();
            request.RequestUrl = string.Format("{0}/{1}", BaseUrl, "validatelogin");
            request.ResultContentType = ContentType.Xml;
            var loginParamters = new LoginBodyParamters()
            {
                UserName = userName,
                Password = password
            };
            var stringContent = new StringContent(loginParamters.SerializeXml(), new UTF8Encoding(), XmlContentType);
            return await request.PostAsync<LoginResponse>(stringContent);
        }

        public async Task<ResponseWrapper<GetAllEmployeesResponse>> GetEmpolyeesAsync(GetAllEmployeesQParameters parameters, CancellationToken? token = null)
        {
            parameters.Server = Server;
            var request = RequestFactory();
            request.RequestUrl = BaseUrl.AppendQueryString(parameters);
            request.ResultContentType = ContentType.Xml;
            return await request.GetAsync<GetAllEmployeesResponse>(token);

        }

        public async Task<ResponseWrapper<Attendance>> GetAttendanceRecordAsync(AttendanceQParameters parameters, CancellationToken? token = null)
        {
            parameters.Server = Server;
            var request = RequestFactory();
            request.RequestUrl = BaseUrl.AppendQueryString(parameters);
            request.ResultContentType = ContentType.Xml;
            return await request.GetAsync<Attendance>(token);
        }

        public Task<ResponseWrapper<AttendanceException>> GetAttendanceExceptionAsync(AttExceptionQParamters parameters, CancellationToken? token = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWrapper<NewDayPassResponse>> NewDayPassAsync(DaypassRequestQParameters qParameters, DaypassRequestBParameters bParamters, CancellationToken? token = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWrapper<DayPassesResponse>> GetPendingDayPassesAsync(DayPassesQParameters parameters, CancellationToken? token = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWrapper<DayPassApproveResponse>> DayPassApproveAsync(DaypassApproveQParameters parameters, CancellationToken? token = null)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseWrapper<DayPassTasksResponse>> DayPassTasksResponseAsync(DayPassTasksQParameters parameters, CancellationToken? token = null)
        {
            throw new NotImplementedException();
        }
    }
}
