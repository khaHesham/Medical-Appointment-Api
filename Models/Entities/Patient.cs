using System.ComponentModel.DataAnnotations.Schema;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointment.API.Models
{
    [Table("Patients")]
    public class Patient : User
    {
        public DateTime? DateOfBirth { get; set; }

        // A patient can have multiple appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}