using System;
using System.Collections.Generic;
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

        public AppointmentGenerator(IServiceScopeFactory scopeFactory)
      {
          this.scopeFactory = scopeFactory;
      }

      public Task StartAsync(CancellationToken cancellationToken)
        {
            // timer repeates call to RemoveScheduledAccounts every 24 hours.
            _timer = new Timer(
                AddAppointments, 
                null, 
                TimeSpan.FromSeconds(20), 
                TimeSpan.FromSeconds(20)
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


        private void AddAppointments(object state)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var clinicContext = scope.ServiceProvider.GetRequiredService<ClinicContext>();
                

                clinicContext.AddRange(new Appointment()
                    {
                        AppointmentDateTime = DateTime.Now.AddDays(3),
                        DurationInMin = 15,
                        DoctorEmailAddress = "Charlie@gmail.uk",
                        PatientCpr = "110695-0004",
                        Description = "Knee checkup"


                    },
                    new Appointment()
                    {
                        AppointmentDateTime = DateTime.Now.AddDays(3),
                        DurationInMin = 15,
                        DoctorEmailAddress = "Karl@gmail.com",
                        PatientCpr = "011200-4041"
                    });

                clinicContext.SaveChanges();
            }
        }
  } 
}
