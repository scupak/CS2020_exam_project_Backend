using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Interfaces;

namespace Core.Services.ApplicationServices.Implementations
{
   public class PatientService : IService<Patient,string>
    {
        private IRepository<Patient, string> _patientRepository;
        private IPatientValidator _patientValidator;

        public PatientService(IRepository<Patient, string> patientRepository, IPatientValidator patientValidator)
        {
            _patientRepository = patientRepository;
            _patientValidator = patientValidator;
        }

        public FilteredList<Patient> GetAll(Filter filter)
        {
            if (filter.CurrentPage < 0 || filter.ItemsPrPage < 0)
            {
                throw new InvalidDataException("current page and items pr page can't be negative");
            }

            if ((filter.CurrentPage - 1) * filter.ItemsPrPage >= _patientRepository.Count())
            {
                throw new ArgumentException("no more patients");
            }

            var filteredPatients = _patientRepository.GetAll(filter);

            if (filteredPatients.List.Count < 1)
            {
                throw new KeyNotFoundException("Could not find patients that satisfy the filter");
            }
            return filteredPatients;
        }

        public Patient GetById(string id)
        {
          _patientValidator.ValidateCPR(id);
            
            return _patientRepository.GetById(id);
        }

        public Patient Add(Patient entity)
        {
           
            _patientValidator.DefaultValidator(entity);
           

            if(_patientRepository.GetById(entity.PatientCPR) != null)
            {
                throw new InvalidDataException("Patient is already in the database");
            }

            return _patientRepository.Add(entity);
        }

        public Patient Edit(Patient entity)
        {
           
            _patientValidator.DefaultValidator(entity);
          
         

            var previousPatient = _patientRepository.GetById(entity.PatientCPR);

            if (previousPatient == null)
            {
                throw new ArgumentException("Patient is not in the database");

               
            }

            /*
            entity.PasswordHash = previousPatient.PasswordHash;
            entity.PasswordSalt = previousPatient.PasswordSalt;
            */
            return _patientRepository.Edit(entity);
        }

        public Patient Remove(string id)
        {
            _patientValidator.ValidateCPR(id);
            
            if(_patientRepository.GetById(id) == null)
            {
                throw new ArgumentException("Nonexistant patient cannot be removed!");

                
            }

            return _patientRepository.Remove(id);


        }
    }
}
