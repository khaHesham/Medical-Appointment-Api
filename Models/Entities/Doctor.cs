using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAppointmentApi.Models.Entities
{
    [Table("Doctors")]
    public class Doctor : User
    {
        public string? Specialization { get; set; }

        // A doctor can have multiple appointments
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
    
}