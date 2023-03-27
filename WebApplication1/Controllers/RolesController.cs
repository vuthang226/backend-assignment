using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.Entities.System;
using Web.Service.System;
using Web.ViewModel.Common;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ServiceResult> InsertRole([FromBody] string roleName)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult = await _roleService.InsertRole(roleName);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }
            return serviceResult;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<WebRole>> GetAllRoles()
        {
            
            try
            {
                return  await _roleService.GetAllRoles();
            }
            catch (Exception e)
            {
                return new List<WebRole>();
            }
            
        }
    }
}
