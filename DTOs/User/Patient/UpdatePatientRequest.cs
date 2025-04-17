using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAppointmentApi.DTOs
{
    public class UpdatePatientRequest
    {
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public  string? FirstName { get; set; }
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public  string? LastName { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public  string? PhoneNumber { get; set; }
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? DateOfBirth { get; set; }
    }
}