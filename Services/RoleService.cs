using Mentore.Models.DTOs.Responses.Base;
using Mentore.Models;
using System.Threading.Tasks;
using Mentore.Services.Interfaces;
using Mentore.Models.DAL.Interfaces;
using Mentore.Services.Base;
using Mentore.Models.DAL;
using System.Collections.Generic;
using Mentore.Models.DAL.Repositories;
using System.Linq;

namespace Mentore.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;
        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper, IUserRepository userRepo) : base(unitOfWork, mapper)
        {
            _roleRepo = roleRepository;
            _userRepo = userRepo;
        }

        public async Task<GeneralResponse> CreateRole(string roleName)
        {
            var listRole = new List<Role>();
            var newRole = new Role 
            {
                Name = "CREATE_WORKSHOP", 
                IsDeleted = false,
                UserGroupId = "ADMIN"
            };
            listRole.Add(newRole);

            var newRole1 = new Role
            {
                Name = "UPDATE_WORKSHOP",
                IsDeleted = false,
                UserGroupId = "ADMIN"
            };
            listRole.Add(newRole1);

            var newRole2 = new Role
            {
                Name = "ACCEPT_POST",
                IsDeleted = false,
                UserGroupId = "ADMIN"
            };
            listRole.Add(newRole2);

            var newRole3 = new Role
            {
                Name = "DELETE_POST",
                IsDeleted = false,
                UserGroupId = "ADMIN"
            };
            listRole.Add(newRole3);

            var newRole4 = new Role
            {
                Name = "DELETE_WORKSHOP",
                IsDeleted = false,
                UserGroupId = "ADMIN"
            };
            listRole.Add(newRole4);


            await _roleRepo.AddRangeAsync(listRole);
            await _unitOfWork.CommitTransaction();
            return new GeneralResponse
            {
                IsSuccess = true,
            };
        }

        public async Task<string> GetCredentials(string userId)
        {
            // 1. Get User Group Id of user
            var groupUserId = (await _userRepo.FindAsync(us => us.Id == userId)).UserGroupId;

            // 2. Get credentials of user
            List<string> listCredentials = _roleRepo.GetQuery(_ =>_.UserGroupId == groupUserId).Select(_ => _.Name).ToList();
            return string.Join(",", listCredentials.ToArray());
        }
    }
}
