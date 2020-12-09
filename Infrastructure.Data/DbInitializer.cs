using System;
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
                DoctorEmailAddress= "Karl@gmail.com",
                PhoneNumber = "23418957"

            }).Entity;

            var doctor2 = context.Add(new Doctor()
            {
                FirstName = "Charlie",
                LastName = "Holmes",
                DoctorEmailAddress = "Charlie@gmail.uk",
                PhoneNumber = "87901234",
                IsAdmin = false

            }).Entity;

            var doctor3 = context.Add(new Doctor()
            {
                FirstName = "Anne",
                LastName = "Gorky",
                DoctorEmailAddress = "Anne@Yahoo.Ru",
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

            var patient2 = context.Add(new Patient()
            {
                PatientCPR = "110695-0004",
                PatientFirstName = "George",
                PatientLastName = "Finch",
                PatientEmail = "George@hotmail.uk",
                PatientPhone = "67891245"

            }).Entity;

            var patient3 = context.Add(new Patient()
            {
                PatientCPR = "230207-5118",
                PatientFirstName = "Kermit",
                PatientLastName = "Holland",
                PatientEmail = "Kermit@gmail.com",
                PatientPhone = "98761234"

            }).Entity;

            var appointment1 = context.Add(new Appointment()
            {
              AppointmentDateTime  = DateTime.Now.AddDays(5),
              DurationInMin = 15
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

            context.SaveChanges();
        }
    }
}