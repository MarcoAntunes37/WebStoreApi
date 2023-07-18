using WebStoreApi.Services;

namespace WebStoreApi.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUsersService userService, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var principal = jwtUtils.GetClaimsPrincipal(token);
            if (principal != null)
            {
                context.Items["User"] = await userService.GetByUsername(principal.Identity.Name);
            }

            await _next(context);
        }
    }
}
