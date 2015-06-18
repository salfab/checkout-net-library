using Checkout.ApiServices.SharedModels;
using Checkout.Helpers;
using Checkout.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Checkout
{
    /// <summary>
    /// Handles http requests and responses
    /// </summary>
    public sealed class ApiHttpClient
    {
        private WebRequestHandler requestHandler;
        private HttpClient httpClient;

        public ApiHttpClient()
        {
            ResetHandler();
        }

        public void ResetHandler()
        {
            if (requestHandler != null)
            {
                requestHandler.Dispose();
            }
            requestHandler = new WebRequestHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                AllowAutoRedirect = false,
                UseDefaultCredentials = false,
                UseCookies = false
            };

            if (httpClient != null)
            {
                httpClient.Dispose();
            }

            httpClient = new HttpClient(requestHandler);
            httpClient.MaxResponseContentBufferSize = AppSettings.MaxResponseContentBufferSize;
            httpClient.Timeout = TimeSpan.FromSeconds(AppSettings.RequestTimeout);
            SetHttpRequestHeader("User-Agent",AppSettings.ClientUserAgentName);
            httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("Gzip"));
        }

        public void SetHttpRequestHeader(string name, string value)
        {
            if (httpClient.DefaultRequestHeaders.Contains(name))
            {
                httpClient.DefaultRequestHeaders.Remove(name);
            }

            if (value != null)
            { httpClient.DefaultRequestHeaders.Add(name, value); }
        }

        public string GetHttpRequestHeader(string name)
        {
            IEnumerable<string> values = null;
            httpClient.DefaultRequestHeaders.TryGetValues(name, out values);

            if (values != null && values.Any())
            { return values.First(); }

            return null;
        }

       
        /// <summary>
        /// Submits a get request to the given web address with default content type e.g. text/plain
        /// </summary>
        public HttpResponse<T> GetRequest<T>(string requestUri,string authenticationKey)
        {
            var httpRequestMsg = new HttpRequestMessage();

            httpRequestMsg.Method = HttpMethod.Get;
            httpRequestMsg.RequestUri = new Uri(requestUri);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine(string.Format("\n\n** Request ** Post {0}", requestUri));
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits a post request to the given web address
        /// </summary>
        public HttpResponse<T> PostRequest<T>(string requestUri,string authenticationKey, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, AppSettings.DefaultContentType);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);
            
            SetHttpRequestHeader("Authorization", authenticationKey);
            
            if (AppSettings.DebugMode)
            {
                Console.WriteLine(string.Format("\n\n** Request ** Post {0}", requestUri));
                Console.WriteLine(string.Format("\n\n** Payload ** \n {0} \n", requestPayloadAsString));
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits a put request to the given web address
        /// </summary>
        public HttpResponse<T> PutRequest<T>(string requestUri, string authenticationKey, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Put, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, AppSettings.DefaultContentType);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine(string.Format("\n\n** Request ** Put {0}", requestUri));
                Console.WriteLine(string.Format("\n\n** Payload ** \n {0} \n", requestPayloadAsString));
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits a delete request to the given web address
        /// </summary>
        public HttpResponse<T> DeleteRequest<T>(string requestUri, string authenticationKey)
        {
            var httpRequestMsg = new HttpRequestMessage();

            httpRequestMsg.Method = HttpMethod.Delete;
            httpRequestMsg.RequestUri = new Uri(requestUri);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine(string.Format("\n\n** Request ** Delete {0}", requestUri));
            }

            return SendRequest<T>(httpRequestMsg).Result; 
        }

        /// <summary>
        /// Sends a http request with the given object. All headers should be set manually here e.g. content type=application/json
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<HttpResponse<T>> SendRequest<T>(HttpRequestMessage request)
        {
            HttpResponse<T> response = null;
            HttpResponseMessage responseMessage = null;
            string responseAsString = null;
            string responseCode = null;

            try
            {
                responseMessage = await httpClient.SendAsync(request); 
               
                responseCode = responseMessage.StatusCode.ToString();

                var responseContent = responseMessage.Content.ReadAsByteArrayAsync().Result;

                if (responseContent != null && responseContent.Length > 0)
                {
                    responseAsString = Encoding.UTF8.GetString(responseContent);

                    if (AppSettings.DebugMode)
                    {
                        Console.WriteLine(string.Format("\n** HttpResponse - Status {0}**\n {1}\n", responseMessage.StatusCode, responseAsString));
                    }
                }

                response = CreateHttpResponse<T>(responseAsString, responseMessage.StatusCode);

                #region log
                //if (response.ContentType == HttpContentTypes.Xml || response.ContentType == HttpContentTypes.Json)
                //{
                //    if (HttpStatusCode.OK == response.HttpResponseStatusCode)
                //    {
                //        string responseContent;

                //        //handle Jsonp content
                //        var regex = new System.Text.RegularExpressions.Regex(@"\((.*)\)$");
                //        var match = regex.Match(response.HttpResponseAsString);
                //        if (match.Success && match.Value != string.Empty)
                //        {
                //            responseContent = match.Value;

                //            //Get rid of enclosing brackets
                //            responseContent = responseContent.StartsWith("(") ? responseContent.Substring(1, responseContent.Length - 1) : responseContent;
                //            responseContent = responseContent.EndsWith(")") ? responseContent.Substring(0, responseContent.Length - 1) : responseContent;

                //            response.HttpResponseAsString = responseContent;

                //            Console.WriteLine(string.Format("\n** HttpResponse is JsonP callback {0}**\n {1}\n", response.HttpResponseStatusCode, response.HttpResponseAsJObject.ToString()));
                //        }
                //        else
                //        {
                //            //Format output
                //            responseContent = response.ContentType == HttpContentTypes.Xml ?
                //                                        response.HttpResponseAsXmlDocument.XToIndentedString() :
                //                                        response.HttpResponseAsJObject.ToString();

                //            Console.WriteLine(string.Format("\n** HttpResponse {0}**\n {1}\n", response.HttpResponseStatusCode, responseContent));
                //        }


                //    }
                //    else
                //    {
                //        Console.WriteLine(string.Format("\n** HttpResponse {0}**\n {1}\n", response.HttpResponseStatusCode, response.HttpResponseAsString));
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                if (AppSettings.DebugMode)
                {
                    Console.WriteLine(string.Format(@"\n** Exception - HttpStatuscode:\n{0}**\n\n 
                        ** ResponseString {1}\n ** Exception Messages{2}\n ", (responseMessage != null ? responseMessage.StatusCode.ToString() : string.Empty), responseAsString, ExceptionHelper.FlattenExceptionMessages(ex)));
                }

                responseCode = "Exception" + ex.Message;

                throw;
            }
            finally
            {
                request.Dispose();
                ResetHandler();
            }

            return response;
        }

        private HttpResponse<T> CreateHttpResponse<T>(string responseAsString, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.OK && responseAsString != null)
            {
                return new HttpResponse<T>(GetResponseAsObject<T>(responseAsString))
                {
                    HttpStatusCode = httpStatusCode
                };
            }
            else if (responseAsString != null)
            {
                return new HttpResponse<T>(default(T))
                {
                    Error = GetResponseAsObject<ResponseError>(responseAsString),
                    HttpStatusCode = httpStatusCode
                };
            }

            return null;
        }

        private string GetObjectAsString(object requestModel)
        {
            return ContentAdaptor.ConvertToJsonString(requestModel);
        }

        private T GetResponseAsObject<T>(string responseAsString)
        {
            return ContentAdaptor.JsonStringToObject<T>(responseAsString);
        }

    }
}
