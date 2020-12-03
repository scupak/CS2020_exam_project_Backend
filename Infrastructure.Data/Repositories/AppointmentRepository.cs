using System.Collections.Generic;
using Core.Entities.Entities.BE;
using Core.Services.DomainServices;

namespace Infrastructure.Data.Repositories
{
    public class AppointmentRepository : IRepository<Appointment, int>
    {
        public List<Appointment> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Appointment GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public Appointment Add(Appointment entity)
        {
            throw new System.NotImplementedException();
        }

        public Appointment Edit(Appointment entity)
        {
            throw new System.NotImplementedException();
        }

        public Appointment Remove(int id)
        {
            throw new System.NotImplementedException();
        }

        public int Count()
        {
            throw new System.NotImplementedException();
        }
    }
}