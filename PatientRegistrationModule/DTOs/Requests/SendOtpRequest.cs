using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.DTOs.Requests
{
    public class SendOtpRequest
    {
        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d{10}$",ErrorMessage ="Enter a valid Phone number")]
        public string Mobile { get; set; }
    }
}
