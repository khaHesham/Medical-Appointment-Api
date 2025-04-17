using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.DTOs.Appointment;
using MedicalAppointmentApi.Exceptions;
using MedicalAppointmentApi.Extentions;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPatientService _patientService;

        private readonly IAppointmentService _appointmentService;

        public PatientController(IUnitOfWork unitOfWork, IPatientService patientService, IAppointmentService appointmentService)
        {
            _unitOfWork = unitOfWork;
            _patientService = patientService;
            _appointmentService = appointmentService;
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("me")]
        public async Task<IActionResult> GetPatientById()
        {
            var patientId = User.GetUserId();
            var patient = await _patientService.GetPatientByIdAsync(patientId);
            return Ok(patient);

        }

        [Authorize(Roles = "Patient")]
        [HttpPatch("me")]
        public async Task<IActionResult> UpdatePatient([FromBody] UpdatePatientRequest updatePatientRequest)
        {
            var patientId = User.GetUserId();
            var result = await _patientService.UpdatePatientAsync(patientId, updatePatientRequest);
            return Ok(result);
        }

        [HttpPost("appointments")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ScheduleAppointment([FromBody] ScheduleAppointmentRequest request)
        {
            var patientId = User.GetUserId();
            var result = await _appointmentService.ScheduleAsync(patientId, request);
            return Ok(result);
        }

        [HttpGet("appointments")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var patientId = User.GetUserId();
            var result = await _appointmentService.GetPatientAppointmentsAsync(patientId, page, pageSize);
            return Ok(result);
        }
    }
}