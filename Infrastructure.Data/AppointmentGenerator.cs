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

                DateTime begin = DateTime.Today;

                DateTime end = DateTime.Today.AddDays(30);


                for (DateTime date = begin; date <= end; date = date.AddDays(1))
                {


                    if (clinicContext.Doctors.Any())
                    {
                        foreach (Doctor doctor in clinicContext.Doctors)
                        {
                            DateTime OpeningTime = date.Date + new TimeSpan(10, 00, 00);
                            DateTime ClosingTime = date.Date + new TimeSpan(16, 00, 00);
                            DateTime LunchStartTime = date.Date + new TimeSpan(12, 00, 00);
                            DateTime LunchEndTime = date.Date + new TimeSpan(13, 00, 00);

                            /*
                            DateTime OpeningTime = date.Hour(10).Minute(00);
                            DateTime ClosingTime = date.Hour(16).Minute(00);
                            DateTime LunchStartTime = date.Hour(12).Minute(00);
                            DateTime LunchEndTime = date.Hour(13).Minute(00);
                            */
                            Console.WriteLine(OpeningTime + ", " + ClosingTime + ", " + LunchStartTime + ", " + LunchEndTime);
                        }

                    }
                }

            }
        }
  } 
}
