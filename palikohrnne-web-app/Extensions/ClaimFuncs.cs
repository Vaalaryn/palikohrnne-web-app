using Microsoft.AspNetCore.Http;
using palikohrnne_web_app.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Extensions
{
    public static class ClaimFuncs
    {
        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static CubesService Authorize(this CubesService cubesService, ClaimsPrincipal userContext)
        {
            var claim = ((ClaimsIdentity)userContext.Identity);
            cubesService.RenseignerToken(claim.GetSpecificClaim("Token"));

            return cubesService;
        }
    }
}
