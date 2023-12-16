using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions.Services
{
    public interface IRoleService
    {
        (object, int) GetAllRoles(int page, int size);
        Task<(string id, string name)> GetRoleById(string id);
        Task<Boolean> CreateRole(string name);
        Task<Boolean> DeleteRole(string id);
        Task<Boolean> UpdateRole(string id, string name);
    }
}
