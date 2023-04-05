using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities.System;
using Web.ViewModel.Common;

namespace Web.Service.System
{
    public class RoleService:IRoleService
    {
        private readonly RoleManager<WebRole> _roleManager;
        public RoleService(RoleManager<WebRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Tạo mới role
        /// </summary>
        /// <param name="roleName">tên role</param>
        /// <returns>ServiceResult</returns>
        public async Task<ServiceResult> InsertRole(string roleName)
        {
            ServiceResult serviceResult = new ServiceResult();
            var isRoleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!isRoleExist)
            {
                IdentityRole role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(new WebRole
                {
                    Name = role.Name,
                    NormalizedName = role.NormalizedName,
                    ConcurrencyStamp = role.ConcurrencyStamp
                });
                if (!result.Succeeded)
                {
                    serviceResult.OnError(0, Resource.Resource.RoleInsertError);
                    return serviceResult;
                }
                serviceResult.OnSuccess(1, Resource.Resource.RoleInsertSuccess);
                return serviceResult;
            }
            
            serviceResult.OnError(0,Resource.Resource.RoleExist);
            return serviceResult;
        }

        //public async Task<ServiceResult> UpdateRole(string roleName)

        /// <summary>
        /// Lấy tất cả role
        /// </summary>
        /// <returns>list role</returns>
        public async Task<List<WebRole>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
