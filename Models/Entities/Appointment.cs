
using System.ComponentModel.DataAnnotations.Schema;

using MedicalAppointment.API.Models;

namespace MedicalAppointmentApi.Models.Entities
{
    public class Appointment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public required string Reason { get; set; }
        public AppointmentStatus Status { get; set; }

        // Navigation properties
        // These properties represent the relationships between Appointment and Patient/Doctor
        public Patient? Patient { get; set; }  // Nullable if the appointment is not yet assigned
        public required Doctor Doctor { get; set; }

        // Foreign keys for Doctor and Patient
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        [ForeignKey("Patient")]
        public int? PatientId { get; set; }
    }

    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        Cancelled
    }
}