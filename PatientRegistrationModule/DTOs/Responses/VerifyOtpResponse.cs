namespace PatientRegistrationModule.DTOs.Responses
{
    public class VerifyOtpResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } 
        public string Mobile { get; set; }

    }
}
