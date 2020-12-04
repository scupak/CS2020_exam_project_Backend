using Core.Entities.Entities.BE;

namespace Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(ClinicContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var doctor1 = context.Add(new Doctor()
            {
                FirstName = "Karl",
                LastName = "Stevenson",
                EmailAddress = "Karl@gmail.com",
                PhoneNumber = "23418957"

            }).Entity;

            var doctor2 = context.Add(new Doctor()
            {
                FirstName = "Charlie",
                LastName = "Holmes",
                EmailAddress = "Charlie@gmail.uk",
                PhoneNumber = "87901234",
                IsAdmin = false

            }).Entity;

            var doctor3 = context.Add(new Doctor()
            {
                FirstName = "Anne",
                LastName = "Gorky",
                EmailAddress = "Anne@Yahoo.Ru",
                PhoneNumber = "45671289",
                IsAdmin = true

            }).Entity;

            var patient1 = context.Add(new Patient()
            {
                PatientCPR = "011200-4041",
                PatientFirstName = "frank",
                PatientLastName = "michel",
                PatientEmail = "frank@hotmail.com",
                PatientPhone = "45301210"

            }).Entity;

            context.SaveChanges();
        }
    }
}