using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Data.Entities.System;
using Web.ViewModel.Common;

namespace Web.Service.System
{
    public interface IRoleService
    {
        Task<ServiceResult> InsertRole(string roleName);

        Task<List<WebRole>> GetAllRoles();
    }
}
