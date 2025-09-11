using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hospital.UI.Areas.Admin.Filters
{
    public class SuperAdminOnlyAttribute : TypeFilterAttribute
    {
        public SuperAdminOnlyAttribute() : base(typeof(SuperAdminOnlyFilter)) { }
    }

    public class SuperAdminOnlyFilter : IAuthorizationFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SuperAdminOnlyFilter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Session’dan rolü oku
            var role = _httpContextAccessor.HttpContext.Session.GetString("AdminRole");

            if (role != "SuperAdmin")
            {
                // Rol yok veya SuperAdmin değil → 403 sayfasına yönlendir
                context.Result = new RedirectToActionResult("Error403", "Error", new { area = "" });
            }
        }
    }
}