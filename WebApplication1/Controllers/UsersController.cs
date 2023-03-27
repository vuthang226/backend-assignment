using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Service.System;
using Web.ViewModel.Common;
using Web.ViewModel.System;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWorkContext _workContext;

        public UsersController(IUserService userService, IWorkContext workContext)
        {
            _userService = userService;
            _workContext = workContext;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ServiceResult> Login([FromBody] LoginRequest request)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult = await _userService.Login(request);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }
            return serviceResult;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ServiceResult> GetUserById(Guid id)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                serviceResult = await _userService.GetUserById(id);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }
            return serviceResult;
        }

        [HttpGet("page")]
        [Authorize]
        public async Task<PagedResult<UserVm>> GetUsersPaging([FromQuery] GetUserPagingRequest request)
        {
            try
            {
                var page =  await _userService.GetUsersPaging(request);
                return page;
            }
            catch (Exception e)
            {
                return new PagedResult<UserVm>() {PageIndex = 1,PageSize=10,TotalRecords =0 };
            }
            
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ServiceResult> Delete(Guid id)
        {
            ServiceResult serviceResult = new ServiceResult();

            try
            {
                serviceResult = await _userService.DeleteUser(_workContext.GetUserId(),id);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }
            return serviceResult;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ServiceResult> Register([FromBody] RegisterRequest request)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult = await _userService.InsertUser(request);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }

            return serviceResult;
        }

        [HttpPost("assign")]
        [Authorize(Roles = "admin")]
        public async Task<ServiceResult> AssignAdmin([FromBody] UserAssign id)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult = await _userService.AssignAdmin(id.Id);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }

            return serviceResult;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ServiceResult> Update(Guid id, [FromBody] UserVm request)
        {
            ServiceResult serviceResult = new ServiceResult();
            Guid cur = _workContext.GetUserId();
            try
            {
                serviceResult = await _userService.UpdateUser(id,request);
            }
            catch (Exception e)
            {
                serviceResult.HandleException(e, "Có lỗi xảy ra");
            }
            return serviceResult;
        }



    }
}
