using PatientRegistrationModule.Enums;
using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.DTOs.Requests
{
    public class PatientRegisterationRequest
    {
        [Required(ErrorMessage ="Full Name is required")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="Phone is required")]
        [RegularExpression(@"^\d{10}$")]
        public string Mobile {  get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [EnumDataType(typeof(Gender), ErrorMessage = "Gender must be Male, Female or Others")]
        public Gender Gender { get; set; }
        [Required]
        public DateOnly DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? City { get;set; }
        public string? State { get; set; }
        public string? Pincode {  get; set; }

    }
}
