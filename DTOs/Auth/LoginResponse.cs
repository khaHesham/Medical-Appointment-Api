using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicalAppointmentApi.DTOs
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string Role { get; set; }
    }
}