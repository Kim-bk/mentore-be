using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Mentore.Models;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses;
using Mentore.Models.DTOs.Responses.Base;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using Model.DTOs.Requests;

namespace Mentore.Services
{
    public class PermissionService : BaseService, IPermissionService
    {
        private readonly ICredentialRepository _credentialRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserGroupRepository _userGroupRepo;
        private readonly IUserRepository _userRepo;

        public PermissionService(IUnitOfWork unitOfWork, IMapperCustom mapper
                , ICredentialRepository credentialRepository, IRoleRepository roleRepository
                , IUserGroupRepository userGroupRepository, IUserRepository userRepo) : base(unitOfWork, mapper)
        {
            _credentialRepo = credentialRepository;
            _roleRepo = roleRepository;
            _userGroupRepo = userGroupRepository;
            _userRepo = userRepo;
        }

        public async Task<string> GetCredentials(string userId)
        {
            // 1. Get User Group Id of user
            var groupUserId = (await _userRepo.FindAsync(us => us.Id == userId)).UserGroupId;

            // 2. Get credentials of user
            List<string> listCredentials = await _credentialRepo.GetCredentialsByUserGroupId(groupUserId);
            string combinedString = string.Join(",", listCredentials.ToArray());
            return combinedString;
        }

        public async Task<GeneralResponse> AddCredential(CredentialRequest req)
        {
            try
            {
                // 1. Check duplicate
                var existCredential = await _credentialRepo.FindAsync
                                (c => c.RoleId == req.RoleId && c.UserGroupId == req.UserGroupId);
                if (existCredential != null && existCredential.IsActivated == false)
                {
                    existCredential.IsActivated = true;
                    return new GeneralResponse
                    {
                        IsSuccess = true,
                    };
                }

                // 2. Find role
                var role = await _roleRepo.FindAsync(r => r.Id == req.RoleId && r.IsDeleted == false);

                // 3. Find user group
                var userGroup = await _userGroupRepo.FindAsync(us => us.Id == req.UserGroupId && us.IsDeleted == false);

                // 4. Add new Credential
                var newCredential = new Credential
                {
                    Role = role,
                    UserGroup = userGroup,
                    IsActivated = true,
                };
                await _credentialRepo.AddAsync(newCredential);
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true
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

        public async Task<GeneralResponse> RemoveCredential(CredentialRequest req)
        {
            try
            {
                // 1. Check credential
                var existedCredential = await _credentialRepo.FindAsync
                    (c => c.RoleId == req.RoleId && c.UserGroupId == req.UserGroupId && c.IsActivated == true);
                if (existedCredential == null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Đầu vào không hợp lệ !"
                    };
                }

                // 2. Else delete that credential
                existedCredential.IsActivated = false;
                await _unitOfWork.CommitTransaction();
                return new GeneralResponse
                {
                    IsSuccess = true
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

        public async Task<bool> UpdateCredential(PermissionRequest req)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                foreach (var role in req.Roles)
                {
                    var credential = await _credentialRepo.FindAsync(c => c.UserGroupId == req.UserGroupId
                                                                      && c.RoleId == role.RoleId);
                    if (credential != null)
                    {
                        credential.IsActivated = role.IsActivated;
                    }
                    else
                    {
                        /*  var findUserGroup = await _userGroupRepo.FindAsync(ug => ug.Id == req.UserGroupId);
                          var findRole = await _roleRepo.FindAsync(r => r.Id == role.Id);*/
                        var newCredential = new Credential
                        {
                            UserGroupId = req.UserGroupId,
                            RoleId = role.RoleId,
                            IsActivated = role.IsActivated,
                        };
                        await _credentialRepo.AddAsync(newCredential);
                    }
                }

                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch
            {
                throw;
            }
        }
    }
}