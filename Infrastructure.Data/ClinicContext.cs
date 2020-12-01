using Core.Entities.Entities.BE;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ClinicContext: DbContext
    {
        public DbSet<Patient> patients { get; set; }

        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>().HasKey(p => p.PatientCPR);
        }
    }
}