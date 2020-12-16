using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
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

        public FilteredList<Doctor> GetAll(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("current page and items pr page can't be negative");
            }

            if ((filter.CurrentPage - 1) * filter.ItemsPrPage >= _doctorRepository.Count())
            {
                throw new ArgumentException("no more doctors");
            }

            var filteredDoctors = _doctorRepository.GetAll(filter);

            if (filteredDoctors.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find doctors that satisfy the filter");
            }
            return filteredDoctors;
        }

        public Doctor GetById(string email)
        {
            _doctorValidator.ValidateEmail(email);
            

            Doctor doctor = _doctorRepository.GetById(email);

            if (doctor == null)
            {
                throw new KeyNotFoundException("Doctor does not exist");
            }

            return doctor;

        }

        public Doctor Add(Doctor entity)
        {
            _doctorValidator.DefaultValidator(entity);
           
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
           _doctorValidator.DefaultValidator(entity);
            

            var previousDoctor = _doctorRepository.GetById(entity.DoctorEmailAddress);

            if (previousDoctor == null)
            {
                throw new ArgumentException("A doctor with this email does not exist");
            }

            /*

            entity.PasswordHash = previousDoctor.PasswordHash;
            entity.PasswordSalt = previousDoctor.PasswordSalt;
            */

            Doctor doctor = _doctorRepository.Edit(entity);
            return doctor;

        }

        public Doctor Remove(string email)
        {
            _doctorValidator.ValidateEmail(email);
            
            if (_doctorRepository.GetById(email) == null)
            {
                throw new KeyNotFoundException("This doctor does not exist");
            }

            Doctor doctor = _doctorRepository.Remove(email);
            return doctor;

        }
    }
}