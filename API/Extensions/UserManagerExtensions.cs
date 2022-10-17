using System.Security.Claims;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
  public static class UserManagerExtensions
  {
    public static async Task<AppUser> FindUserByClaimsPrincipalWithAdressAsync
    (this UserManager<AppUser> input,
    ClaimsPrincipal user)
    {
      //var email = user?.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
      var email = user.FindFirstValue(ClaimTypes.Email);

      return await input.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);
    }

      public static async Task<AppUser> FindByEmailFromClaimsPrincipal
    (this UserManager<AppUser> input,
    ClaimsPrincipal user)
    {
      //var email = user?.Claims?.FirstOrDefault(e => e.Type == ClaimTypes.Email)?.Value;
      var email = user.FindFirstValue(ClaimTypes.Email);

      return await input.Users.SingleOrDefaultAsync(x => x.Email == email);
    }
  
  }
}