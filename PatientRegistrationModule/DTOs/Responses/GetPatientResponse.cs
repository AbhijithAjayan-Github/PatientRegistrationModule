using PatientRegistrationModule.Models;

namespace PatientRegistrationModule.DTOs.Responses
{
    public class GetPatientResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Patient patient { get; set; }
    }
}
