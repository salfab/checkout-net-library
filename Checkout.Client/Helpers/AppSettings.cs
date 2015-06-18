using Checkout.Utilities;
using System;
using System.Configuration;
using CheckoutEnvironment = Checkout.Helpers.Environment;
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
        private static double _clientVersion = 1.0;
        private static string _clientUserAgentName;
        private static int? _maxResponseContentBufferSize;
        private static CheckoutEnvironment _environment = CheckoutEnvironment.Undefined;


        public static string BaseApiUri { get { return _baseApiUri ?? (_baseApiUri = ConfigurationManager.AppSettings["BaseApiUri"]); } set { _baseApiUri = value; } }

        public static string PublicKey { get { return _publicKey ?? (_publicKey = ConfigurationManager.AppSettings["PublicKey"]); } set { _publicKey = value; } }

        public static string SecretKey { get { return _secretKey ?? (_secretKey = ConfigurationManager.AppSettings["SecretKey"]); } set { _secretKey = value; } }

        public static string DefaultContentType { get { return _defaultContentType ?? (_defaultContentType = "application/json"); } set { _defaultContentType = value; } }

        public static int RequestTimeout { get { return _requestTimeout ?? (_requestTimeout = int.Parse(ConfigurationManager.AppSettings["RequestTimeout"])).Value; } set { _requestTimeout = value; } }

        public static int MaxResponseContentBufferSize { get { return _maxResponseContentBufferSize ?? (_maxResponseContentBufferSize = int.Parse(ConfigurationManager.AppSettings["MaxResponseContentBufferSize"])).Value; } set { _maxResponseContentBufferSize = value; } }

        public static bool DebugMode { get { return _debugMode ?? (_debugMode = bool.Parse(ConfigurationManager.AppSettings["DebugMode"])).Value; } set { _debugMode = value; } }

        public static string ClientUserAgentName { get { return _clientUserAgentName ?? string.Format("Checkout-DotNetLibraryClient/{0}", _clientVersion); } }

        public static CheckoutEnvironment Environment
        {
            get
            {
                CheckoutEnvironment selectedEnvironment;

                if (_environment == CheckoutEnvironment.Undefined && Enum.TryParse<CheckoutEnvironment>(ConfigurationManager.AppSettings["Environment"], out selectedEnvironment) &&
                    Enum.IsDefined(typeof(CheckoutEnvironment), selectedEnvironment)
                    )
                {
                    return (_environment = selectedEnvironment);
                }

                return (_environment = CheckoutEnvironment.Sandbox);
            }

            set { _environment = value; }
        }
    }
}
