using MedicalAppointmentApi.DTOs;
using MedicalAppointmentApi.DTOs.User.Doctor;
using MedicalAppointmentApi.Exceptions;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointmentApi.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public DoctorService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public Task<DoctorDTO> GetDoctorByIdAsync(int id)
        {
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var doctor = doctorRepo.GetByIdAsync(id).Result;
            if (doctor == null)
            {
                throw new NotFoundException($"Doctor with ID {id} not found.");
            }

            // convert to DTO
            var doctorDto = new DoctorDTO();
            doctorDto.Id = doctor.Id;
            doctorDto.FirstName = doctor.FirstName;
            doctorDto.LastName = doctor.LastName;
            doctorDto.PhoneNumber = doctor.PhoneNumber;
            doctorDto.Email = doctor.Email;
            // return DTO
            return Task.FromResult(doctorDto);
        }

        public async Task<DoctorDTO> UpdateDoctorAsync(int doctorId, UpdateDoctorRequest updateDoctorRequest)
        {
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var doctor = doctorRepo.GetByIdAsync(doctorId).Result;

            if (doctor == null)
                throw new NotFoundException($"Doctor with ID {doctorId} not found.");

            // update doctor properties
            if (updateDoctorRequest.FirstName != null)
                doctor.FirstName = updateDoctorRequest.FirstName;
            if (updateDoctorRequest.LastName != null)
                doctor.LastName = updateDoctorRequest.LastName;
            if (updateDoctorRequest.PhoneNumber != null)
                doctor.PhoneNumber = updateDoctorRequest.PhoneNumber;
            if(updateDoctorRequest.Specialization != null)
                doctor.Specialization = updateDoctorRequest.Specialization;

            // save changes
            await _unitOfWork.CompleteAsync();

            // convert to DTO
            var doctorDto = new DoctorDTO();
            doctorDto.Id = doctor.Id;
            doctorDto.FirstName = doctor.FirstName;
            doctorDto.LastName = doctor.LastName;
            doctorDto.PhoneNumber = doctor.PhoneNumber;
            doctorDto.Email = doctor.Email;
            doctorDto.Specialization = doctor.Specialization;

            return doctorDto;
        }
        
    }
}