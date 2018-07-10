using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NETCore_Angular1.Modules.Auth
{
    [Authorize]
    public class IdentityModule : BaseModule
    {
        public IdentityModule() : base("/auth")
        {
            Get("/identity", args => new JsonResult("Auth Works"));
        }
    }
}
