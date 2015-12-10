using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Strings;

namespace AdaaMobile.DataServices
{
    public interface IRequestMessageResolver
    {
        string GetMessage(ResponseWrapper wrapper);
    }

    public class RequestMessageResolver : IRequestMessageResolver
    {
        public string GetMessage(ResponseWrapper wrapper)
        {
            switch (wrapper.ResponseStatus)
            {
                case ResponseStatus.SuccessWithResult:
                    return AppResources.Ok;
                case ResponseStatus.NoInternet:
                    return AppResources.NoInternet;
                case ResponseStatus.InvalidToken:
                    return "Invalid Session, You will be redirected to login page";
                case ResponseStatus.SuccessWithNoData:
                    return AppResources.NoData;
                case ResponseStatus.UserCanceled:
                    return "";
                case ResponseStatus.HttpError:
                case ResponseStatus.ClientSideError:
                case ResponseStatus.TimeOut:
                case ResponseStatus.ParserException:
                case ResponseStatus.MalformedRequest:
                default:
                    return AppResources.LoadingError;
            }
        }
    }
}
