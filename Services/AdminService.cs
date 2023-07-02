using Mentore.Models.DTOs.Requests;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Responses;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using Model.DTOs.Requests;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services.Interfaces;
using API.Model.DAL.Interfaces;
using DAL.Entities;

namespace Mentore.Services
{
    public class AdminService : BaseService, IAdminService
    {
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;
        private readonly Encryptor _encryptor;
        private readonly IPostService _postService;
        private readonly IPostRepository _postRepo;

        public AdminService(IMapperCustom mapper
            , IUnitOfWork unitOfWork, IRoleRepository roleRepo
            , IUserRepository userRepo, Encryptor encryptor
            , IPostService postService
            , IPostRepository postRepo) : base(unitOfWork, mapper)
        {
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _encryptor = encryptor;
            _postService = postService;
            _postRepo = postRepo;
        }

       /* public async Task<List<CredentialResponse>> GetRolesOfUserGroup(string userGroup)
        {
            var listCredentials = new List<CredentialResponse>();
            var rolesActivated = await _credentialRepo.GetRolesOfUserGroup(userGroup);
            var allRoles = await _roleRepo.GetAll();

            foreach (var role in rolesActivated)
            {
                var credential = new CredentialResponse
                {
                    RoleId = role.RoleId,
                    IsActivated = role.IsActivated,
                };
                listCredentials.Add(credential);
            }

            foreach (var role in allRoles)
            {
                if (listCredentials.FirstOrDefault(r => r.RoleId == role.Id) == null)
                {
                    var credential = new CredentialResponse
                    {
                        RoleId = role.Id,
                        IsActivated = false,
                    };
                    listCredentials.Add(credential);
                }
            }

            return listCredentials;
        }*/

        public async Task<List<UserDTO>> GetUsers()
        {
            var rs = await _userRepo.GetAccounts();
            return _mapper.MapUsers(rs);
        }

        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find admin account
            var admin = await _userRepo.FindAsync(us => us.Email == req.Email && (us.UserGroupId == "Admin"));

            // 2. Check if user exist
            if (admin == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không phải tài khoản Admin !",
                };
            }

            // 3. Check if login password match
            if (_encryptor.MD5Hash(req.Password) != admin.Password)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Sai mật khẩu hoặc tên đăng nhập !",
                };
            }

            return new UserResponse
            {
                User = admin,
                IsSuccess = true
            };
        }

        public async Task<List<Post>> GetPosts()
        {
            var posts = await _postRepo.GetAll();
            return posts.OrderBy(p => p.IsAccepted).ToList();
        }

        public async Task<List<Post>> AcceptPost(string postId)
        {
            var post = await _postRepo.FindAsync(_ => _.Id == postId && !_.IsDeleted);
            post.IsAccepted = true;
            _postRepo.Update(post);
            await _unitOfWork.CommitTransaction();
            return await GetPosts();
        }
    }
}