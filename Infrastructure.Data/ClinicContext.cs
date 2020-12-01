using Core.Entities.Entities.BE;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Data
{
    public class ClinicContext: DbContext
    {
        public DbSet<Patient> patients { get; set; }
    
        public DbSet<Doctor> Doctors { get; set; }

        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.PatientCPR);

            modelBuilder.Entity<Patient>()
                .Property(p => p.PatientCPR).ValueGeneratedNever();
            
            modelBuilder.Entity<Doctor>()
                .HasKey(doctor => doctor.DoctorId);
        }
    }
}