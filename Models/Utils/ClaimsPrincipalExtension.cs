using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Medicare_API.Models.Utils
{
    public static class ClaimsPrincipalExtension
    {

     public static int GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id) ? id : 0;
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole("ADMIN");
        }

    }
}