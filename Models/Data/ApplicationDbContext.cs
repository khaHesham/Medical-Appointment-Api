
using MedicalAppointment.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointmentApi.Models.Entities
{


    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Patient>().ToTable("Patients");
            modelBuilder.Entity<Doctor>().ToTable("Doctors");


            //  Doctor-Appointment relationship
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)                  // One appointment has one doctor
                .WithMany(d => d.Appointments)          // One doctor can have many appointments
                .HasForeignKey(a => a.DoctorId)         // The foreign key in Appointment for Doctor
                .OnDelete(DeleteBehavior.NoAction);     // no action when Doctor is deleted this appointment may be assigned to another doctor

            //  Patient-Appointment relationship
            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)                     // One appointment has one patient
                .WithMany(p => p.Appointments)          // One patient can have many appointments
                .HasForeignKey(a => a.PatientId)        // The foreign key in Appointment for Patient
                .OnDelete(DeleteBehavior.NoAction);     // no action when Patient is deleted this appointment may be assigned to another patient

            base.OnModelCreating(modelBuilder);
        }
    }

}