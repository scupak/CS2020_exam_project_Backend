using System.Collections.Generic;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;

namespace Core.Services.ApplicationServices.Implementations
{
    public class DoctorService : IService<Doctor>
    {
        private readonly IRepository<Doctor> _doctorRepository;

        public DoctorService(IRepository<Doctor> doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public List<Doctor> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Doctor Get(int id)
        {
            throw new System.NotImplementedException();
        }

        public Doctor Add(Doctor entity)
        {
            throw new System.NotImplementedException();
        }

        public Doctor Edit(Doctor entity)
        {
            throw new System.NotImplementedException();
        }

        public Doctor Remove(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}