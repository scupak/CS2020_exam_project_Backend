using System;
using Core.Entities.Entities.BE;

namespace Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private IAuthenticationHelper authenticationHelper;

        public DbInitializer(IAuthenticationHelper authHelper)
        {
            authenticationHelper = authHelper;
        }

        public void Initialize(ClinicContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Create two users with hashed and salted passwords
            string password = "1234";

            byte[] passwordHashKarl, passwordSaltKarl, passwordHashCharlie, passwordSaltCharlie, passwordHashAnne, passwordSaltAnne, passwordHashfrank, passwordSaltfrank, passwordHashGeorge, passwordSaltGeorge ,passwordHashKermit, passwordSaltKermit, passwordHashAdmin, passwordSaltAdmin;
            authenticationHelper.CreatePasswordHash(password, out passwordHashKarl, out passwordSaltKarl);
            authenticationHelper.CreatePasswordHash(password, out passwordHashCharlie, out passwordSaltCharlie);
            authenticationHelper.CreatePasswordHash(password, out passwordHashAnne, out passwordSaltAnne);
            authenticationHelper.CreatePasswordHash(password, out passwordHashfrank, out passwordSaltfrank);
            authenticationHelper.CreatePasswordHash(password, out passwordHashGeorge, out passwordSaltGeorge);
            authenticationHelper.CreatePasswordHash(password, out passwordHashKermit, out passwordSaltKermit);
            authenticationHelper.CreatePasswordHash(password, out passwordHashAdmin, out passwordSaltAdmin);

            var doctor1 = context.Add(new Doctor()
            {
                FirstName = "Karl",
                LastName = "Stevenson",
                DoctorEmailAddress= "Karl@gmail.com",
                PhoneNumber = "23418957",
                PasswordHash = passwordHashKarl,
                PasswordSalt = passwordSaltKarl,
                IsAdmin = true

            }).Entity;

            var doctor2 = context.Add(new Doctor()
            {
                FirstName = "Charlie",
                LastName = "Holmes",
                DoctorEmailAddress = "Charlie@gmail.uk",
                PhoneNumber = "87901234",
                IsAdmin = false,
                PasswordHash = passwordHashCharlie,
                PasswordSalt = passwordSaltCharlie,

            }).Entity;

            var doctor3 = context.Add(new Doctor()
            {
                FirstName = "Anne",
                LastName = "Gorky",
                DoctorEmailAddress = "Anne@Yahoo.Ru",
                PhoneNumber = "45671289",
                IsAdmin = true,
                PasswordHash = passwordHashAnne,
                PasswordSalt = passwordSaltAnne,

            }).Entity;

            var doctor4 = context.Add(new Doctor()
            {
                FirstName = "Admin",
                LastName = "Admin",
                DoctorEmailAddress = "4Admin@gmail.com",
                PhoneNumber = "11111111",
                IsAdmin = true,
                PasswordHash = passwordHashAnne,
                PasswordSalt = passwordSaltAnne,

            }).Entity;

            var patient1 = context.Add(new Patient()
            {
                PatientCPR = "011200-4041",
                PatientFirstName = "frank",
                PatientLastName = "michel",
                PatientEmail = "frank@hotmail.com",
                PatientPhone = "45301210", 
                PasswordHash = passwordHashfrank,
                PasswordSalt = passwordSaltfrank,

            }).Entity;

            var patient2 = context.Add(new Patient()
            {
                PatientCPR = "110695-0004",
                PatientFirstName = "George",
                PatientLastName = "Finch",
                PatientEmail = "George@hotmail.uk",
                PatientPhone = "67891245",
                PasswordHash = passwordHashGeorge,
                PasswordSalt = passwordSaltGeorge,

            }).Entity;

            var patient3 = context.Add(new Patient()
            {
                PatientCPR = "230207-5118",
                PatientFirstName = "Kermit",
                PatientLastName = "Holland",
                PatientEmail = "Kermit@gmail.com",
                PatientPhone = "98761234",
                PasswordHash = passwordHashKermit,
                PasswordSalt = passwordSaltKermit,


            }).Entity;

            var appointment1 = context.Add(new Appointment()
            {
              AppointmentDateTime  = DateTime.Now.AddDays(5),
              DurationInMin = 15,
              DoctorEmailAddress = "Charlie@gmail.uk",
            }).Entity;

            var appointment2 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(3),
                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com",
                PatientCpr = "011200-4041"
            }).Entity;

            var appointment3 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(3),
                DurationInMin = 15,
                DoctorEmailAddress = "Charlie@gmail.uk",
                PatientCpr = "110695-0004",
                Description = "Knee checkup"
            }).Entity;

            /*
            DateTime begin = DateTime.Today;

            DateTime end = DateTime.Today.AddDays(30);


            for(DateTime date = begin; date <= end; date = date.AddDays(1))
            {
                foreach (Doctor doctor in context.Doctors)
                {



                }

            }
            */

            var appointment4 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(5),
                DurationInMin = 15,
                DoctorEmailAddress = "Charlie@gmail.uk",
                PatientCpr = "011200-4041"
            }).Entity;

            var appointment5 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(3),
                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com",
                PatientCpr = "110695-0004",
            }).Entity;

            var appointment6 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(3),
                DurationInMin = 15,
                DoctorEmailAddress = "Charlie@gmail.uk",
                PatientCpr = "230207-5118",
                Description = "Knee checkup"
            }).Entity;

            context.SaveChanges();
        }


    }
}