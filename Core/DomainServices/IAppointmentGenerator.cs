using System;
using Microsoft.Extensions.Hosting;

namespace Core.Services.DomainServices
{
   public interface IAppointmentGenerator : IHostedService,IDisposable
    {
        public bool Running { get; set; }
    }
}
