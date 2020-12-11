using Core.Entities.Entities.BE;
using Core.Services.Validators.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Services.Validators.Implementations
{
    public class PatientValidator : IPatientValidator
    {
        public void DefaultValidator(Patient patient)
        {
            if(patient == null)
            {
                throw new  NullReferenceException("Patient cannot be null!");
            }
            ValidateFirstName(patient);
            ValidateLastName(patient);
            ValidatePhone(patient);
            ValidateEmail(patient);
            ValidateCPR(patient);
        }

        private void ValidateFirstName(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientFirstName))
            {
                throw new  NullReferenceException("Patient Firstname cannot be null or empty!");
            }
            
        }

        private void ValidateLastName(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientLastName))
            {
                throw new  NullReferenceException("Patient Lastname cannot be null or empty!");
            }
            
        }

        private void ValidatePhone(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientPhone))
            {
                throw new  NullReferenceException("Patient phone number cannot be null or empty!");
            }

            if (!Regex.IsMatch(patient.PatientPhone, "(^\\s?\\d{2}\\s?\\d{2}\\s?\\d{2}\\s?\\d{2})$"))
            {

                throw new InvalidDataException("Patient Phone number has to be a valid Phone number");
            }
            
        }

        private void ValidateEmail(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientEmail))
            {
                throw new  NullReferenceException("Patient e-mail cannot be null or empty!");
            }

            if (!Regex.IsMatch(patient.PatientEmail, "^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$"))
            {

                throw new InvalidDataException("Patient Email has to be a valid Email");
            }
            
        }

        void ValidateCPR(Patient patient)
        {
            if (String.IsNullOrEmpty(patient.PatientCPR))
            {
                throw new  NullReferenceException("Patient CPR cannot be null or empty!");
            }

            if (!Regex.IsMatch(patient.PatientCPR, "^((((0[1-9]|[12][0-9]|3[01])(0[13578]|10|12)(\\d{2}))|(([0][1-9]|[12][0-9]|30)(0[469]|11)(\\d{2}))|((0[1-9]|1[0-9]|2[0-8])(02)(\\d{2}))|((29)(02)(00))|((29)(02)([2468][048]))|((29)(02)([13579][26])))[-]\\d{4})$"))
            {

                throw new InvalidDataException("Patient CPR has to be a valid CPR number");
            }



        }
        public void ValidateCPR(string CPR)
        {
            if (String.IsNullOrEmpty(CPR))
            {
                throw new NullReferenceException("Patient CPR cannot be null or empty!");
            }

            if (!Regex.IsMatch(CPR, "^((((0[1-9]|[12][0-9]|3[01])(0[13578]|10|12)(\\d{2}))|(([0][1-9]|[12][0-9]|30)(0[469]|11)(\\d{2}))|((0[1-9]|1[0-9]|2[0-8])(02)(\\d{2}))|((29)(02)(00))|((29)(02)([2468][048]))|((29)(02)([13579][26])))[-]\\d{4})$"))
            {

                throw new InvalidDataException("Patient CPR has to be a valid CPR number");
            }



        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("a Patient needs a valid password");
            }
        }
    }
}
