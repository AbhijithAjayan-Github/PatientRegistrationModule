using PatientRegistrationModule.DTOs.Requests;
using PatientRegistrationModule.DTOs.Responses;

namespace PatientRegistrationModule.Services.Interfaces
{
    public interface IPatientService
    {
        Task<SendOtpResponse> SendOTP(SendOtpRequest request);
        Task<VerifyOtpResponse> VerifyOTP(VerifyOtpRequest request);
    }
}
