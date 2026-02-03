using PatientRegistrationModule.DTOs.Requests;
using PatientRegistrationModule.DTOs.Responses;
using PatientRegistrationModule.Models;

namespace PatientRegistrationModule.Services.Interfaces
{
    public interface IPatientService
    {
        Task<SendOtpResponse> SendOTP(SendOtpRequest request);
        Task<VerifyOtpResponse> VerifyOTP(VerifyOtpRequest request);
        Task<PatientRegisterationResponse>Register(PatientRegisterationRequest request);
        Task<string> GeneratePatientCode(int patientId);
        Task<GetPatientResponse> GetPatient(int patientId);
    }
}
