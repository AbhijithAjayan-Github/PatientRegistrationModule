using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.DTOs.Requests
{
    public class VerifyOtpRequest
    {
        [Required]
        [Phone]
        public string Mobile {  get; set; }
        [Required]
        public string OTP { get; set; }
    }
}
