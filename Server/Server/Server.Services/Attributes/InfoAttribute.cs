using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class InfoAttribute : Attribute, IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                ((BaseController)context.Controller).LoadInfo();
            }
            catch { }
            return next();
        }
    }
}
