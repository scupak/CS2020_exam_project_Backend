using System;
using System.Collections.Generic;
using System.Linq;
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
            return _doctorRepository.GetAll();
        }

        public Doctor GetById(int id)
        {
            try
            {
                _doctorValidator.IdValidation(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Id validation failed\n" + ex.Message , ex);
            }

            Doctor doctor = _doctorRepository.GetById(id);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor does not exist");
            }

            return doctor;

        }

        public Doctor Add(Doctor entity)
        {
            try
            {
                _doctorValidator.CreateValidation(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Validation failed\n" + ex.Message, ex);
            }

            Doctor doctor = _doctorRepository.Add(entity);
            return doctor;

        }

        public Doctor Edit(Doctor entity)
        {
            try
            {
                _doctorValidator.EditValidation(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Validation failed\n" + ex.Message, ex);
            }

            var previousDoctor = _doctorRepository.GetById(entity.DoctorId);

            if (previousDoctor == null)
            {
                throw new ArgumentException("A doctor with this id does not exist");
            }

            entity.PasswordHash = previousDoctor.PasswordHash;
            entity.PasswordSalt = previousDoctor.PasswordSalt;

            Doctor doctor = _doctorRepository.Edit(entity);
            return doctor;

        }

        public Doctor Remove(int id)
        {
            try
            {
                _doctorValidator.IdValidation(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Validation failed\n" + ex.Message, ex);
            }

            if (_doctorRepository.GetById(id) == null)
            {
                throw new KeyNotFoundException("This doctor does not exist");
            }

            Doctor doctor = _doctorRepository.Remove(id);
            return doctor;

        }
    }
}