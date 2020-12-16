using Core.Entities.Entities.BE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Data
{
    public class ClinicContext: DbContext
    {
        public DbSet<Patient> Patients { get; set; }
    
        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>()
                .HasKey(appointment => appointment.AppointmentId);

            modelBuilder.Entity<Appointment>()
                .HasOne<Doctor>(appointment => appointment.Doctor)
                .WithMany(doctor => doctor.Appointments)
                .HasForeignKey(appointment => appointment.DoctorEmailAddress)
                .IsRequired(true);

            modelBuilder.Entity<Appointment>()
                .HasOne<Patient>(appointment => appointment.Patient)
                .WithMany(patient => patient.Appointments)
                .HasForeignKey(appointment => appointment.PatientCpr)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Patient>()
                .HasKey(patient => patient.PatientCPR);

            modelBuilder.Entity<Patient>()
                .Property(patient => patient.PatientCPR).ValueGeneratedNever();
            
            modelBuilder.Entity<Doctor>()
                .HasKey(doctor => doctor.DoctorEmailAddress);

            modelBuilder.Entity<Doctor>()
                .Property(doctor => doctor.DoctorEmailAddress).ValueGeneratedNever();
        }
    }
}