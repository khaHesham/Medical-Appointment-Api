using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.DTOs.User.Doctor;

namespace MedicalAppointmentApi.Interfaces
{
    public interface IDoctorService
    {
        Task<DoctorDTO> GetDoctorByIdAsync(int id);
        Task<DoctorDTO> UpdateDoctorAsync(int doctorId, UpdateDoctorRequest updateDoctorRequest);
    }
}