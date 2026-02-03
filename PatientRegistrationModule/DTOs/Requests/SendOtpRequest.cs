using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.DTOs.Requests
{
    public class SendOtpRequest
    {
        [Required]
        [Phone]
        public string Mobile { get; set; }
    }
}
