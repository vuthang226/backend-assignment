using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities.System;

namespace Web.Service.System
{
    public interface IWorkContext
    {
        Guid GetUserId();
        Task<string> GetUserName();
    }
    public class WorkContext : IWorkContext
    {
        private readonly HttpContext _httpContext;
        private readonly UserManager<WebUser> _userManager;
        public WorkContext(IHttpContextAccessor httpContextAccessor, UserManager<WebUser> userManager)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _userManager = userManager;
        }
        public Guid GetUserId()
        {
            var userId = _httpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (userId == null)
                return Guid.Empty;
            return Guid.Parse(userId);
        }

        public async Task<string> GetUserName()
        {
            var userId = _httpContext.User.FindFirstValue(ClaimTypes.Sid);
            if (userId == null)
                return string.Empty;
            var user = await _userManager.FindByIdAsync(userId);
            return user.UserName;
        }
    }
}
