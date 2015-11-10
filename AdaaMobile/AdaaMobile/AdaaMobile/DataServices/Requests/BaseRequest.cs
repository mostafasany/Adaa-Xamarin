using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AdaaMobile.Helpers;

namespace AdaaMobile.DataServices.Requests
{
    public class BaseRequest
    {
        /// <summary>
        /// This is used to add headers to http client.
        /// </summary>
        private Dictionary<string, string> _headersDictionary;

        public HttpMessageHandler HttpMessageHandler { get; set; }

        public string RequestUrl { get; set; }

        /// <summary>
        /// Avoid retrieving results from the client side cache, Default is true.
        /// </summary>
        public bool NoCahce { get; set; }

        /// <summary>
        /// Default is true, which will dispose handler automatically
        /// </summary>
        public bool DisposeHandler = true;

        private readonly INetworkHelper _networkHelper;

        private ContentType _resultContentType = ContentType.Xml;
        /// <summary>
        /// Default is Xml
        /// </summary>
        public ContentType ResultContentType
        {
            get { return _resultContentType; }
            set { _resultContentType = value; }
        }

        /// <summary>
        /// Override User Agent of request
        /// </summary>
        public string UserAgent { get; set; }

        public void AddHeader(string key, string value)
        {
            if (_headersDictionary == null)
            {
                _headersDictionary = new Dictionary<string, string>();
            }
            _headersDictionary[key] = value;
        }

        public BaseRequest(INetworkHelper networkHelper)
        {
            _networkHelper = networkHelper;
            NoCahce = true;
        }

