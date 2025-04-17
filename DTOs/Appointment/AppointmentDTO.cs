using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointmentApi.DTOs.Appointment
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string? Reason { get; set; }
        public string? Status { get; set; }

        public string? DoctorName { get; set; }
        public string? PatientName { get; set; }
    }
}