using PatientRegistrationModule.Enums;
using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.Models
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string? PatientCode { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string? Email { get; set; }
        public Gender Gender { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
