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
        
        public async Task<PatientRegisterationResponse> Register(PatientRegisterationRequest request)
        {
            PatientRegisterationResponse response = new PatientRegisterationResponse();
            var alreadyRegistered = await dbContext.Patients.FirstOrDefaultAsync(p => p.Mobile == request.Mobile);
            if (alreadyRegistered != null)
            {
                response.Sucess = true;
                response.Message = "Patient is already registered";
                response.PatientId = alreadyRegistered.PatientId;
                response.PatientCode = alreadyRegistered.PatientCode;
                return response;
            }
            string patientCode = await GeneratePatientCode();
            Patient patient = new Patient
            {
                PatientCode = patientCode,
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                Gender = request.Gender,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                State = request.State,
                City = request.City,
                PinCode = request.Pincode,
                CreatedDate = DateTime.Now,                
            };
            await dbContext.Patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();
            response.Sucess = true;
            response.Message = $"Successfully registered the patient {patient.FullName}";
            response.PatientId = patient.PatientId;
            response.PatientCode = patient.PatientCode;
            return response;
        }
        public async Task<string>GeneratePatientCode()
        {
            string patientCode = string.Empty;
            string prefix = $"PAT{DateTime.Now.Year}";
            var nextVal = await dbContext.Database.SqlQuery<int>($"SELECT NEXT VALUE FOR PatientCodeSeq").SingleAsync();
            patientCode = $"{prefix}{nextVal:D3}";
            return patientCode;
        }
    }
}
