
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Extensions;
using AdaaMobile.Helpers;
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
        private readonly Func<BaseRequest> _requestFactory;
        private readonly IAppSettings _appSettings;
        public DataService(Func<BaseRequest> requestFactory, IAppSettings appSettings)
        {
            _requestFactory = requestFactory;
            _appSettings = appSettings;
        }

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
            return await request.PostAsync<LoginResponse>(stringContent);
        }

        public async Task<ResponseWrapper<GetAllEmployeesResponse>> GetEmpolyeesAsync(GetAllEmployeesQParameters parameters, CancellationToken? token = null)
        {
            parameters.Server = Server;
            var request = _requestFactory();
            request.RequestUrl = BaseUrl.AppendQueryString(parameters);
            request.ResultContentType = ContentType.Xml;
            return await request.GetAsync<GetAllEmployeesResponse>(token);

        }

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
            //parameters.Server = Server;
            //var request = _requestFactory();
            //request.RequestUrl = BaseUrl.AppendQueryString(parameters);
            //request.ResultContentType = ContentType.Xml;
            //return await request.GetAsync<GetExceptionsRepsonse>(token);
            //TODO:Change when implemented in Backend
            return new ResponseWrapper<GetExceptionsRepsonse>()
            {
                ResponseStatus = ResponseStatus.SuccessWithResult,
                Result = new GetExceptionsRepsonse()
                {
                    ExceptionDays = new ExceptionDay[] {
                        new ExceptionDay(){ Date =DateTime.Now.Subtract(TimeSpan.FromDays(4))},
                        new ExceptionDay(){ Date =DateTime.Now.Subtract(TimeSpan.FromDays(3))},
                        new ExceptionDay(){ Date =DateTime.Now.Subtract(TimeSpan.FromDays(1))},
                    }
                }
            };
        }

        public async Task<ResponseWrapper<AttendanceException>> GetAttendanceExceptionAsync(ExceptionDetailsQParamters parameters, CancellationToken? token = null)
        {
            parameters.Server = Server;
            var request = _requestFactory();
            request.RequestUrl = BaseUrl.AppendQueryString(parameters);
            request.ResultContentType = ContentType.Xml;
            return await request.GetAsync<AttendanceException>(token);
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
