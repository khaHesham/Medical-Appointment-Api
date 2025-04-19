using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointment.API.Models;
using MedicalAppointmentApi.DTOs.Appointment;
using MedicalAppointmentApi.Exceptions;
using MedicalAppointmentApi.Interfaces;
using MedicalAppointmentApi.Models.Data;
using MedicalAppointmentApi.Models.Entities;

namespace MedicalAppointmentApi.Services
{
    public class AppointmentService : IAppointmentService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;

        public AppointmentService(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<bool> CancelAsync(int appointmentId, int userId, string role)
        {
            var repo = _unitOfWork.Repository<Appointment>();
            var appointment = await repo.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new NotFoundException("Appointment not found.");

            if (role == Role.Patient.ToString() && appointment.PatientId != userId)
                throw new ForbiddenAccessException("Cannot cancel another patient's appointment.");

            if (role == Role.Doctor.ToString() && appointment.DoctorId != userId)
                throw new ForbiddenAccessException("Cannot cancel another doctor's appointment.");

            appointment.Status = AppointmentStatus.Cancelled;
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<IEnumerable<AppointmentDTO>> GetDoctorAppointmentsAsync(int doctorId, int page, int pageSize)
        {
            var repo = _unitOfWork.Repository<Appointment>();
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var patientRepo = _unitOfWork.Repository<Patient>();


            var doctor = await doctorRepo.GetByIdAsync(doctorId);
            if (doctor == null)
                throw new NotFoundException("Doctor not found.");
            
            var appointments = (await repo.GetAllAsync())
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            return appointments.Select(a =>
            {
                var patient = patientRepo.GetByIdAsync(a.PatientId!).Result;
                return new AppointmentDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status.ToString(),
                    DoctorName = doctor != null ? $"{doctor.FirstName} {doctor.LastName}" : null,
                    PatientName = patient != null ? $"{patient.FirstName} {patient.LastName}" : null
                };
            });
        }

        public async Task<IEnumerable<AppointmentDTO>> GetPatientAppointmentsAsync(int patientId, int page, int pageSize)
        {
            var repo = _unitOfWork.Repository<Appointment>();
            var patientRepo = _unitOfWork.Repository<Patient>();
            var doctorRepo = _unitOfWork.Repository<Doctor>();

            var patient = await patientRepo.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException("Patient not found.");

            var appointments = (await repo.GetAllAsync())
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            if (appointments == null || !appointments.Any())
                throw new NotFoundException("No appointments found for this patient.");

            return appointments.Select(a =>
            {
                var doctor = doctorRepo.GetByIdAsync(a.DoctorId).Result;
                return new AppointmentDTO
                {
                    Id = a.Id,
                    AppointmentDate = a.AppointmentDate,
                    Reason = a.Reason,
                    Status = a.Status.ToString(),
                    DoctorName = doctor != null ? $"{doctor.FirstName} {doctor.LastName}" : null,
                    PatientName = $"{patient.FirstName} {patient.LastName}"
                };
            });
        }

        public async Task<bool> ScheduleAsync(int patientId, ScheduleAppointmentRequest request)
        {
            var doctorRepo = _unitOfWork.Repository<Doctor>();
            var appointmentRepo = _unitOfWork.Repository<Appointment>();
            var patientRepo = _unitOfWork.Repository<Patient>();

            var doctor = await doctorRepo.GetByIdAsync(request.DoctorId);
            if (doctor == null)
                throw new NotFoundException("Doctor not found.");

            var patient = await patientRepo.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException("Patient not found.");

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patientId,
                AppointmentDate = request.AppointmentDate,
                Reason = request.Reason,
                Status = AppointmentStatus.Scheduled,
                Doctor = doctor,
                Patient = patient // Patient will be set later when the appointment is confirmed
            };

            await appointmentRepo.AddAsync(appointment);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<AppointmentDTO> GetAppointmentByIdAsync(int appointmentId, int userId, string role)
        {
            var repo = _unitOfWork.Repository<Appointment>();
            var appointment = await repo.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new NotFoundException("Appointment not found.");

            if (role == Role.Patient.ToString() && appointment.PatientId != userId)
                throw new ForbiddenAccessException("Cannot access another patient's appointment.");

            if (role == Role.Doctor.ToString() && appointment.DoctorId != userId)
                throw new ForbiddenAccessException("Cannot access another doctor's appointment.");

            return new AppointmentDTO
            {
                Id = appointment.Id,
                AppointmentDate = appointment.AppointmentDate,
                Reason = appointment.Reason,
                Status = appointment.Status.ToString(),
                DoctorName = appointment.Doctor != null ? $"{appointment.Doctor.FirstName} {appointment.Doctor.LastName}" : null,
                PatientName = appointment.Patient != null ? $"{appointment.Patient.FirstName} {appointment.Patient.LastName}" : null
            };
        }
    }
};