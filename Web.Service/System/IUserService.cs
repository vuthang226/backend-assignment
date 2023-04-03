using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModel.Common;
using Web.ViewModel.System;

namespace Web.Service.System
{
    public interface IUserService
    {
        Task<ServiceResult> InsertUser(RegisterRequest request);

        Task<ServiceResult> Login(LoginRequest request);

        Task<ServiceResult> DeleteUser(Guid currentUser,Guid idUser);

        Task<ServiceResult> GetUserById(Guid idUser);

        Task<ServiceResult> UpdateUser(Guid idUser, UserVm userInfo);

        Task<PagedResult<UserVm>> GetUsersPaging(GetUserPagingRequest request);

        Task<ServiceResult> AssignRole(UserAssign request,Guid cur);

    }
}