        public async Task<ResponseWrapper<TR>> PostAsync<TR>(StringContent content, CancellationToken? token = null)
        {

            var response = new ResponseWrapper<TR>();
            HttpClient httpClient = null;
            try
            {
                if (!_networkHelper.HasInternetAccess())
                {
                    response.ResponseStatus = ResponseStatus.NoInternet;
                    return response;
                }

                //If HttpMessageHandler is null, 
                //create new HttpClientHandler to set DisposeHandler boolean
                httpClient = HttpMessageHandler != null ?
                    new HttpClient(HttpMessageHandler, DisposeHandler) :
                    new HttpClient(new HttpClientHandler(), DisposeHandler);

                if (NoCahce)
                    httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                if (!String.IsNullOrEmpty(UserAgent))
                    httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                //Add headers to the http client request, if any.
                if (_headersDictionary != null && _headersDictionary.Count > 0)
                {
                    foreach (var key in _headersDictionary.Keys)
                    {
                        httpClient.DefaultRequestHeaders.Add(key, _headersDictionary[key]);
                    }
                }


                HttpResponseMessage postResponse;
                if (token == null)
                    postResponse = await httpClient.PostAsync(RequestUrl, content);
                else
                    postResponse = await httpClient.PostAsync(RequestUrl, content, token.Value);

                if (token != null && token.Value.IsCancellationRequested)
                {
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                    return response;
                }

                var stringValue = await postResponse.Content.ReadAsStringAsync();
                response.StatusCode = postResponse.StatusCode;
                ParseResult(stringValue, response);//parse Result even when there is error
                if (!postResponse.IsSuccessStatusCode)
                {
                    response.ResponseStatus = ResponseStatus.HttpError;//Override responseWrapper status value to be HttpError
                }
                return response;
            }
            catch (OperationCanceledException)
            {
                if (token != null && token.Value.IsCancellationRequested)
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                else
                    response.ResponseStatus = ResponseStatus.TimeOut;
            }
            catch (WebException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch (HttpRequestException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.ClientSideError;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();

            }
            return response;
        }

        public async Task<ResponseWrapper<TR>> GetAsync<TR>(CancellationToken? token = null)
        {
            var response = new ResponseWrapper<TR>();
            HttpClient httpClient = null;
            try
            {
                if (!_networkHelper.HasInternetAccess())
                {
                    response.ResponseStatus = ResponseStatus.NoInternet;
                    return response;
                }

                //If HttpMessageHandler is null, 
                //create new HttpClientHandler to set DisposeHandler boolean
                httpClient = HttpMessageHandler != null ?
                      new HttpClient(HttpMessageHandler, DisposeHandler) :
                      new HttpClient(new HttpClientHandler(), DisposeHandler);

                if (NoCahce)
                    httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue() { NoCache = true };

                //Add headers to the http client request, if any.
                if (_headersDictionary != null && _headersDictionary.Count > 0)
                {
                    foreach (var key in _headersDictionary.Keys)
                    {
                        httpClient.DefaultRequestHeaders.Add(key, _headersDictionary[key]);
                    }
                }

                HttpResponseMessage getResponse;
                if (token == null)
                    getResponse = await httpClient.GetAsync(RequestUrl);
                else
                    getResponse = await httpClient.GetAsync(RequestUrl, token.Value);

                if (token != null && token.Value.IsCancellationRequested)
                {
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                    return response;
                }

                response.StatusCode = getResponse.StatusCode;
                var stringValue = await getResponse.Content.ReadAsStringAsync();
                ParseResult(stringValue, response);//parse Result even when there is error
                if (!getResponse.IsSuccessStatusCode)
                {
                    response.ResponseStatus = ResponseStatus.HttpError;//Override responseWrapper status value to be HttpError
                }

                return response;
            }
            catch (OperationCanceledException)
            {
                if (token != null && token.Value.IsCancellationRequested)
                    response.ResponseStatus = ResponseStatus.UserCanceled;
                else
                    response.ResponseStatus = ResponseStatus.TimeOut;
            }
            catch (WebException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch (HttpRequestException)
            {
                response.ResponseStatus = ResponseStatus.HttpError;
            }
            catch (Exception)
            {
                response.ResponseStatus = ResponseStatus.ClientSideError;
            }
            finally
            {
                if (httpClient != null) httpClient.Dispose();
            }
            return response;
        }

        private void ParseResult<TR>(string stringValue, ResponseWrapper<TR> responseWrapper)
        {
            if (String.IsNullOrWhiteSpace(stringValue))
            {
                responseWrapper.ResponseStatus = ResponseStatus.SuccessWithNoData;
                return;
            }
            try
            {

                if (ResultContentType == ContentType.Json)
                {
                    //var result = JsonConvert.DeserializeObject<TR>(stringValue);
                    //responseWrapper.Result = result;
                    //responseWrapper.ResponseStatus = ResponseStatus.SuccessWithResult;
                    throw new NotImplementedException();
                }
                else if (ResultContentType == ContentType.Xml)
                {
                    var serializer = new XmlSerializer(typeof(TR));
                    using (var reader = new StringReader(stringValue))
                    {
                        responseWrapper.Result = (TR)serializer.Deserialize(reader);
                    }
                }
                else if (ResultContentType == ContentType.Text)
                {
                    //Only TR with type string can be assigned to Text
                    if (typeof(TR) != typeof(string))
                    {
                        responseWrapper.ResponseStatus = ResponseStatus.ParserException;
                        return;
                    }
                    responseWrapper.Result = (TR)(object)stringValue;
                }
            }
            catch (Exception)
            {
                responseWrapper.ResponseStatus = ResponseStatus.ParserException;
                return;
            }

            if (responseWrapper.Result.Equals(default(TR))) responseWrapper.ResponseStatus = ResponseStatus.SuccessWithNoData;
            //Check if the result is of type Ilist, and the count ==0, if true assign ResponseStatus to no Data
            else if (responseWrapper.Result is IList && (responseWrapper.Result as IList).Count == 0) responseWrapper.ResponseStatus = ResponseStatus.SuccessWithNoData;
        }
    }

    public enum ContentType
    {
        Text,
        Json,
        Xml,
    }
}
