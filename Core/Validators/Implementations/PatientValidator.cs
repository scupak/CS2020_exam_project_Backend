using Core.Entities.Entities.BE;
using Core.Services.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Validators.Implementations
{
    public class PatientValidator : IPatientValidator
    {
        public void DefaultValidator(Patient patient)
        {
            if(patient == null)
            {
                throw new ArgumentNullException("Patient cannot be null!");
            }
            ValidateName(patient);
        }

        private void ValidateName(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientName))
            {
                throw new ArgumentNullException("Patient name cannot be null or empty!");
            }
            ValidatePhone(patient);
        }

        private void ValidatePhone(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientPhone))
            {
                throw new ArgumentNullException("Patient phone number cannot be null or empty!");
            }
            ValidateEmail(patient);
        }

        private void ValidateEmail(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientEmail))
            {
                throw new ArgumentNullException("Patient e-mail cannot be null or empty!");
            }
            ValidateCPR(patient);
        }

        private void ValidateCPR(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientCPR))
            {
                throw new ArgumentNullException("Patient CPR cannot be null or empty!");
            }
        }
    }
}
