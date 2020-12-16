using System;
using System.Collections.Generic;
using System.Linq;
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
            /*
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
            */
            
            /*
            DateTime begin = DateTime.Today;

            DateTime end = DateTime.Today.AddDays(30);
            */

           
            
            /*
            var appointment4 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(5),
                DurationInMin = 15,
                DoctorEmailAddress = "Charlie@gmail.uk",
                PatientCpr = "011200-4041"
            }).Entity;

            var appointment5 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Today,
                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com"
            }).Entity;

            var appointment6 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Today.AddDays(1),
                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com"
            }).Entity;

            var appointment7 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(2),
                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com"
            }).Entity;

            var appointment8 = context.Add(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(3),
                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com",
            }).Entity;

            var appointment9 = context.Add(new Appointment()
            {

                AppointmentDateTime = DateTime.Today + new TimeSpan(10, 00, 00),

                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com",
                Description = "generator test"
            }).Entity;

            var appointment10 = context.Add(new Appointment()
            {

                AppointmentDateTime = DateTime.Today + new TimeSpan(10, 15, 00),

                DurationInMin = 15,
                DoctorEmailAddress = "Karl@gmail.com",
                Description = "generator test"
            }).Entity;

            var appointment11 = context.Add(new Appointment()
            {

                AppointmentDateTime = DateTime.Today + new TimeSpan(10, 30, 00),

                DurationInMin = 240,
                DoctorEmailAddress = "Karl@gmail.com",
                Description = "generator test"
            }).Entity;

            var appointment12 = context.Add(new Appointment()
            {

                AppointmentDateTime = DateTime.Today.AddDays(1) + new TimeSpan(10, 30, 00),

                DurationInMin = 300,
                DoctorEmailAddress = "Karl@gmail.com",
                Description = "generator test"
            }).Entity;

            var appointment14 = context.Add(new Appointment()
            {

                AppointmentDateTime = DateTime.Today.AddDays(2) + new TimeSpan(10, 00, 00),

                DurationInMin = 1,
                DoctorEmailAddress = "Karl@gmail.com",
                Description = "generator test super short"
            }).Entity;

             var appointment15 = context.Add(new Appointment()
            {

                AppointmentDateTime = DateTime.Today + new TimeSpan(10, 00, 00),

                DurationInMin = 15,
                DoctorEmailAddress = "Charlie@gmail.uk",
                Description = "generator test"
            }).Entity;


             var appointment16 = context.Add(new Appointment()
             {

                 AppointmentDateTime = DateTime.Today.AddDays(2) + new TimeSpan(10, 01, 00),

                 DurationInMin = 14,
                 DoctorEmailAddress = "Karl@gmail.com",
                 Description = "generator test super short 2"
             }).Entity;
            */
            context.SaveChanges();


               

















                /* if (clinicContext.Doctors.First(doctor => doctor.DoctorEmailAddress.Contains("Karl@gmail.com")) != null)
                 {
                     clinicContext.AddRange(
                         new Appointment()
                         {
                             AppointmentDateTime = DateTime.Now.AddDays(3),
                             DurationInMin = 15,
                             DoctorEmailAddress = "Karl@gmail.com"
                         });

                     clinicContext.SaveChanges();
                 } */


            //Only make appointments if there are any doctors. 

            
            /*
            if (context.Doctors.Any())
            {
                List<Appointment> appointmentsToAdd = new List<Appointment>();

                DateTime begin = DateTime.Today;

                DateTime end = DateTime.Today.AddDays(7);

                //loop through the days. 
                for (DateTime date = begin; date <= end; date = date.AddDays(1))
                {
                    //find all appointments for the day
                    IEnumerable<Appointment> appointmentsInDay = new List<Appointment>();

                    if ( context.Appointments.Any(appointment =>
                        appointment.AppointmentDateTime.Date == date.Date))
                    {
                        appointmentsInDay =  context.Appointments.Where(appointment =>
                            appointment.AppointmentDateTime.Date == date.Date);
                    }


                    if(date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    {


                        foreach (Doctor doctor in context.Doctors)
                        {
                            DateTime openingTime = date.Date + new TimeSpan(10, 00, 00);
                            DateTime closingTime = date.Date + new TimeSpan(16, 00, 00);
                            DateTime lunchStartTime = date.Date + new TimeSpan(12, 00, 00);
                            DateTime lunchEndTime = date.Date + new TimeSpan(13, 00, 00);

                            DateTime iterateDateTime = openingTime;

                            while (iterateDateTime < closingTime)
                            {
                                if (iterateDateTime ! >= lunchStartTime && iterateDateTime ! <= lunchEndTime)
                                {




                                }
                                else
                                {
                                    /*
                                     * filtering = filtering.Where(appointment =>
                        (appointment.AppointmentDateTime >= filter.OrderStartDateTime && appointment.AppointmentDateTime <= filter.OrderStopDateTime)
                        &&
                        (appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) >= filter.OrderStartDateTime && appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) <= filter.OrderStopDateTime));
                    
                                     */
            /*
                                    bool anyIntersectingAppointments =  appointmentsInDay.Where(appointment => appointment.DoctorEmailAddress.Contains(doctor.DoctorEmailAddress)).Any(appointment =>
                                        (appointment.AppointmentDateTime >= iterateDateTime && appointment.AppointmentDateTime <= iterateDateTime.AddMinutes(15))
                                        &&
                                        (appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) >= iterateDateTime && appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) <= iterateDateTime.AddMinutes(15)));


                                    bool anyIntersectingAppointments2 = appointmentsInDay.Where(appointment => appointment.DoctorEmailAddress.Contains(doctor.DoctorEmailAddress)).Any(appointment =>
                                        (iterateDateTime >= appointment.AppointmentDateTime && iterateDateTime < appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin))
                                        ||
                                        (iterateDateTime.AddMinutes(15) > appointment.AppointmentDateTime && iterateDateTime.AddMinutes(15) <= appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin)));
                                    
                             

                                    if(!anyIntersectingAppointments && !anyIntersectingAppointments2)
                                    { 
                                        appointmentsToAdd.Add(new Appointment()
                                        {
                                            AppointmentDateTime = iterateDateTime,
                                            DurationInMin = 15,
                                            DoctorEmailAddress = doctor.DoctorEmailAddress,

                                        });

                                    }

                                }

                                iterateDateTime = iterateDateTime.AddMinutes(15);




                            }

                        }
                    }


                }

                if (appointmentsToAdd.Any())
                {
                    context.AddRange(appointmentsToAdd);
                    context.SaveChanges();
                }
           
            }
                */ 
            
        }


    }
}