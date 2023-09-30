using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Model.DAL.Interfaces;
using API.Model.DTOs;
using API.Model.Entities;
using API.Services.Interfaces;
using DAL.Entities;
using Mentore.Models.DAL;
using Mentore.Models.DTOs.Requests;
using Mentore.Services;
using Mentore.Services.Interfaces;
using Mentore.Services.TokenGenerators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mentore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IMentorService _mentorService;
        private readonly ILocationRepository _locationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMentorRepository _mentorRepo;
        private readonly IEntityFieldRepository _entityFieldRepository;
        private readonly IFieldRepository _fieldRepo;
        private readonly IRoleService _roleService;
        private readonly IMenteeService _menteeService;

        public UserController(IUserService userService
            , IAuthService authService
            , RefreshTokenGenerator refreshTokenGenerator
            , IMentorService mentorService
            , ILocationRepository locationRepo
            , IMentorRepository mentorRepo
            , IFieldRepository fieldRepo
            , IEntityFieldRepository entityFieldRepository
            , IRoleService roleService
            , IMenteeService menteeService
            , IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _authService = authService;
            _refreshTokenGenerator = refreshTokenGenerator;
            _mentorService = mentorService;
            _locationRepo = locationRepo;
            _fieldRepo = fieldRepo;
            _unitOfWork = unitOfWork;
            _entityFieldRepository = entityFieldRepository;
            _mentorRepo = mentorRepo;
            _roleService = roleService;
            _menteeService = menteeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _userService.FindById(userId);
            if (rs.IsSuccess)
            {
                return Ok(rs.UserDTO);
            }

            return BadRequest(rs.ErrorMessage);
        }
        #region Import Data

        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Test");
        }

        [AllowAnonymous]
        [HttpGet("field")]
        public async Task<IActionResult> GetField()
        {
           List<string> options = new List<string>
            {
                "Ngân hàng",
                "Kế toán",
                "Kiểm toán",
                "Bất động sản",
                "Chứng khoán",
                "Nhân sự",
                "Marketing",
                "Nghiên cứu kinh tế",
                "Truyền thông",
                "Thương mại quốc tế",
                "Xuất nhập khẩu",
                "Tài chính",
                "Sales",
                "Công nghệ thông tin (CNTT)",
                "Kiến trúc",
                "Xây dựng",
                "Khác",
                "Logistic",
                "Supply Chain",
                "Operation",
                "Management Consulting",
                "Business Analyst",
                "Data Analyst",
                "Data Science",
                "Data Engineer",
                "Giáo dục",
                "Quản lý dự án",
                "Học tập",
                "Phát triển kỹ năng mềm",
                "Các hoạt động xã hội (CLB/ tình nguyện...)",
                "Exchange/trao đổi sinh viên",
                "Học chuyển tiếp",
                "Du học (Đại học, thạc sĩ...)",
                "Management Trainee",
                "Ứng tuyển vào Big4",
                "Khởi nghiệp"
            };

            foreach (var opt in options)
            {
                var field = new Field
                {
                    Type = opt
                };

                await _fieldRepo.AddAsync(field);
                await _unitOfWork.CommitTransaction();

            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("location")]
        public async Task<IActionResult> ImportLocation()
        {
            string[] provinces = {
            "Hà Nội", "Hồ Chí Minh", "Hải Phòng", "Đà Nẵng", "Cần Thơ", "An Giang", "Bà Rịa - Vũng Tàu",
            "Bắc Giang", "Bắc Kạn", "Bạc Liêu", "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước",
            "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp",
            "Gia Lai", "Hà Giang", "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình", "Hưng Yên",
            "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An",
            "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Quảng Bình", "Quảng Nam", "Quảng Ngãi",
            "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa",
            "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái", "Phú Yên"
          };

            foreach (var opt in provinces)
            {
                var field = new Location
                {
                    Name = opt
                };

                await _locationRepo.AddAsync(field);
                await _unitOfWork.CommitTransaction();

            }

            return Ok();
        }


        [HttpGet("import")]
        public async Task<List<Mentor>> ImportMentorData()
        {

            using (StreamReader r = new("mentor_data.json"))
            {
                Random random = new Random();
                string json = r.ReadToEnd();
                List<MentorData> mentorData = JsonConvert.DeserializeObject<List<MentorData>>(json);
                List<Mentor> mentors = new();
                List<Field> fields = await _fieldRepo.GetAll();

                foreach (var data in mentorData)
                {
                    var index = random.Next(1, 36);
                    var field = fields[index];
                    // mentor already exists
                    try
                    {
                        var findMentor = await _mentorRepo.FindAsync(_ => _.Name.Equals(data.Name));

                        while (true)
                        {
                            var existedMentorField = await _entityFieldRepository.FindAsync
                                (_ => _.TableId == findMentor.Id
                                && _.TableName == "Mentor"
                                && _.FieldTypeId == field.Id);

                            if (existedMentorField == null)
                                break;

                            index = random.Next(1, 36);
                            field = fields[index];
                        }

                        var mentorField = new EntityField
                        {
                            FieldTypeId = field.Id,
                            TableName = "Mentor",
                            TableId = findMentor.Id,
                        };

                        await _entityFieldRepository.AddAsync(mentorField);
                        await _unitOfWork.CommitTransaction();
                    }
                    catch
                    {
                        var location = await _locationRepo.FindAsync(_ => _.Name == GetRandomProvince());
                        var mentor = new Mentor
                        {
                            Name = data.Name,
                            Avatar = data.Avatar,
                            Description = data.Description,
                            CurrentJob = data.Job,
                            LocationId = (await _locationRepo.FindAsync(_ => _.Name == GetRandomProvince())).Id,
                            PhoneNumber = GetRandomPhoneNumber(),
                            BirthDate = RandomBirthDate(),
                            Email = RandomEmail(),
                        };

                        var mentorField = new EntityField
                        {
                            FieldTypeId = field.Id,
                            TableName = "Mentor",
                            TableId = mentor.Id,
                        };


                        await _entityFieldRepository.AddAsync(mentorField);
                        await _unitOfWork.CommitTransaction();

                        mentors.Add(mentor);
                    }
                }

                return await _mentorService.ImportData(mentors);
            }
        }

        private DateTime RandomBirthDate()
        {
            Random random = new Random();
            int year = random.Next(1980, 1996);  // Generate a random year between 1980 and 1995
            int month = random.Next(1, 13);      // Generate a random month (1-12)
            int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1); // Generate a random day

            return new DateTime(year, month, day);
        }

        private string RandomEmail()
        {
            Random random = new Random();

            string[] domains = { "gmail.com", "yahoo.com", "hotmail.com", "outlook.com" }; // Add more domains as needed

            return GenerateRandomString(10) + "@" + domains[random.Next(domains.Length)];
        }

        private string GenerateRandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            char[] randomChars = new char[length];

            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                randomChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomChars);
        }

        private string GetRandomProvince()
        {
            string[] provinces = {
            "Hà Nội", "Hồ Chí Minh", "Hải Phòng", "Đà Nẵng", "Cần Thơ", "An Giang", "Bà Rịa - Vũng Tàu",
            "Bắc Giang", "Bắc Kạn", "Bạc Liêu", "Bắc Ninh", "Bến Tre", "Bình Định", "Bình Dương", "Bình Phước",
            "Bình Thuận", "Cà Mau", "Cao Bằng", "Đắk Lắk", "Đắk Nông", "Điện Biên", "Đồng Nai", "Đồng Tháp",
            "Gia Lai", "Hà Giang", "Hà Nam", "Hà Tĩnh", "Hải Dương", "Hậu Giang", "Hòa Bình", "Hưng Yên",
            "Khánh Hòa", "Kiên Giang", "Kon Tum", "Lai Châu", "Lâm Đồng", "Lạng Sơn", "Lào Cai", "Long An",
            "Nam Định", "Nghệ An", "Ninh Bình", "Ninh Thuận", "Phú Thọ", "Quảng Bình", "Quảng Nam", "Quảng Ngãi",
            "Quảng Ninh", "Quảng Trị", "Sóc Trăng", "Sơn La", "Tây Ninh", "Thái Bình", "Thái Nguyên", "Thanh Hóa",
            "Thừa Thiên Huế", "Tiền Giang", "Trà Vinh", "Tuyên Quang", "Vĩnh Long", "Vĩnh Phúc", "Yên Bái", "Phú Yên"
          };

            // Get a random province
            Random random = new Random();
            return provinces[random.Next(provinces.Length)];
        }

        private string GetRandomPhoneNumber()
        {
            Random random = new Random();

            // Generate a random phone number
            string phoneNumber = "0" + random.Next(1, 10).ToString();

            for (int i = 0; i < 9; i++)
            {
                phoneNumber += random.Next(0, 10).ToString();
            }

            return phoneNumber;
        }
        #endregion

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var rs = await _userService.Login(request);
                if (rs.IsSuccess)
                {
                    // 1. Get list credentials of user
                    var listCredentials = await _roleService.GetCredentials(rs.User.Id);

                    // 2. Authenticate user
                    var res = await _authService.Authenticate(rs.User, listCredentials);
                    if (res.IsSuccess)
                        return Ok(res);
                    else
                        return BadRequest(res.ErrorMessage);
                }
                return BadRequest(rs.ErrorMessage);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            _ = await _userService.Logout(userId);
            return Ok("Đăng xuất thành công !");
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshRequest)
        {
            try
            {
                var rs = await _refreshTokenGenerator.Refresh(refreshRequest.Token);
                if (rs.IsSuccess)
                {
                    // 1. Get list credentials of user
                    var listCredentials = await _roleService.GetCredentials(rs.User.Id);

                    // 2. Authenticate user
                    var responseTokens = await _authService.Authenticate(rs.User, listCredentials);
                    return Ok(responseTokens);
                }

                return BadRequest(rs.ErrorMessage);
            }
            catch (Exception e)
            {
                return BadRequest(e.ToString());
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistRequest request)
        {
            var rs = await _userService.Register(request);

            if (rs.IsSuccess)
                return Ok("Vui lòng vào Email kiểm tra tin nhắn !");

            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("verify-account")]
        public async Task<IActionResult> VerifyAccount([FromQuery] string code)
        {
            var rs = await _userService.CheckUserByActivationCode(new Guid(code));
            if (rs)
                return Redirect("http://localhost:8080/success-page");


            return BadRequest("Xác thực thất bại !");
        }

        [Authorize]
        [HttpGet("mentee")]
        public async Task<MenteeDTO> GetMenteeData()
        {
            string userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _menteeService.GetMenteeData(userId);
        }

        [Authorize]
        [HttpGet("mentor")]
        public async Task<MentorDTO> GetMentorData()
        {
            string userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            return await _mentorService.GetMentorByAccountId(userId);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var rs = await _userService.ForgotPassword(request.Email);
            if (rs.IsSuccess)
            {
                return Ok("Kiểm tra Email của bạn để thay đổi mật khẩu !");
            }
            return BadRequest(rs.ErrorMessage);
        }

        #region Reset Password

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var rs = await _userService.ResetPassword(request);
            if (rs.IsSuccess)
            {
                return Ok("Bạn đã thay đổi mật khẩu thành công !");
            }
            return BadRequest(rs.ErrorMessage);
        }

        [HttpGet("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery] string code)
        {
            var rs = await _userService.GetUserByResetCode(new Guid(code));
            if (rs)
            {
                // Redirect sang trang cập nhật mật khẩu, gửi kèm theo code
                return Redirect("https://2clothy.vercel.app/resetpassword?code=" + code);
            }
            return BadRequest("Không tìm thấy tài khoản tương ứng !");
        }

        #endregion Reset Password

        [Authorize]
        [HttpPut]
        public async Task<bool> UpdateAccount([FromForm] UserRequest request)
        {
            var userId = Convert.ToString(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var rs = await _userService.UpdateUser(request, userId);
            if (rs.IsSuccess)
                return true;
            
            return false;
        }
    }
}