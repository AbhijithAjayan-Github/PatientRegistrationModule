using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.Models
{
    public class OTPVerification
    {
        [Key]
        public int Id { get; set; }
        public string Mobile { get; set; }
        public string OTP {  get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
