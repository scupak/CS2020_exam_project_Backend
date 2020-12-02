using System.Collections.Generic;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Interfaces;

namespace Core.Services.ApplicationServices.Implementations
{
    public class DoctorService : IService<Doctor, int>
    {
        private readonly IRepository<Doctor, int> _doctorRepository;
        private readonly IDoctorValidator _doctorValidator;

        public DoctorService(IRepository<Doctor, int> doctorRepository, IDoctorValidator doctorValidator)
        {
            _doctorRepository = doctorRepository;
            _doctorValidator = doctorValidator;
        }

        public List<Doctor> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Doctor GetById(int id)
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