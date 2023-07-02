using System;
using System.Threading.Tasks;
using AutoMapper;
using Mentore.Models.DTOs.Requests;
using Mentore.Models;
using Mentore.Models.DAL;
using Mentore.Models.DAL.Interfaces;
using Mentore.Models.DAL.Repositories;
using Mentore.Models.DTOs;
using Mentore.Models.DTOs.Responses;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using API.Model.DAL.Interfaces;
using API.Model.Entities;
using API.Services;

namespace Mentore.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IMentorRepository _mentorRepo;
        private readonly IMenteeRepository _menteeRepo;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly Encryptor _encryptor;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _map;
        private readonly UploadImageService _uploadImageService;

        public UserService(IUserRepository userRepo, IUnitOfWork unitOfWork, Encryptor encryptor
            , IEmailSender emailSender, IMapperCustom mapper
            , IRefreshTokenRepository refreshTokenRepossitory, IMapper map
            , UploadImageService uploadImageService
            , IMenteeRepository menteeRepo
            , IMentorRepository mentorRepo) : base(unitOfWork, mapper)
        {
            _userRepo = userRepo;
            _encryptor = encryptor;
            _emailSender = emailSender;
            _refreshTokenRepo = refreshTokenRepossitory;
            _menteeRepo = menteeRepo;
            _mentorRepo = mentorRepo;
            _map = map;
            _uploadImageService = uploadImageService;
        }

        public async Task<UserResponse> FindById(string userId)
        {
            try
            {
                var user = await _userRepo.FindAsync(us => us.Id == userId);
                if (user.UserGroupId == "MENTEE")
                {
                    var menteeInfo = await _menteeRepo.FindAsync(_ => _.AccountId == userId);
                    var infoReturn = new UserDTO
                    {
                        Avatar = menteeInfo.Avatar,
                        Email = menteeInfo.Email,
                    };

                    return new UserResponse
                    {
                        IsSuccess = true,
                        UserDTO = infoReturn
                    };
                }
                else
                {
                    var mentorInfo = await _mentorRepo.FindAsync(_ => _.AccountId == userId);
                    var infoReturn = new UserDTO
                    {
                        Avatar = mentorInfo.Avatar,
                        Email = mentorInfo.Email,
                    };

                    return new UserResponse
                    {
                        IsSuccess = true,
                        UserDTO = infoReturn
                    };
                }
            }
            catch (Exception e)
            {
                return new UserResponse
                {
                    ErrorMessage = e.Message,
                    IsSuccess = false
                };
            }
        }

        public async Task<bool> Logout(string userId)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _refreshTokenRepo.DeleteAll(userId);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> CheckUserByActivationCode(Guid activationCode)
        {
            var user = await _userRepo.FindAsync(us => us.ActivationCode == activationCode);
            if (user == null)
                return false;

            user.IsActivated = true;
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<UserResponse> ForgotPassword(string userEmail)
        {
            try
            {
                // 1. Find user by email
                var user = await _userRepo.FindAsync(us => us.Email == userEmail && us.IsActivated == true);

                // 2. Check
                if (user == null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không thể tìm thấy Email được đăng ký !",
                    };
                }

                // 3. Generate reset password code to validate
                var resetCode = Guid.NewGuid();
                user.ResetPasswordCode = resetCode;

                // 3. Send email to user to reset password
                await _emailSender.SendEmailVerificationAsync(userEmail, resetCode.ToString(), "reset-password");
                await _unitOfWork.CommitTransaction();

                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }

        public async Task<bool> GetUserByResetCode(Guid resetPassCode)
        {
            return await _userRepo.FindAsync(us => us.ResetPasswordCode == resetPassCode) != null;
        }

        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find user by user name
            var user = await _userRepo.FindAsync(us => us.Email == req.Email && us.UserGroupId != "ADMIN");

            // 2. Check if user exist
            if (user == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không thể tìm thấy tài khoản !",
                };
            }

            // 3. Check if user is activated
            if (!user.IsActivated)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Vui lòng kiểm tra Email đã đăng ký để kích hoạt tài khoản !",
                };
            }

            // 4. Check if login password match
            if (_encryptor.MD5Hash(req.Password) != user.Password)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Sai mật khẩu hoặc tên đăng nhập !",
                };
            }

            return new UserResponse
            {
                User = user,
                IsSuccess = true
            };
        }

        public async Task<UserResponse> Register(RegistRequest req, bool isCreateMentor = false)
        {
            try
            {
                // 1. Check if duplicated account created
                var getUser = await _userRepo.FindAsync(us => us.Email == req.Email);

                if (getUser != null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Email đã được sử dụng !",
                    };
                }

                // 2. Check pass with confirm pass
                if (!String.Equals(req.Password, req.ConfirmPassWord))
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Mật khẩu xác nhận không khớp !",
                    };
                }

                await _unitOfWork.BeginTransaction();

                // 3. Create new account
                var user = new Account
                {
                    Email = req.Email,
                    IsActivated = false,
                    ActivationCode = Guid.NewGuid(),
                    DateCreated = DateTime.UtcNow.Date,
                    UserGroupId = isCreateMentor ? "MENTOR" : "MENTEE",

                    // 4. Encrypt password
                    Password = _encryptor.MD5Hash(req.Password),
                };

                // 5. Add user
                await _userRepo.AddAsync(user);

                // 6. Create mentee info
                if (!isCreateMentor)
                {
                    var mentee = new Mentee
                    {
                        Name = req.Name,
                        BirthDate = req.BirthDate,
                        Avatar = "https://ui-avatars.com/api/?name=" + req.Name.ToLower().Trim()
                    };

                    await _menteeRepo.AddAsync(mentee);
                }    

                await _unitOfWork.CommitTransaction();

                // 7. Send an email activation
                await _emailSender.SendEmailVerificationAsync(user.Email, user.ActivationCode.ToString(), "verify-account");

                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.ToString(),
                };
            }
        }

        public async Task<UserResponse> ResetPassword(ResetPasswordRequest req)
        {
            try
            {
                // 1. Find user by reset password code
                var user = await _userRepo.FindAsync(us => us.ResetPasswordCode == new Guid(req.ResetPasswordCode) && us.IsActivated == true);

                // 2. Check
                if (user == null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy tài khoản !",
                    };
                }

                user.Password = _encryptor.MD5Hash(req.NewPassword);
                user.ResetPasswordCode = new Guid();

                await _unitOfWork.CommitTransaction();

                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }

        public async Task<UserResponse> UpdateUser(UserRequest req, string idAccount)
        {
            try
            {
                var userReq = await _userRepo.FindAsync(it => it.Id == idAccount);

                if (userReq == null)
                {
                    return new UserResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy tài khoản !",
                    };
                }
                await _unitOfWork.BeginTransaction();

                // Mentor
                if (userReq.UserGroupId == "MENTOR")
                    UpdateMentorInfo(req, idAccount);

                if (userReq.UserGroupId == "MENTEE")
                    UpdateMenteeInfo(req, idAccount);

                await _unitOfWork.CommitTransaction();

                return new UserResponse
                {
                    IsSuccess = true,
                };
            }
            catch (Exception e)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = e.Message,
                };
            }
        }

        private async void UpdateMentorInfo(UserRequest model, string idAccount)
        {
            var mentor = await _mentorRepo.FindAsync(_ => _.AccountId == idAccount);

            mentor.Name = model.Name;
            mentor.PhoneNumber = model.PhoneNumber;
            mentor.CurrentJob = model.CurrentJob;
            mentor.BirthDate = model.BirthDate;
            mentor.Description = model.Description;
            if (model.Avatar != null)
                mentor.Avatar = _uploadImageService.UploadFile(model.Avatar);

            _mentorRepo.Update(mentor);
        }

        private async void UpdateMenteeInfo(UserRequest model, string idAccount)
        {
            var mentee = await _menteeRepo.FindAsync(_ => _.AccountId == idAccount);

            mentee.Name = model.Name;
            mentee.PhoneNumber = model.PhoneNumber;
            mentee.StudyAt = model.StudyAt;
            mentee.Address = model.Address;
            mentee.BirthDate = model.BirthDate;
            mentee.Description = model.Description;
            if (model.Avatar != null)
                mentee.Avatar = _uploadImageService.UploadFile(model.Avatar);

            _menteeRepo.Update(mentee);
        }     
    }
}