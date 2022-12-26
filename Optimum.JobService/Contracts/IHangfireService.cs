using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Optimum.Hangfire.Contracts
{
    public interface IHangfireService
    {
        Task RecurringJobs<T>(string cron, Expression<Action<T>> methodCall);
    }
}
