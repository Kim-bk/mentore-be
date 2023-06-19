using Mentore.Models.DAL;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DTOs.Requests;
using Mentore.Models.DTOs.Responses.Base;
using Mentore.Services.Base;
using System.Threading.Tasks;
using System;
using Mentore.Models;
using Mentore.Models.DTOs.Responses;
using Mentore.Services.Interfaces;

namespace Mentore.Services
{
    public class UserGroupService : BaseService, IUserGroupService
    {
        private readonly IUserGroupRepository _userGroupRepo;
        public UserGroupService(IUserGroupRepository userGroupRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _userGroupRepo = userGroupRepository;
        }

        public async Task<GeneralResponse> UpdateUserGroup(UserGroupRequest req)
        {
            try
            {
                // 1. Find User Group
                var userGroup = await _userGroupRepo.FindAsync(ug => ug.Id == req.UserGroupId);
                if (userGroup == null)
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy tên quyền " + req.Name + " !"
                    };
                }

                // 2. Update that User Group
                userGroup.Name = req.Name;
                _userGroupRepo.Update(userGroup);
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
        public async Task<GeneralResponse> AddUserGroup(string userGroupName)
        {
            try
            {
                // 1. Validate
                if (string.IsNullOrEmpty(userGroupName))
                {
                    return new GeneralResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cú pháp không hợp lệ !"
                    };
                }

                var userGroup = await _userGroupRepo.FindAsync(ug => ug.Name == userGroupName);
                if (userGroup != null && userGroup.IsDeleted == true)
                {
                    userGroup.IsDeleted = false;
                    return new GeneralResponse
                    {
                        IsSuccess = true,
                    };
                }

                var newUserGroup = new UserGroup
                {
                    Name = userGroupName,
                    IsDeleted = false,
                };
                await _userGroupRepo.AddAsync(newUserGroup);
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
                    ErrorMessage = e.Message,
                };
            }
        }

        public async Task<GeneralResponse> DeleteUserGroup(string userGroupId)
        {
            try
            {
                var userGroup = await _userGroupRepo.FindAsync(ug => ug.Id == userGroupId && ug.IsDeleted == false);
                userGroup.IsDeleted = true;

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

        public async Task<UserGroupResponse> GetUserGroups()
        {
            try
            {
                var rs = await _userGroupRepo.GetMainUserGroup();
                return new UserGroupResponse
                {
                    IsSuccess = true,
                    UserGroups = _mapper.MapUserGroups(rs)
                };

            }
            catch (Exception e)
            {
                return new UserGroupResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message
                };
            }
        }
    }
}
