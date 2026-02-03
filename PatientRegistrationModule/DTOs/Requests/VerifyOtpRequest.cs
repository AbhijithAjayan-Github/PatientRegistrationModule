using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.DTOs.Requests
{
    public class VerifyOtpRequest
    {
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$",ErrorMessage ="Enter a valid phone number")]
        public string Mobile {  get; set; }
        [Required]
        [RegularExpression(@"^\d{6}$",ErrorMessage ="OTP should be 6 digits")]
        public string OTP { get; set; }
    }
}
