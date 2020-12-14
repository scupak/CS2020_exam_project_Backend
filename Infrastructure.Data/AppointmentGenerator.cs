using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Data
{
  public class AppointmentGenerator : IHostedService
  {
      private readonly IServiceScopeFactory scopeFactory;
        private Timer _timer;
        public bool Runnning { get; set; }

        public AppointmentGenerator(IServiceScopeFactory scopeFactory)
      {
          this.scopeFactory = scopeFactory;
      }

      public Task StartAsync(CancellationToken cancellationToken)
      {
          Runnning = true;
            // timer repeates call to RemoveScheduledAccounts every 24 hours.
            _timer = new Timer(
                AddAppointments, 
                null, 
                TimeSpan.FromSeconds(40), 
                TimeSpan.FromSeconds(20)
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Runnning = false;
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


        private void AddAppointments(object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var clinicContext = scope.ServiceProvider.GetRequiredService<ClinicContext>();

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


                if (clinicContext.Doctors.Any())
                {
                    List<Appointment> appointmentsToAdd = new List<Appointment>();

                    DateTime begin = DateTime.Today;

                    DateTime end = DateTime.Today.AddDays(7);


                    for (DateTime date = begin; date <= end; date = date.AddDays(1))
                    {
                        IEnumerable<Appointment> appointmentsInDay = new List<Appointment>();

                        if (clinicContext.Appointments.Any(appointment =>
                            appointment.AppointmentDateTime.Date == date.Date))
                        {
                            appointmentsInDay = clinicContext.Appointments.Where(appointment =>
                                appointment.AppointmentDateTime.Date == date.Date);
                        }



                        foreach (Doctor doctor in clinicContext.Doctors)
                        {
                            DateTime openingTime = date.Date + new TimeSpan(10, 00, 00);
                            DateTime closingTime = date.Date + new TimeSpan(16, 00, 00);
                            DateTime lunchStartTime = date.Date + new TimeSpan(12, 00, 00);
                            DateTime lunchEndTime = date.Date + new TimeSpan(13, 00, 00);

                            DateTime iterateDateTime = openingTime;

                            while (iterateDateTime < closingTime)
                            {
                                if (iterateDateTime !>= lunchStartTime && iterateDateTime !<= lunchEndTime)
                                {
                                    

                                  

                                }
                                else
                                {


                                    
                                    bool anyIntersectingAppointments =  appointmentsInDay.Any(appointment =>
                                        (appointment.AppointmentDateTime >= iterateDateTime && appointment.AppointmentDateTime <= iterateDateTime.AddMinutes(15))
                                        &&
                                        (appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) >= iterateDateTime && appointment.AppointmentDateTime.AddMinutes(appointment.DurationInMin) <= iterateDateTime.AddMinutes(15)));

                                    
                                    if(!anyIntersectingAppointments)
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

                    if (appointmentsToAdd.Any())
                    {
                        clinicContext.AddRange(appointmentsToAdd);
                        clinicContext.SaveChanges();
                    }

                }

            }
        }
  } 
}
