using MedicalAppointment.API.Models;
using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.Exceptions;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointmentApi.Services
{
    public class PatientService : IPatientService
    {


        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public PatientService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public Task<PatientDTO> GetPatientByIdAsync(int id)
        {
            var patientRepo = _unitOfWork.Repository<Patient>();
            var patient = patientRepo.GetByIdAsync(id).Result;
            if (patient == null)
            {
                throw new NotFoundException($"Patient with ID {id} not found.");
            }

            // convert to DTO
            var patientDto = new PatientDTO();
            patientDto.Id = patient.Id;
            patientDto.FirstName = patient.FirstName;
            patientDto.LastName = patient.LastName;
            patientDto.PhoneNumber = patient.PhoneNumber;
            patientDto.Email = patient.Email;
            patientDto.DateOfBirth = patient.DateOfBirth;
            // return DTO
            return Task.FromResult(patientDto);
        }

        public async Task<PatientDTO> UpdatePatientAsync(int patientId, UpdatePatientRequest updatePatientRequest)
        {
            var patientRepo = _unitOfWork.Repository<Patient>();
            var patient =  patientRepo.GetByIdAsync(patientId).Result;

            if (patient == null)
                throw new NotFoundException($"Patient with ID {patientId} not found.");

            // update patient properties
            if (updatePatientRequest.FirstName != null)
                patient.FirstName = updatePatientRequest.FirstName;
            if (updatePatientRequest.LastName != null)
                patient.LastName = updatePatientRequest.LastName;
            if (updatePatientRequest.PhoneNumber != null)
                patient.PhoneNumber = updatePatientRequest.PhoneNumber;
            if (updatePatientRequest.DateOfBirth != null)
                patient.DateOfBirth = updatePatientRequest.DateOfBirth;

            // save changes
            await _unitOfWork.CompleteAsync();

            // convert to DTO
            var patientDto = new PatientDTO();
            patientDto.Id = patient.Id;
            patientDto.FirstName = patient.FirstName;
            patientDto.LastName = patient.LastName;
            patientDto.PhoneNumber = patient.PhoneNumber;
            patientDto.Email = patient.Email;
            patientDto.DateOfBirth = patient.DateOfBirth;
            // return DTO
            return patientDto;
        }
    }
}