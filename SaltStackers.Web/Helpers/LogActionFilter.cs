using SaltStackers.Application.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Dynamic;
using System.Security.Claims;

namespace SaltStackers.Web.Helpers
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Any())
            {
                context.ActionArguments.TryGetValue("model", out dynamic? model);
                if (!object.ReferenceEquals(null, model))
                {
                    if (model != null)
                    {
                        if (IsPropertyExist(model, "LogInfo"))
                        {
                            model.LogInfo = context.HttpContext.Request.GetClientInfo();
                        }

                        if (IsPropertyExist(model, "LogUserId") && context.HttpContext.User.Identity != null &&
                            context.HttpContext.User.Identity?.IsAuthenticated)
                        {
                            model.LogUserId =
                                context.HttpContext.User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier)
                                    ?.Value ?? string.Empty;
                        }

                        context.ActionArguments["model"] = model;
                    }
                }
            }

            base.OnActionExecuting(context);
        }

        private static bool IsPropertyExist(dynamic settings, string name)
        {
            if (settings is ExpandoObject)
                return ((IDictionary<string, object>)settings).ContainsKey(name);

            return settings.GetType().GetProperty(name) != null;
        }
    }
}
