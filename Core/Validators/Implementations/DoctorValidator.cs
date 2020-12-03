using System;
using System.Text.RegularExpressions;
using Core.Entities.Entities.BE;
using Core.Services.Validators.Interfaces;

namespace Core.Services.Validators.Implementations
{
    public class DoctorValidator : IDoctorValidator
    {
        public void CreateValidation(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new NullReferenceException("Doctor cannot be null");
            }

            CreateIdValidation(doctor);
            FirstNameValidation(doctor);
            LastNameValidation(doctor);
            EmailValidation(doctor);
            PhoneValidation(doctor);

        }

        private void PhoneValidation(Doctor doctor)
        {
            if (doctor.PhoneNumber == null)
            {
                throw new NullReferenceException("a doctor needs a phone number");
            }

            if (!Regex.IsMatch(doctor.PhoneNumber,
                "(^\\s?\\d{2}\\s?\\d{2}\\s?\\d{2}\\s?\\d{2})$"))
            {
                throw new ArgumentException("a doctor needs a valid phone number");
            }
        }

        private void EmailValidation(Doctor doctor)
        {
            if (doctor.EmailAddress == null)
            {
                throw new NullReferenceException("a doctor needs an email");
            }

            if (!Regex.IsMatch(doctor.EmailAddress, "^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$"))
            {
                throw new ArgumentException("a doctor needs a valid email address");
            }
        }

        private void LastNameValidation(Doctor doctor)
        {
            
            if (string.IsNullOrEmpty(doctor.LastName) || doctor.LastName.Length < 2)
            {
                throw new ArgumentException("a doctor needs a valid last name");
            }
        }

        private void FirstNameValidation(Doctor doctor)
        {
            if (string.IsNullOrEmpty(doctor.FirstName) || doctor.FirstName.Length < 2)
            {
                throw new ArgumentException("a doctor needs a valid first name");
            }
        }

        private void CreateIdValidation(Doctor doctor)
        {
            if (doctor.DoctorId > 0)
            {
                throw new ArgumentException("A doctor should not have an id");
            }
        }

        public void EditValidation(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new NullReferenceException("Doctor cannot be null");
            }

            EditIdValidation(doctor);
            FirstNameValidation(doctor);
            LastNameValidation(doctor);
            EmailValidation(doctor);
            PhoneValidation(doctor);
        }

        private void EditIdValidation(Doctor doctor)
        {
            if (doctor.DoctorId < 1)
            {
                throw new NullReferenceException("A doctor should have an id");
            }
        }

        public void IdValidation(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("id cannot be lower than 1");
            }
        }
    }
}