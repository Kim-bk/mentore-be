using Mentore.Models.DTOs.Responses.Base;
using Mentore.Models;
using System.Threading.Tasks;
using System;
using Mentore.Services.Interfaces;
using Mentore.Models.DAL.Interfaces;
using Mentore.Services.Base;
using Mentore.Models.DAL;
using Mentore.Models.DTOs.Requests;

namespace Mentore.Services
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IRoleRepository _roleRepo;
        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _roleRepo = roleRepository;
        }

        public async Task<GeneralResponse> CreateRole(string roleName)
        {
            try
            {
                // 1. Validate 
                if (String.IsNullOrEmpty(roleName))
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cú pháp không hợp lệ !"
                    };
                }

                var role = await _roleRepo.FindAsync(r => r.Name == roleName);
                if (role != null && role.IsDeleted == true)
                {
                    role.IsDeleted = false;
                    return new GeneralResponse
                    {
                        IsSuccess = true,
                    };
                }
                else
                {
                    var newRole = new Role { Name = roleName, IsDeleted = false };
                    await _roleRepo.AddAsync(newRole);
                    await _unitOfWork.CommitTransaction();
                    return new GeneralResponse
                    {
                        IsSuccess = true,
                    };
                }
              
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<GeneralResponse> DeleteRole(string roleId)
        {
            try
            {
                var role = await _roleRepo.FindAsync(r => r.Id == roleId && r.IsDeleted == false);
                role.IsDeleted = true;
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }

        public async Task<GeneralResponse> UpdateRole(RoleRequest req)
        {
            try
            {
                // 1. Find role by Id
                var role = await _roleRepo.FindAsync(r => r.Id == req.RoleId && r.IsDeleted == false);

                // 2. Check
                if (role == null)
                {
                    throw new ArgumentNullException("Can't find role !");
                }

                // 3. Update
                role.Name = req.Name;
                _roleRepo.Update(role);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
