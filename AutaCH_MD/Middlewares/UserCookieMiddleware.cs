using AutaCH_MD.Contexts;


namespace AutaCH_MD.Middlewares
{
    public class UserCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public UserCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            using(var scope = ctx.RequestServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDataContext>();

                if(ctx.Request.Cookies.TryGetValue("UserId", out string id))
                {
                    if (Guid.TryParse(id, out var userId))
                    {
                        var user = dbContext.Users.FirstOrDefault(user => user.UserId == userId);
                        if (user != null)
                        {
                            ctx.Session.SetString("Login", user.Login);
                            ctx.Session.SetString("Type", user.Type);
                        }

                        var cookiesOptions = new CookieOptions
                        {
                            Expires = DateTime.Now.AddYears(1)
                        };

                        ctx.Response.Cookies.Append("UserId", user.UserId.ToString(), cookiesOptions);
                    }
                    
                }

                await _next(ctx);
            }
        }
    }
}
