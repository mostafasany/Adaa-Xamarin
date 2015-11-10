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

namespace AdaaMobile.DataServices
{
    public class DataService : IDataService
    {
        private const string BaseUrl = "http://adaa.getsandbox.com";
        private const string ContenTypeKey = "Content-Type";
        private const string XmlContentType = "application/xml";
        private readonly Func<BaseRequest> RequestFactory;
        public DataService(Func<BaseRequest> requestFactory)
        {
            RequestFactory = requestFactory;
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

        public Task<ResponseWrapper<List<Employee>>> GetEmpolyees(CancellationToken? token)
        {
            throw new NotImplementedException();

        }
    }
}
