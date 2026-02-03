using Azure;
using Microsoft.EntityFrameworkCore;
using PatientRegistrationModule.Data;
using PatientRegistrationModule.DTOs.Requests;
using PatientRegistrationModule.DTOs.Responses;
using PatientRegistrationModule.Models;
using PatientRegistrationModule.Services.Helpers;
using PatientRegistrationModule.Services.Interfaces;
using System.Text.Json;

namespace PatientRegistrationModule.Services
{
    public class PatientService : IPatientService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<PatientService> logger;
        private readonly ApplicationDbContext dbContext;
        private readonly TimeZoneHelper timeZoneHelper;
        public PatientService(ApplicationDbContext dbContext, IConfiguration configuration, ILogger<PatientService> logger,TimeZoneHelper timeZoneHelper)
        {
            this.dbContext = dbContext;
            this.configuration = configuration;
            this.timeZoneHelper = timeZoneHelper;
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
                logger.LogInformation($"Already verified patient : {JsonSerializer.Serialize(response)}");
                return response;
            }
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
            logger.LogInformation($"Sending OTP : {JsonSerializer.Serialize(response)}");
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
                logger.LogInformation($"Already verified patient : {JsonSerializer.Serialize(response)}");
                return response;
            }

            var otpRecord = await dbContext.OTPVerifications
                .FirstOrDefaultAsync(p => p.Mobile == request.Mobile && p.OTP == request.OTP);

            if (otpRecord == null)
            {
                response.Success = false;
                response.Mobile = request.Mobile;
                response.Message = "Invalid OTP.";
                logger.LogInformation($"Invalid OTP : {JsonSerializer.Serialize(response)}");
                return response;
            }

            if (DateTime.UtcNow >= otpRecord.ExpiryTime)
            {
                response.Success = false;
                response.Mobile = request.Mobile;
                response.Message = "OTP has expired.";
                logger.LogInformation($" Expired OTP : {JsonSerializer.Serialize(response)}");
                return response;
            }

            otpRecord.IsVerified = true;
            await dbContext.SaveChangesAsync();

            response.Success = true;
            response.Mobile = request.Mobile;
            response.Message = $"Successfully verified the mobile number {request.Mobile}";
            logger.LogInformation($"Verified OTP Successfully : {JsonSerializer.Serialize(response)}");
            return response;
        }
        
        public async Task<PatientRegisterationResponse> Register(PatientRegisterationRequest request)
        {
            PatientRegisterationResponse response = new PatientRegisterationResponse();
            var isOtpVerified = await dbContext.OTPVerifications
                                    .FirstOrDefaultAsync(p => p.Mobile == request.Mobile && p.IsVerified);

            if (isOtpVerified == null)
            {
                response.Sucess = false;
                response.Message = "Patient is not OTP verified, Please go through OTP Verification";
                logger.LogInformation($"Registeration Failed  : {JsonSerializer.Serialize(response)}");
                return response;
            }
            var alreadyRegistered = await dbContext.Patients.FirstOrDefaultAsync(p => p.Mobile == request.Mobile);
            if (alreadyRegistered != null)
            {
                response.Sucess = true;
                response.Message = "Patient is already registered";
                response.PatientId = alreadyRegistered.PatientId;
                response.PatientCode = alreadyRegistered.PatientCode;
                logger.LogInformation($"Already Registered Patient : {JsonSerializer.Serialize(response)}");
                return response;
            }
            Patient patient = new Patient
            {
                FullName = request.FullName,
                Email = request.Email,
                Mobile = request.Mobile,
                Gender = request.Gender,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                State = request.State,
                City = request.City,
                PinCode = request.Pincode,
                CreatedDate = DateTime.UtcNow ,                
            };
            await dbContext.Patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();
            patient.PatientCode = await GeneratePatientCode(patient.PatientId);
            await dbContext.SaveChangesAsync();
            response.Sucess = true;
            response.Message = $"Successfully registered the patient {patient.FullName}";
            response.PatientId = patient.PatientId;
            response.PatientCode = patient.PatientCode;
            logger.LogInformation($"Registered Patient : {JsonSerializer.Serialize(response)}");
            return response;
        }
        public async Task<string>GeneratePatientCode(int patientId)
        {
            string patientCode = string.Empty;
            string prefix = $"PAT{DateTime.Now.Year}";
            patientCode = $"{prefix}{patientId:D3}";
            logger.LogInformation($"Generated PatientCode : {patientCode}");
            return patientCode;
        }
        
        public async Task<GetPatientResponse> GetPatient(int patientId)
        {
            GetPatientResponse response = new GetPatientResponse();
            var patient = await dbContext.Patients.Where(p => p.PatientId == patientId).Select(p => new Patient
            {
                PatientId = p.PatientId,
                PatientCode = p.PatientCode,
                FullName = p.FullName,
                Email = p.Email,
                Mobile = p.Mobile,
                DateOfBirth = p.DateOfBirth,
                Address = p.Address,
                Gender = p.Gender,
                State = p.State,
                City = p.City,
                PinCode = p.PinCode,
                CreatedDate = timeZoneHelper.ConvertToLocalTime(p.CreatedDate),
                UpdatedDate = timeZoneHelper.ConvertToLocalTime(p.UpdatedDate)
            }).FirstOrDefaultAsync();
            if (patient == null) throw new KeyNotFoundException($"Patient Not found with id {patientId}");
            response.Success = true;
            response.Message = "Succesfully fetched Patient";
            response.patient = patient;
            logger.LogInformation($"Fetching Patient : {JsonSerializer.Serialize(patient)}");
            return response;
        }
    }
}
