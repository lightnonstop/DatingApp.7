using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var username = resultContext.HttpContext.User.GetUsername();

            var useRepo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var user = await useRepo.GetUserByUsernameAsync(username);
            user.LastActive = DateTime.UtcNow;
            await useRepo.SaveAllAsync();
        }
    }
}