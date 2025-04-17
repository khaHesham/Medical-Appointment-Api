using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointmentApi.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<bool?> RegisterAsync(RegisterRequest request, Role role);
    }
}