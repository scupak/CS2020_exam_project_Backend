using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Entities.Entities.BE;
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

        public List<Patient> GetAll()
        {
            return _patientRepository.GetAll();
        }

        public Patient GetById(string id)
        {
            try
            {
                _patientValidator.ValidateCPR(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }

            return _patientRepository.GetById(id);
        }

        public Patient Add(Patient entity)
        {
            try 
            {
                _patientValidator.DefaultValidator(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

            if(_patientRepository.GetById(entity.PatientCPR) != null)
            {
                throw new InvalidDataException("Patient is already in the database");
            }

            return _patientRepository.Add(entity);
        }

        public Patient Edit(Patient entity)
        {
            try
            {
                _patientValidator.DefaultValidator(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);

            }

            if(_patientRepository.GetById(entity.PatientCPR) == null)
            {
                throw new ArgumentException("Patient is not in the database");

               
            }

            return _patientRepository.Edit(entity);
        }

        public Patient Remove(string id)
        {
            try
            {
                _patientValidator.ValidateCPR(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }

            if(_patientRepository.GetById(id) == null)
            {
                throw new ArgumentException("Nonexistant patient cannot be removed!");

                
            }

            return _patientRepository.Remove(id);


        }
    }
}
