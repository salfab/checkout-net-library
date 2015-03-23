using Checkout.Utilities;
using System;
using System.Configuration;

namespace Checkout
{
    /// <summary>
    /// Holds application settings that is read from the app.config or web.config
    /// </summary>
    public sealed class AppSettings
    {
        private static string _baseApiUri;
        private static int? _requestTimeout;
        private static string _secretKey;
        private static string _publicKey;
        private static string _defaultContentType;
        private static bool? _debugMode;
        private static double _clientVersion=1.0;
        private static string _clientUserAgentName;
        private static int? _maxResponseContentBufferSize;


        public static string BaseApiUri { get { return _baseApiUri ?? (_baseApiUri = ConfigurationManager.AppSettings["BaseApiUri"]); } }
        public static string PublicKey { get { return _publicKey?? (_publicKey = ConfigurationManager.AppSettings["PublicKey"]); } }
        public static string SecretKey { get { return _secretKey?? (_secretKey = ConfigurationManager.AppSettings["SecretKey"]); } }
        public static string DefaultContentType { get { return _defaultContentType ?? (_defaultContentType = "application/json"); } }
        public static int RequestTimeout { get { return _requestTimeout?? (_requestTimeout=int.Parse(ConfigurationManager.AppSettings["RequestTimeout"])).Value; } }
        public static int MaxResponseContentBufferSize { get { return _maxResponseContentBufferSize ?? (_maxResponseContentBufferSize = int.Parse(ConfigurationManager.AppSettings["MaxResponseContentBufferSize"])).Value; } }
        public static bool DebugMode { get { return _debugMode ?? (_debugMode=bool.Parse(ConfigurationManager.AppSettings["DebugMode"])).Value; } }
        public static string ClientUserAgentName { get { return _clientUserAgentName ?? string.Format("Checkout-DotNetLibraryClient/{0}", _clientVersion); } }
    }
}
