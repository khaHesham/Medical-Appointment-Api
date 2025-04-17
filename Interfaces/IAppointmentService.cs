using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.DTOs.Appointment;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointmentApi.Interfaces
{
    public interface IAppointmentService
    {
        Task<bool> ScheduleAsync(int patientId, ScheduleAppointmentRequest request);
        Task<IEnumerable<AppointmentDTO>> GetPatientAppointmentsAsync(int patientId, int page, int pageSize);
        Task<IEnumerable<AppointmentDTO>> GetDoctorAppointmentsAsync(int doctorId, int page, int pageSize);
        Task<bool> CancelAsync(int appointmentId, int userId, string role);
    }
}