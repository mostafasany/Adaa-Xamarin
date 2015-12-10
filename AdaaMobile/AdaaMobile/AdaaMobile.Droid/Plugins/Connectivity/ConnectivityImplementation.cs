//Author jamesmontemagno
//Source: https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Connectivity
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdaaMobile.Droid.Plugins.Connectivity;
using AdaaMobile.Plugins.Connectivity;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Java.Net;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(ConnectivityImplementation))]
namespace AdaaMobile.Droid.Plugins.Connectivity
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class ConnectivityImplementation : BaseConnectivity
    {
        private ConnectivityChangeBroadcastReceiver receiver;
        /// <summary>
        /// Default constructor
        /// </summary>
        public ConnectivityImplementation()
        {
            ConnectivityChangeBroadcastReceiver.ConnectionChanged = OnConnectivityChanged;
            receiver = new ConnectivityChangeBroadcastReceiver();
            Application.Context.RegisterReceiver(receiver, new IntentFilter(ConnectivityManager.ConnectivityAction));
        }
        private ConnectivityManager connectivityManager;
        private WifiManager wifiManager;

        ConnectivityManager ConnectivityManager
        {
            get
            {
                connectivityManager = connectivityManager ??
                                       (ConnectivityManager)
                                       (Application.Context
                                           .GetSystemService(Context.ConnectivityService));
                return connectivityManager;
            }
        }

        WifiManager WifiManager
        {
            get
            {
                wifiManager = wifiManager ??
                               (WifiManager)
                               (Application.Context.GetSystemService(Context.WifiService));
                return wifiManager;
            }
        }

        /// <summary>
        /// Gets if there is an active internet connection
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                try
                {
                    var activeConnection = ConnectivityManager.ActiveNetworkInfo;

                    return ((activeConnection != null) && activeConnection.IsConnected);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Unable to get connected state - do you have ACCESS_NETWORK_STATE permission? - error: {0}", e);
                    return false;
                }
            }
        }

        /// <summary>
        /// Tests if a host name is pingable
        /// </summary>
        /// <param name="host">The host name can either be a machine name, such as "java.sun.com", or a textual representation of its IP address (127.0.0.1)</param>
        /// <param name="msTimeout">Timeout in milliseconds</param>
        /// <returns></returns>
        public override async Task<bool> IsReachable(string host, int msTimeout = 5000)
        {
            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");

            if (!IsConnected)
                return false;

            return await Task.Run(() =>
            {
                bool reachable;
                try
                {
                    reachable = InetAddress.GetByName(host).IsReachable(msTimeout);
                }
                catch (UnknownHostException ex)
                {
                    Debug.WriteLine("Unable to reach: " + host + " Error: " + ex);
                    reachable = false;
                }
                catch (Exception ex2)
                {
                    Debug.WriteLine("Unable to reach: " + host + " Error: " + ex2);
                    reachable = false;
                }
                return reachable;
            });

        }

        /// <summary> 
        /// Tests if a remote host name is reachable 
        /// </summary>
        /// <param name="host">Host name can be a remote IP or URL of website (no http:// or www.)</param>
        /// <param name="port">Port to attempt to check is reachable.</param>
        /// <param name="msTimeout">Timeout in milliseconds.</param>
        /// <returns></returns>
        public override async Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000)
        {

            if (string.IsNullOrEmpty(host))
                throw new ArgumentNullException("host");

            if (!IsConnected)
                return false;

            host = host.Replace("http://www.", string.Empty).
              Replace("http://", string.Empty).
              Replace("https://www.", string.Empty).
              Replace("https://", string.Empty);

            return await Task.Run(async () =>
            {
                try
                {
                    var sockaddr = new InetSocketAddress(host, port);
                    using (var sock = new Socket())
                    {

                        await sock.ConnectAsync(sockaddr, msTimeout);
                        return true;

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Unable to reach: " + host + " Error: " + ex);
                    return false;
                }
            });
        }

        /// <summary>
        /// Gets the list of all active connection types.
        /// </summary>
        public override IEnumerable<ConnectionType> ConnectionTypes
        {
            get
            {
                try
                {
                    ConnectionType type;
                    var activeConnection = ConnectivityManager.ActiveNetworkInfo;
                    switch (activeConnection.Type)
                    {
                        case ConnectivityType.Wimax:
                            type = ConnectionType.Wimax;
                            break;
                        case ConnectivityType.Wifi:
                            type = ConnectionType.WiFi;
                            break;
                        default:
                            type = ConnectionType.Cellular;
                            break;
                    }
                    return new ConnectionType[] { type };
                }
                catch (Exception ex)
                {
                    //no connections
                    return new ConnectionType[] { };
                }
            }
        }

        /// <summary>
        /// Retrieves a list of available bandwidths for the platform.
        /// Only active connections.
        /// </summary>
        public override IEnumerable<UInt64> Bandwidths
        {
            get
            {
                try
                {
                    if (ConnectionTypes.Contains(ConnectionType.WiFi))
                        return new[] { (UInt64)WifiManager.ConnectionInfo.LinkSpeed };
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Unable to get connected state - do you have ACCESS_WIFI_STATE permission? - error: {0}", e);
                }

                return new UInt64[] { };
            }
        }

        private bool disposed = false;


        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        public override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {



                    if (receiver != null)
                        Application.Context.UnregisterReceiver(receiver);

                    ConnectivityChangeBroadcastReceiver.ConnectionChanged = null;
                    if (wifiManager != null)
                    {
                        wifiManager.Dispose();
                        wifiManager = null;
                    }

                    if (connectivityManager != null)
                    {
                        connectivityManager.Dispose();
                        connectivityManager = null;
                    }

                }

                disposed = true;
            }

            base.Dispose(disposing);
        }


    }
}