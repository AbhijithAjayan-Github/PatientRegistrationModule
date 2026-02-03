namespace PatientRegistrationModule.DTOs.Responses
{
    public class PatientRegisterationResponse
    {
        public bool Sucess { get; set; }
        public string? Message { get; set; }

        public int PatientId { get; set; }
        public string PatientCode { get; set; }
    }
}
