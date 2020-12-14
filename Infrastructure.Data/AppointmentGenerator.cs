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

                if (clinicContext.Doctors.First(doctor => doctor.DoctorEmailAddress.Contains("Karl@gmail.com")) != null)
                {
                    clinicContext.AddRange(
                        new Appointment()
                        {
                            AppointmentDateTime = DateTime.Now.AddDays(3),
                            DurationInMin = 15,
                            DoctorEmailAddress = "Karl@gmail.com"
                        });

                    clinicContext.SaveChanges();
                }

                
               
            }
        }
  } 
}
