using System.Collections.Generic;
using Core.Entities.Entities.BE;
using Core.Services.DomainServices;

namespace Infrastructure.Data.Repositories
{
    public class AppointmentRepository : IRepository<Appointment>
    {
        public IEnumerable<Appointment> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Appointment Get(long id)
        {
            throw new System.NotImplementedException();
        }

        public void Add(Appointment entity)
        {
            throw new System.NotImplementedException();
        }

        public void Edit(Appointment entity)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}