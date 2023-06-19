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

namespace Mentore.Services
{
    public class AdminService : BaseService, IAdminService
    {
        private readonly ICredentialRepository _credentialRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IUserRepository _userRepo;
        private readonly Encryptor _encryptor;

        public AdminService(ICredentialRepository credentialRepo, IMapperCustom mapper
            , IUnitOfWork unitOfWork, IRoleRepository roleRepo
            , IUserRepository userRepo, Encryptor encryptor) : base(unitOfWork, mapper)
        {
            _credentialRepo = credentialRepo;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
            _encryptor = encryptor;
        }

       /* public async Task<bool> ManageTransaction(TransactionDTO transactionDto, string userGroupId)
        {
            // Find account admin
            await _unitOfWork.BeginTransaction();
            var admin = await _userRepo.FindAsync(us => us.UserGroupId == 1);
            var customerMoney = 0;
            var adminMoney = 0;
            var shopMoney = 0;

            if (userGroupId == 1)
            {
                // Add money to account admin
                adminMoney = admin.Wallet.HasValue == false ? 0 : admin.Wallet.Value;
                adminMoney += transactionDto.Money;
                admin.Wallet = adminMoney;

                // Save to history transaction that order is prepared
                var history = new HistoryTransaction
                {
                    BillId = transactionDto.BillId,
                    CustomerId = transactionDto.CustomerId,
                    ShopId = transactionDto.ShopId,
                    Money = transactionDto.Money,
                    TransactionDate = DateTime.Now,
                    StatusId = 1,
                };
                await _historyTransactionRepo.AddAsync(history);
                 
            }

            if (userGroupId == 2)
            {
                // Find account customer
                var customer = await _userRepo.FindAsync(us => us.Id == transactionDto.CustomerId);
                adminMoney = admin.Wallet.HasValue == false ? 0 : admin.Wallet.Value;
                customerMoney = customer.Wallet.HasValue == false ? 0 : customer.Wallet.Value;

                customerMoney += transactionDto.Money;
                adminMoney -= transactionDto.Money;

                customer.Wallet = customerMoney;
                admin.Wallet = adminMoney;

                // Save to history transaction that order canceled
                // Find the history transaction of that order
                var history = await _historyTransactionRepo.FindAsync(h => h.BillId == transactionDto.BillId);
                history.StatusId = 4;

               *//* var history = new HistoryTransaction
                {
                    BillId = transactionDto.BillId,
                    CustomerId = transactionDto.CustomerId,
                    ShopId = transactionDto.ShopId,
                    Money = transactionDto.Money,
                    TransactionDate = DateTime.Now,
                    StatusId = 4,
                };
                await _historyTransactionRepo.AddAsync(history);*//*
            }

            if (userGroupId == 3)
            {
                // Find shop account and wallet
                var shop = await _shopRepo.FindAsync(shop => shop.Id == transactionDto.ShopId);
                adminMoney = admin.Wallet.HasValue == false ? 0 : admin.Wallet.Value;
                shopMoney = shop.ShopWallet.HasValue == false ? 0 : shop.ShopWallet.Value;

                shopMoney += transactionDto.Money;
                adminMoney -= transactionDto.Money;

                shop.ShopWallet = shopMoney;
                admin.Wallet = adminMoney;

                // Save to history transaction that order completed
                var history = await _historyTransactionRepo.FindAsync(h => h.BillId == transactionDto.BillId);
                history.StatusId = 3;

              *//*  var history = new HistoryTransaction
                {
                    BillId = transactionDto.BillId,
                    CustomerId = transactionDto.CustomerId,
                    ShopId = transactionDto.ShopId,
                    Money = transactionDto.Money,
                    TransactionDate = DateTime.Now,
                    StatusId = 3,
                };
                await _historyTransactionRepo.AddAsync(history);*//*
            }
            _userRepo.Update(admin);
            await _unitOfWork.CommitTransaction();
            return true;
        }*/

        public async Task<List<CredentialResponse>> GetRolesOfUserGroup(string userGroup)
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
        }

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

        public async Task<bool> UpdateUserGroupOfUser(UserGroupUpdatedRequest request)
        {
            var user = await _userRepo.FindAsync(u => u.Id == request.UserId);
            user.UserGroupId = request.UserGroupId;
            await _unitOfWork.CommitTransaction();
            return true;
        }
    }
}