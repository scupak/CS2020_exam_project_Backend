using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ClinicContext: DbContext
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}