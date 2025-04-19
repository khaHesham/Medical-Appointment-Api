using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using MedicalAppointmentApi.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentApi.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(IAuthService authService, IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized();
            }
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register/patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] RegisterRequest request)
        {
            var role = Role.Patient;
            var result = await _authService.RegisterAsync(request, role);
            if (result == null)
            {
                return BadRequest();
            }
            return StatusCode(201, result);
        }

        [AllowAnonymous]
        [HttpPost("register/doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterRequest request)
        {
            var role = Role.Doctor;
            var result = await _authService.RegisterAsync(request, role);
            if (result == null)
            {
                return BadRequest();
            }
            return StatusCode(201, result);
        }
    }

}