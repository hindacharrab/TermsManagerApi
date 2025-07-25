using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using TermsManagerAPI.Repositories.Interface;
using TermsManagerAPI.Helpers;


namespace CGUManagementAPI.Middlewares
{
    public class CGUAcceptanceMiddleware
    {
        private readonly RequestDelegate _next;

        public CGUAcceptanceMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserRepository userRepo, ICGURepository cguRepo)
        {
            // Ignorer les routes publiques ou d’authentification
            var path = context.Request.Path.ToString().ToLower();

            if (path.Contains("/auth") || path.Contains("/swagger") || path.Contains("/cgu/accept") || path.Contains("/cgu/check") || path.Contains("/cgu/latest"))
            {
                await _next(context);
                return;
            }

            // Vérifier si l'utilisateur est connecté
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userRepo.GetByIdAsync(int.Parse(userId));
                var latestCGU = await cguRepo.GetLatestVersionAsync();

                if (user != null && latestCGU != null && UserHelper.RequiresCGUAcceptance(user, latestCGU.Version))

                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Vous devez accepter la dernière version des CGU avant d'accéder à l'application.");
                    return;
                }
            }

            // Continuer la requête normalement
            await _next(context);
        }
    }
}
