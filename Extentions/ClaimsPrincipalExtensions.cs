using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MedicalAppointmentApi.Extentions
{
public static class ClaimsPrincipalExtensions
    {
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(idClaim, out var id)
                ? id
                : throw new UnauthorizedAccessException("Invalid user ID in token.");
        }

        public static string GetUserRole(this ClaimsPrincipal user)
        {
            var roleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
            return roleClaim ?? throw new UnauthorizedAccessException("Invalid role in token.");
        }
    }
}