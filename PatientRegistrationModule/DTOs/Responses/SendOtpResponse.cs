namespace PatientRegistrationModule.DTOs.Responses
{
    public class SendOtpResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string OTP {  get; set; }
        public string Mobile { get; set; }
    }
}
