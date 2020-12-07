using System;
using System.Collections.Generic;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Interfaces;

namespace Core.Services.ApplicationServices.Implementations
{
    public class DoctorService : IService<Doctor, string>
    {
        private readonly IRepository<Doctor, string> _doctorRepository;
        private readonly IDoctorValidator _doctorValidator;

        public DoctorService(IRepository<Doctor, string> doctorRepository, IDoctorValidator doctorValidator)
        {
            _doctorRepository = doctorRepository;
            _doctorValidator = doctorValidator;
        }

        public List<Doctor> GetAll()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor GetById(string email)
        {
            try
            {
                _doctorValidator.ValidateEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception("email validation failed\n" + ex.Message , ex);
            }

            Doctor doctor = _doctorRepository.GetById(email);

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
                _doctorValidator.DefaultValidator(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Validation failed\n" + ex.Message, ex);
            }

            var previousDoctor = _doctorRepository.GetById(entity.DoctorEmailAddress);

            if (previousDoctor != null)
            {
                throw new ArgumentException("A doctor with this email already exists");
            }

            Doctor doctor = _doctorRepository.Add(entity);
            return doctor;

        }

        public Doctor Edit(Doctor entity)
        {
            try
            {
                _doctorValidator.DefaultValidator(entity);
            }
            catch (Exception ex)
            {
                throw new Exception("Validation failed\n" + ex.Message, ex);
            }

            var previousDoctor = _doctorRepository.GetById(entity.DoctorEmailAddress);

            if (previousDoctor == null)
            {
                throw new ArgumentException("A doctor with this email does not exist");
            }

            entity.PasswordHash = previousDoctor.PasswordHash;
            entity.PasswordSalt = previousDoctor.PasswordSalt;

            Doctor doctor = _doctorRepository.Edit(entity);
            return doctor;

        }

        public Doctor Remove(string email)
        {
            try
            {
                _doctorValidator.ValidateEmail(email);
            }
            catch (Exception ex)
            {
                throw new Exception("Validation failed\n" + ex.Message, ex);
            }

            if (_doctorRepository.GetById(email) == null)
            {
                throw new KeyNotFoundException("This doctor does not exist");
            }

            Doctor doctor = _doctorRepository.Remove(email);
            return doctor;

        }
    }
}