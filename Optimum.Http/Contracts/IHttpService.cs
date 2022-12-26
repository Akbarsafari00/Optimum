using Optimum.Http.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Http.Contracts
{
    public interface IHttpService 
    {
        public Task<HttpResult<TResponse>> GetAsync<TResponse>(string url);
        public Task<HttpResult<TResponse>> PostJsonAsync<TResponse,TBody>(string url , TBody body);

    }
}
