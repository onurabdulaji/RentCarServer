using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using RentCarServer.Application.Services;

namespace RentCarServer.Infrastructure.Services;

internal class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;

        var claims = httpContext.User.Claims;

        string? userId = claims.FirstOrDefault(q => q.Type == ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            throw new ArgumentNullException("Kullanici bilgisi bulunamadi");
        }

        try
        {
            var id = Guid.Parse(userId);
            return id;

        }
        catch (Exception)
        {
            throw new ArgumentNullException("Kullanici Uygun GUID Formatinda Degil !");
        }
    }
}