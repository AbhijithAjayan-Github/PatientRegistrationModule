using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PatientRegistrationModule.DTOs.Requests;
using PatientRegistrationModule.DTOs.Responses;
using PatientRegistrationModule.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PatientRegistrationModule.Controllers
{
    [Route("api/patient")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService patientService;
        private readonly ILogger<PatientController> logger;
        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            this.patientService = patientService;
            this.logger = logger;
        }

        [HttpPost]
        [Route("send-otp")]
        public async Task<IActionResult> SendOTP(SendOtpRequest request)
        {
            SendOtpResponse response = new SendOtpResponse();
            response = await patientService.SendOTP(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("verify-otp")]
        public async Task<IActionResult>VerifyOTP(VerifyOtpRequest request)
        {
            VerifyOtpResponse response = new VerifyOtpResponse();
            response = await patientService.VerifyOTP(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult>Register(PatientRegisterationRequest patient)
        {
            if (patient.DateOfBirth > DateOnly.FromDateTime(DateTime.Now)) throw new ValidationException("Date of birth should be in the past");
            PatientRegisterationResponse response = new PatientRegisterationResponse();
            return Ok(response);
        }
    }
}
