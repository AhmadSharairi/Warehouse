using System.Security.Claims;
using Core.Entities;

namespace Core.Services
{
    public interface IAuthService
    {
     
         string CreateJwt(User user);
         string CreateRefreshToken();
          Task<bool> CheckEmailExistAsync(string email);
          ClaimsPrincipal GetPrincipleFromExpiredToken(string token);
    }
}
