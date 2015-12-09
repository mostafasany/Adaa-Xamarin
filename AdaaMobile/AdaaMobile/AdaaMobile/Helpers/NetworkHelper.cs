using System;
using AdaaMobile.Plugins.Connectivity;

namespace AdaaMobile.Helpers
{
    public class NetworkHelper : INetworkHelper
    {
        public bool HasInternetAccess()
        {
            return CrossConnectivity.Current.IsConnected;
        }
    }
}
