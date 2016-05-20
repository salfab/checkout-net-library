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
        private WebRequestHandler _requestHandler;
        private HttpClient _httpClient;

        public ApiHttpClient()
        {
            ResetHandler();
        }

        public void ResetHandler()
        {
            _requestHandler?.Dispose();
            _requestHandler = new WebRequestHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip,
                AllowAutoRedirect = false,
                UseDefaultCredentials = false,
                UseCookies = false
            };

            _httpClient?.Dispose();

            _httpClient = new HttpClient(_requestHandler)
            {
                MaxResponseContentBufferSize = AppSettings.MaxResponseContentBufferSize,
                Timeout = TimeSpan.FromSeconds(AppSettings.RequestTimeout)
            };
            SetHttpRequestHeader("User-Agent",AppSettings.ClientUserAgentName);
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("Gzip"));
        }

        public void SetHttpRequestHeader(string name, string value)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(name))
            {
                _httpClient.DefaultRequestHeaders.Remove(name);
            }

            if (value != null)
            { _httpClient.DefaultRequestHeaders.Add(name, value); }
        }

        public string GetHttpRequestHeader(string name)
        {
            IEnumerable<string> values = null;
            _httpClient.DefaultRequestHeaders.TryGetValues(name, out values);

            if (values != null && values.Any())
            { return values.First(); }

            return null;
        }

        #region Get

        /// <summary>
        /// Submits a get request to the given web address with default content type e.g. text/plain
        /// </summary>
        public HttpResponse<T> GetRequest<T>(string requestUri, string authenticationKey)
        {
            var httpRequestMsg = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri)
            };

            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Post {requestUri}");
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits an asynchronous get request to the given web address with default content type e.g. text/plain
        /// </summary>
        public async Task<HttpResponse<T>> GetRequestAsync<T>(string requestUri, string authenticationKey)
        {
            var httpRequestMsg = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUri)
            };

            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);
            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Post {requestUri}");
            }

            return await SendRequestAsync<T>(httpRequestMsg);
        }

        #endregion

        #region Post

        /// <summary>
        /// Submits a post request to the given web address
        /// </summary>
        public HttpResponse<T> PostRequest<T>(string requestUri, string authenticationKey, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, AppSettings.DefaultContentType);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Post {requestUri}");
                Console.WriteLine($"\n\n** Payload ** \n {requestPayloadAsString} \n");
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits an asynchronous post request to the given web address
        /// </summary>
        public async Task<HttpResponse<T>> PostRequestAsync<T>(string requestUri, string authenticationKey, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Post, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, AppSettings.DefaultContentType);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Post {requestUri}");
                Console.WriteLine($"\n\n** Payload ** \n {requestPayloadAsString} \n");
            }

            return await SendRequestAsync<T>(httpRequestMsg);
        }

        #endregion

        #region Put

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
                Console.WriteLine($"\n\n** Request ** Put {requestUri}");
                Console.WriteLine($"\n\n** Payload ** \n {requestPayloadAsString} \n");
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits an asynchronous put request to the given web address
        /// </summary>
        public async Task<HttpResponse<T>> PutRequestAsync<T>(string requestUri, string authenticationKey, object requestPayload = null)
        {
            var httpRequestMsg = new HttpRequestMessage(HttpMethod.Put, requestUri);
            var requestPayloadAsString = GetObjectAsString(requestPayload);

            httpRequestMsg.Content = new StringContent(requestPayloadAsString, Encoding.UTF8, AppSettings.DefaultContentType);
            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Put {requestUri}");
                Console.WriteLine($"\n\n** Payload ** \n {requestPayloadAsString} \n");
            }

            return await SendRequestAsync<T>(httpRequestMsg);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Submits a delete request to the given web address
        /// </summary>
        public HttpResponse<T> DeleteRequest<T>(string requestUri, string authenticationKey)
        {
            var httpRequestMsg = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri)
            };

            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Delete {requestUri}");
            }

            return SendRequest<T>(httpRequestMsg).Result;
        }

        /// <summary>
        /// Submits an asynchronous delete request to the given web address
        /// </summary>
        public async Task<HttpResponse<T>> DeleteRequestAsync<T>(string requestUri, string authenticationKey)
        {
            var httpRequestMsg = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri(requestUri)
            };

            httpRequestMsg.Headers.Add("Accept", AppSettings.DefaultContentType);

            SetHttpRequestHeader("Authorization", authenticationKey);

            if (AppSettings.DebugMode)
            {
                Console.WriteLine($"\n\n** Request ** Delete {requestUri}");
            }

            return await SendRequest<T>(httpRequestMsg);
        }

        #endregion

        #region SendRequest

        /// <summary>
        /// Sends a http request with the given object. All headers should be set manually here e.g. content type=application/json
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private Task<HttpResponse<T>> SendRequest<T>(HttpRequestMessage request)
        {
            Task<HttpResponse<T>> response;
            HttpResponseMessage responseMessage = null;
            string responseAsString = null;

            try
            {
                responseMessage = _httpClient.SendAsync(request).Result;
                var responseContent = responseMessage.Content.ReadAsByteArrayAsync().Result;

                if (responseContent != null && responseContent.Length > 0)
                {
                    responseAsString = Encoding.UTF8.GetString(responseContent);

                    if (AppSettings.DebugMode)
                    {
                        Console.WriteLine($"\n** HttpResponse - Status {responseMessage.StatusCode}**\n {responseAsString}\n");
                    }
                }

                response = CreateHttpResponse<T>(responseAsString, responseMessage.StatusCode);
            }
            catch (Exception ex)
            {
                if (AppSettings.DebugMode)
                {
                    Console.WriteLine(
                        $@"\n** Exception - HttpStatuscode:\n{responseMessage?.StatusCode.ToString() ?? string.Empty}**\n\n 
                        ** ResponseString {responseAsString}\n ** Exception Messages{ExceptionHelper.FlattenExceptionMessages(ex)}\n ");
                }

                throw;
            }
            finally
            {
                request.Dispose();
                ResetHandler();
            }

            return response;
        }

        /// <summary>
        /// Sends an asynchronous http request with the given object. All headers should be set manually here e.g. content type=application/json
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<HttpResponse<T>> SendRequestAsync<T>(HttpRequestMessage request)
        {
            HttpResponse<T> response;
            HttpResponseMessage responseMessage = null;
            string responseAsString = null;

            try
            {
                responseMessage = await _httpClient.SendAsync(request);
                var responseContent = await responseMessage.Content.ReadAsByteArrayAsync();

                if (responseContent != null && responseContent.Length > 0)
                {
                    responseAsString = Encoding.UTF8.GetString(responseContent);

                    if (AppSettings.DebugMode)
                    {
                        Console.WriteLine($"\n** HttpResponse - Status {responseMessage.StatusCode}**\n {responseAsString}\n");
                    }
                }

                response = await CreateHttpResponse<T>(responseAsString, responseMessage.StatusCode);
            }
            catch (Exception ex)
            {
                if (AppSettings.DebugMode)
                {
                    Console.WriteLine($@"\n** Exception - HttpStatuscode:\n{responseMessage?.StatusCode.ToString() ?? string.Empty}**\n\n 
                        ** ResponseString {responseAsString}\n ** Exception Messages{ExceptionHelper.FlattenExceptionMessages(ex)}\n ");
                }

                throw;
            }
            finally
            {
                request.Dispose();
                ResetHandler();
            }

            return response;
        }

        #endregion

        #region Helpers

        private static async Task<HttpResponse<T>> CreateHttpResponse<T>(string responseAsString, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode == HttpStatusCode.OK && responseAsString != null)
            {
                return await Task.FromResult(new HttpResponse<T>(GetResponseAsObject<T>(responseAsString))
                {
                    HttpStatusCode = httpStatusCode
                });
            }
            if (responseAsString != null)
            {
                return await Task.FromResult(new HttpResponse<T>(default(T))
                {
                    Error = GetResponseAsObject<ResponseError>(responseAsString),
                    HttpStatusCode = httpStatusCode
                });
            }

            return null;
        }

        private static string GetObjectAsString(object requestModel)
        {
            return ContentAdaptor.ConvertToJsonString(requestModel);
        }

        private static T GetResponseAsObject<T>(string responseAsString)
        {
            return ContentAdaptor.JsonStringToObject<T>(responseAsString);
        }

        #endregion
    }
}
