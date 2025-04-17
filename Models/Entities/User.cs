
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentApi.Models.Entities
{
    [Table("Users")]
    [Index(nameof(Email), IsUnique = true)]
    [PrimaryKey(nameof(Id))]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public required Role UserRole { get; set; } // e.g., "Patient", "Doctor"
    }

    public enum Role
    {
        Patient,
        Doctor
    }

}