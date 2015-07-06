using Checkout.Utilities;
using System;
using System.Collections.Generic;
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
        private static string _defaultContentType;
        private static bool? _debugMode;
        private static double _clientVersion = 1.0;
        private static string _clientUserAgentName;
        private static int? _maxResponseContentBufferSize;
        private static string _environment;
        private const string _liveUrl = "https://api2.checkout.com/v2";
        private const string _sandboxUrl = "http://sandbox.checkout.com/api2/v2";


        public static string BaseApiUri { get { return _baseApiUri; } set { _baseApiUri = value; } }

        public static string SecretKey { get { return _secretKey ?? (_secretKey = ReadConfig("Checkout.SecretKey")); } set { _secretKey = value; } }

        public static string DefaultContentType { get { return _defaultContentType ?? (_defaultContentType = "application/json"); } set { _defaultContentType = value; } }

        public static int RequestTimeout { get { return _requestTimeout ?? (_requestTimeout = int.Parse(ReadConfig("Checkout.RequestTimeout"))).Value; } set { _requestTimeout = value; } }

        public static int MaxResponseContentBufferSize { get { return _maxResponseContentBufferSize ?? (_maxResponseContentBufferSize = int.Parse(ReadConfig("Checkout.MaxResponseContentBufferSize"))).Value; } set { _maxResponseContentBufferSize = value; } }

        public static bool DebugMode { get { return _debugMode ?? (_debugMode = bool.Parse(ReadConfig("Checkout.DebugMode"))).Value; } set { _debugMode = value; } }

        public static string ClientUserAgentName { get { return _clientUserAgentName ?? string.Format("Checkout-DotNetLibraryClient/{0}", _clientVersion); } }

        public static void SetEnvironment(CheckoutEnvironment env)
        {

            switch (env)
            {
                case CheckoutEnvironment.Live: 
                    _baseApiUri = _liveUrl;
                    ApiUrls.ResetApiUrls();
                    break;
                case CheckoutEnvironment.Sandbox:
                    _baseApiUri = _sandboxUrl;
                    ApiUrls.ResetApiUrls();
                    break;
                case CheckoutEnvironment.Undefined:
                     CheckoutEnvironment selectedEnvironment;

                     if (Enum.TryParse<CheckoutEnvironment>(ReadConfig("Checkout.Environment"), out selectedEnvironment) && Enum.IsDefined(typeof(CheckoutEnvironment), selectedEnvironment))
                    {   SetEnvironment(selectedEnvironment); }
                    else
                    { throw new KeyNotFoundException("App Settings Key not found for: Environment"); }
                    
                    break;

                default: 
                    _baseApiUri = _sandboxUrl;
                    ApiUrls.ResetApiUrls();
                    break;
            };
        }

        private static string ReadConfig(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch (Exception)
            {
                throw new KeyNotFoundException("App Settings Key not found for: " + key);
            }
        }
    }
}
