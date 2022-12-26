using Newtonsoft.Json;
using Optimum.Http.Contracts;
using Optimum.Http.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Http
{
    public class HttpService : IHttpService
    {

        private readonly HttpClient _httpClient;

        public HttpService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResult<TResponse>> GetAsync<TResponse>(string url)
        {
            try
            {
                var res = await _httpClient.GetAsync(url);
                var content = await res.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<TResponse>(content);
                return new HttpResult<TResponse>
                {
                    Data = data,
                    IsOk = res.IsSuccessStatusCode,
                    StatusCode = res.StatusCode,
                    IsSuccess = true,
                    Message = null,
                };
            }
            catch (Exception e)
            {
                return new HttpResult<TResponse>
                {
                    IsOk = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = e.InnerException?.Message ?? e.Message,
                };
            }
        }

        public async Task<HttpResult<TResponse>> PostJsonAsync<TResponse, TBody>(string url, TBody body)
        {
            try
            {
                var bodyContent = JsonContent.Create(body);
                var res = await _httpClient.PostAsync(url,bodyContent);
                var content = await res.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<TResponse>(content);
                return new HttpResult<TResponse>
                {
                    Data = data,
                    IsOk = res.IsSuccessStatusCode,
                    StatusCode = res.StatusCode,
                    IsSuccess = true,
                    Message = null,
                };
            }
            catch (Exception e)
            {
                return new HttpResult<TResponse>
                {
                    IsOk = false,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    IsSuccess = false,
                    Message = e.InnerException?.Message ?? e.Message,
                };
            }
        }
    }
}
