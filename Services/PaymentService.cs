using Mentore.Commons.VNPay;
using Mentore.Models.DAL;
using Mentore.Services.Base;
using Mentore.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using API.Model.DTOs.Requests;
using API.Model.Entities;
using API.Model.DAL.Interfaces;

namespace Mentore.Services
{
    public class PaymentService : BaseService, IPaymentService
    {
        private readonly VNPaySettings _VNPaySettings;
        private readonly IMenteeRepository _menteeRepo;
        private readonly IUserWorkshopRepository _userWorkshopRepo;
        private readonly IWorkshopRepository _workshopRepo;
        private readonly IEmailSender _emailSender;

        public PaymentService(IUnitOfWork unitOfWork
            , IMapperCustom mapper, IOptions<VNPaySettings> vnPay
            , IUserWorkshopRepository userWorkshopRepo
            , IWorkshopRepository workshopRepo
            , IMenteeRepository menteeRepo
            , IEmailSender emailSender) : base(unitOfWork, mapper)
        {
            _VNPaySettings = vnPay.Value;
            _userWorkshopRepo = userWorkshopRepo;
            _menteeRepo = menteeRepo;
            _workshopRepo = workshopRepo;
            _emailSender = emailSender;
        }

        #region VNPay
        public async Task<(string, bool)> VNPayCheckOut(WorkshopRequest request, string userId, HttpContext context)
        {
            // 1. Add User Workshop
            var mentee = await _menteeRepo.FindAsync(_ => _.AccountId == userId);

            // 2. Check user workshop is already exists
            string userWorkshopId;
            var findUserWorkshop = await _userWorkshopRepo.FindAsync(
                _ => _.WorkshopId == request.Id
                && _.MenteeId == mentee.Id
                && !_.IsDeleted);

            if (findUserWorkshop == null)
            {
                var userWorkshop = new UserWorkshop
                {
                    MenteeId = mentee.Id,
                    WorkshopId = request.Id,
                    IsActived = false
                };

                userWorkshopId = userWorkshop.Id;
                await _userWorkshopRepo.AddAsync(userWorkshop);
            }
            else
            {
                if (findUserWorkshop.IsActived)
                    return ("Đã thanh toán!", false);

                userWorkshopId = findUserWorkshop.Id;
            }

            // Setting variables vnPay
            string vnp_ReturnUrl = _VNPaySettings.ReturnUrl; //URL nhan ket qua tra ve
            string vnp_Url = _VNPaySettings.Url; //URL thanh toan cua VNPAY
            string vnp_TmnCode = _VNPaySettings.TmnCode; //Ma website
            string vnp_HashSecret = _VNPaySettings.HashSecret; //Chuoi bi mat

            //Build URL for VNPAY
            VNPayLibrary vnpay = new();
            vnpay.AddRequestData("vnp_Version", VNPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (request.Price * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán Workshop: " + request.Title.ToUpper());
            vnpay.AddRequestData("vnp_OrderType", "string");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", userWorkshopId);

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            if (paymentUrl != "")
            {
                await _unitOfWork.CommitTransaction();
                return (paymentUrl, true);
            }

            await _unitOfWork.RollbackTransaction();
            return ("Error ! Vui lòng liên hệ IT để được hỗ trợ !", false);
        }
        #endregion VNPay
     
        public async Task<bool> PaySuccess(IQueryCollection collections)
        {
            var pay = new VNPayLibrary();
            var response = pay.GetFullResponseData(collections, _VNPaySettings.HashSecret);
            if (!response.IsSuccess)
                return false;

            var userWorkshop = await _userWorkshopRepo.FindAsync(_ => _.Id == response.UserWorkshopId);
            userWorkshop.IsActived = true;
            // Create invitation code for this payment
            userWorkshop.InvitationCode = GenerateRandomCode();
            _userWorkshopRepo.Update(userWorkshop);

            var workshop = await _workshopRepo.FindAsync(_ => _.Id == userWorkshop.WorkshopId);
            workshop.Participated += 1;
            _workshopRepo.Update(workshop);

            // Send email invitation
            // Find mentee email
            var mentee = await _menteeRepo.FindAsync(_ => _.Id == userWorkshop.MenteeId);
            await _emailSender.SendEmailPaySuccessAsync(mentee.Email, userWorkshop.InvitationCode, workshop.Title.ToUpper());

            await _unitOfWork.CommitTransaction();
            return true;
        }

        private static string GenerateRandomCode()
        {
            Random random = new();
            int randomNumber = random.Next(1000000, 9999999);
            return randomNumber.ToString();
        }    
    }
}