using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointment.API.Models;
using MedicalAppointmentApi.DTOs;

namespace MedicalAppointmentApi.Interfaces
{
    public interface IPatientService
    {
        Task<PatientDTO> GetPatientByIdAsync(int id);
        Task<PatientDTO> UpdatePatientAsync(int patientId,UpdatePatientRequest updatePatientRequest);

    }
}