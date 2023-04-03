using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities.System;
using Web.ViewModel.Common;
using Web.ViewModel.System;

namespace Web.Service.System
{
    public class UserService: IUserService
    {
        private readonly UserManager<WebUser> _userManager;
        private readonly SignInManager<WebUser> _signInManager;
        private readonly RoleManager<WebRole> _roleManager;
        private readonly AngularDbContext _context;
        private readonly IConfiguration _config;
        public UserService(UserManager<WebUser> userManager, SignInManager<WebUser> signInManager, RoleManager<WebRole> roleManager, IConfiguration config, AngularDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;

        }

        /// <summary>
        /// Tạo tài khoản mới
        /// </summary>
        /// <param name="request">thông tin tài khoản mới</param>
        /// <returns></returns>
        public async Task<ServiceResult> InsertUser(RegisterRequest request)
        {
            ServiceResult serviceResult = new ServiceResult();
            if(request.Password != request.ConfirmPassword)
            {
                serviceResult.OnError(0, Resource.UserInsertError);
                return serviceResult;
            }
            if(await _userManager.FindByNameAsync(request.UserName) != null)
            {
                serviceResult.OnError(0, Resource.UserNameExist);
                return serviceResult;
            }
            if(await _userManager.FindByEmailAsync(request.Email) != null)
            {
                serviceResult.OnError(0, Resource.UserMailExist);
                return serviceResult;
            }
            var user = new WebUser
            {
                Email = request.Email,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                FullName = request.FullName
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                serviceResult.OnError(0, Resource.UserInsertError);
                return serviceResult;
            }
            await _userManager.AddToRoleAsync(user, "reader");
            serviceResult.OnSuccess(1,Resource.UserInsertSuccess);
            return serviceResult;


        }
        /// <summary>
        /// ĐĂng nhập
        /// </summary>
        /// <param name="request">thông tin đăng nhập</param>
        /// <returns>token + thông tin user</returns>
        public async Task<ServiceResult> Login(LoginRequest request)
        {
            ServiceResult serviceResult = new ServiceResult();
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                serviceResult.OnError(0, Resource.UserNameNotFound);
                return serviceResult;
            }

            var result = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if (!result.Succeeded)
            {
                serviceResult.OnError(0, Resource.LoginError);
                return serviceResult;
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid,user.Id.ToString()),
                new Claim(ClaimTypes.Role, string.Join(";",roles)),
                new Claim(ClaimTypes.Name, request.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            var userVm = new UserVm()
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = roles[0]
            };
            serviceResult.OnSuccess(new
            {
                jwtToken,
                userVm
                
            }, Resource.LoginSuccess);
            return serviceResult;
        }
        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="idUser">id user</param>
        /// <returns></returns>
        public async Task<ServiceResult> DeleteUser(Guid currentUser,Guid idUser)
        {
            ServiceResult serviceResult = new ServiceResult();
            if(currentUser == idUser)
            {
                serviceResult.OnError(0, Resource.UserDeleteError);
                return serviceResult;
            }
            var user = await _userManager.FindByIdAsync(idUser.ToString());
            if(user == null)
            {
                serviceResult.OnError(0, Resource.UserNotFound);
                return serviceResult;
            }
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                serviceResult.OnError(0, Resource.UserDeleteError);
                return serviceResult;
            }
            serviceResult.OnSuccess(1, Resource.UserDeleteSuccess);
            return serviceResult;
        }
        /// <summary>
        /// Lấy thông tin 1 tài khoản
        /// </summary>
        /// <param name="idUser">id user</param>
        /// <returns>thông tin tk</returns>
        public async Task<ServiceResult> GetUserById(Guid idUser)
        {
            ServiceResult serviceResult = new ServiceResult();
            var user = await _userManager.FindByIdAsync(idUser.ToString());
            if(user == null) {
                serviceResult.OnError(0, Resource.UserNotFound);
                return serviceResult;
            }
            //Check is admin
            serviceResult.OnSuccess(new UserVm {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                Role = ""
            });
            return serviceResult;
        }
        /// <summary>
        /// cập nhật tài khoản
        /// </summary>
        /// <param name="idUser">id user</param>
        /// <param name="userInfo">thông tin cập nhật</param>
        /// <returns></returns>
        public async Task<ServiceResult> UpdateUser(Guid idUser, UserVm userInfo)
        {
            ServiceResult serviceResult = new ServiceResult();
            var user = await _userManager.FindByIdAsync(idUser.ToString());
            if (user == null)
            {
                serviceResult.OnError(0, Resource.UserNotFound);
                return serviceResult;
            }
            //Check mail exist
            if (await _userManager.Users.AnyAsync(x => x.Email == userInfo.Email && x.Id != idUser))
            {
                serviceResult.OnError(0, Resource.UserMailExist);
                return serviceResult;
            }
            //Update data
            user.Email = userInfo.Email;
            user.FullName = userInfo.FullName;
            user.PhoneNumber = userInfo.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                serviceResult.OnError(0, Resource.UserUpdateError);
                return serviceResult;
            }
            serviceResult.OnSuccess(1, Resource.UserUpdateSuccess);

            return serviceResult;
        }
        /// <summary>
        /// Lấy dữ liệu tài khoản theo trang
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PagedResult<UserVm>> GetUsersPaging(GetUserPagingRequest request)
        {
            ServiceResult serviceResult = new ServiceResult();
            //_userManager.Users;
            var query =  from u in _context.Users
                         join ur in _context.UserRoles on u.Id equals ur.UserId into t
                         from urole in t.DefaultIfEmpty()
                         join r in _context.Roles on urole.RoleId equals r.Id into k
                         from role in k.DefaultIfEmpty()
                         select new { u, role };
            //if (!string.IsNullOrEmpty(request.Keyword))
            //{
            //    query = query.Where(x => x.UserName.Contains(request.Keyword)
            //     || x.PhoneNumber.Contains(request.Keyword) || x.Email.Contains(request.Keyword));
            //}
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new UserVm()
                {
                    Role = x.role.Name,
                    Email = x.u.Email,
                    PhoneNumber = x.u.PhoneNumber,
                    UserName = x.u.UserName,
                    FullName = x.u.FullName,
                    Id = x.u.Id,
                }).ToListAsync();

            
            return new PagedResult<UserVm>()
            {
                Items = data,
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            }; 

        }
        /// <summary>
        /// Gán quyền admin tài khoản
        /// </summary>
        /// <param name="userId">id tài khoản</param>
        /// <returns></returns>
        public async Task<ServiceResult> AssignRole(UserAssign request,Guid currentId)
        {
            
            ServiceResult serviceResult = new ServiceResult();
            if (currentId == request.UserId)
            {
                serviceResult.OnError(0, Resource.UserNotAssignItSelf);
                return serviceResult;
            }
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            
            if (user == null)
            {
                serviceResult.OnError(0, Resource.UserNameNotFound);
                return serviceResult;
            }
            if (await _userManager.IsInRoleAsync(user, request.RoleName) == false)
            {
                //await _userManager.RemoveFromRolesAsync(user, new[] { "admin", "reader", "writer" });
                await _userManager.RemoveFromRoleAsync(user, "admin");
                await _userManager.RemoveFromRoleAsync(user,  "reader");
                await _userManager.RemoveFromRoleAsync(user, "writer");
                await _userManager.AddToRoleAsync(user, request.RoleName);
            }
            //await _userManager.UpdateAsync(user);
            serviceResult.OnSuccess(1, Resource.AssignAdmin);
            return serviceResult;

            
        }
    }
}
