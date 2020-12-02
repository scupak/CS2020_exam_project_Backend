using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        public Patient GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Patient Add(Patient entity)
        {
            throw new NotImplementedException();
        }

        public Patient Edit(Patient entity)
        {
            throw new NotImplementedException();
        }

        public Patient Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}
