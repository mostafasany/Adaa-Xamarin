using System;

namespace AdaaMobile.Helpers
{
    public class NetworkHelper : INetworkHelper
    {
        public bool HasInternetAccess()
        {
            return true;//TODO:Add connectivity Nuget package or it's functionality copied here.
        }
    }
}
