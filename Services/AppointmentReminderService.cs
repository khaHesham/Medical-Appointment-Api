using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicalAppointmentApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentApi.Services
{
public class AppointmentReminderService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentReminderService> _logger;

        public AppointmentReminderService(IServiceProvider serviceProvider, ILogger<AppointmentReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("📨 Appointment Reminder Service is starting...");

            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckUpcomingAppointments();

                // Wait for 1 hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }

            _logger.LogInformation("❌ Appointment Reminder Service is stopping...");
        }

        private async Task CheckUpcomingAppointments()
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var now = DateTime.Now;
            var upcoming = now.AddHours(24);  // Check for appointments in the next two days

            var appointments = await dbContext.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.AppointmentDate > now && a.AppointmentDate <= upcoming && a.Status == AppointmentStatus.Scheduled)
                .ToListAsync();

            foreach (var appt in appointments)
            {
                var doctorName = appt.Doctor != null ? $"{appt.Doctor.FirstName} {appt.Doctor.LastName}": "Unassigned";
                var patientName = appt.Patient != null ? $"{appt.Patient.FirstName} {appt.Patient.LastName}" : "Unassigned";

                _logger.LogInformation($"🔔 Reminder: Appointment at {appt.AppointmentDate:g} with Doctor {doctorName} for Patient {patientName}.");
            }
        }
    }
}