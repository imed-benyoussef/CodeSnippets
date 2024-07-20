using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aiglusoft.IAM.Server.Extensions
{
    public static class ControllerExtensions
    {
        public static string? GetUserId(this ControllerBase controller)
        {
            var userIdClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User ID claim not found.");
            }

            return userIdClaim.Value;
        }
    }

}
