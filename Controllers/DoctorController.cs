using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.DTOs.User.Doctor;
using MedicalAppointmentApi.Extentions;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public DoctorController(IUnitOfWork unitOfWork, IDoctorService doctorService, IAppointmentService appointmentService)
        {
            _unitOfWork = unitOfWork;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorById()
        {
            var doctorId = User.GetUserId();
            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            return Ok(doctor);
        }

        [HttpPatch("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorRequest updateDoctorRequest)
        {
            var doctorId = User.GetUserId();
            var result = await _doctorService.UpdateDoctorAsync(doctorId, updateDoctorRequest);
            return Ok(result);
        }

        [HttpGet("appointments")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var doctorId = User.GetUserId();
            var result = await _appointmentService.GetDoctorAppointmentsAsync(doctorId, page, pageSize);
            return Ok(result);
        }


    }



}