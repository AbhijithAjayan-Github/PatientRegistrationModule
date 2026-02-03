using Microsoft.EntityFrameworkCore;
using PatientRegistrationModule.Data;
using PatientRegistrationModule.DTOs.Requests;
using PatientRegistrationModule.DTOs.Responses;
using PatientRegistrationModule.Models;
using PatientRegistrationModule.Services.Interfaces;

namespace PatientRegistrationModule.Services
{
    public class PatientService : IPatientService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PatientService> logger;
        private readonly ApplicationDbContext dbContext;
        public PatientService(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<PatientService> logger)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<SendOtpResponse> SendOTP(SendOtpRequest request)
        {
            SendOtpResponse response = new SendOtpResponse();
            string otp = new Random().Next(100000, 999999).ToString();
            var alreadyVerified = await dbContext.OTPVerifications
                                .FirstOrDefaultAsync(p => p.Mobile == request.Mobile && p.IsVerified);

            if (alreadyVerified != null)
            {
                response.Success = true;
                response.Mobile = request.Mobile;
                response.Message = "Patient is already verified";
                return response;
            }
            var otpInfo = await dbContext.OTPVerifications.FirstOrDefaultAsync(info => info.Mobile == request.Mobile);
            OTPVerification otpDetails = new OTPVerification
            {
                Mobile = request.Mobile,
                OTP = otp,
                ExpiryTime = DateTime.UtcNow.AddMinutes(3),
                CreatedDate = DateTime.UtcNow,
                IsVerified = false
            };
            await dbContext.AddAsync(otpDetails);
            await dbContext.SaveChangesAsync();

            response.Success = true;
            response.Message = "Sucessfully Send OTP";
            response.Mobile = request.Mobile;
            response.OTP = otp;
            logger.LogInformation($"OTP of patient {request.Mobile} : {otp}");
            return response;
        }

        public async Task<VerifyOtpResponse> VerifyOTP(VerifyOtpRequest request)
        {
            var response = new VerifyOtpResponse();

            var alreadyVerified = await dbContext.OTPVerifications
                .FirstOrDefaultAsync(p => p.Mobile == request.Mobile && p.IsVerified);

            if (alreadyVerified != null)
            {
                response.Success = true;
                response.Mobile = request.Mobile;
                response.Message = "Patient is already verified";
                return response;
            }

            var otpRecord = await dbContext.OTPVerifications
                .FirstOrDefaultAsync(p => p.Mobile == request.Mobile && p.OTP == request.OTP);

            if (otpRecord == null)
            {
                response.Success = false;
                response.Mobile = request.Mobile;
                response.Message = "Invalid OTP.";
                return response;
            }

            if (DateTime.UtcNow >= otpRecord.ExpiryTime)
            {
                response.Success = false;
                response.Mobile = request.Mobile;
                response.Message = "OTP has expired.";
                return response;
            }

            otpRecord.IsVerified = true;
            await dbContext.SaveChangesAsync();

            response.Success = true;
            response.Mobile = request.Mobile;
            response.Message = $"Successfully verified the mobile number {request.Mobile}";
            return response;
        }

    }
}
