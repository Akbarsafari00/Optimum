using Hangfire;
using Optimum.Hangfire.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Hangfire
{
    public class HangfireService : IHangfireService
    {
        public async Task RecurringJobs<T>( string cron, Expression<Action<T>> methodCall)
        {
            RecurringJob.AddOrUpdate<T>(methodCall,cron);
        }
    }
}
