using System;
using System.Text.RegularExpressions;
using Core.Entities.Entities.BE;
using Core.Services.Validators.Interfaces;

namespace Core.Services.Validators.Implementations
{
    public class DoctorValidator : IDoctorValidator
    {
        public void DefaultValidator(Doctor doctor)
        {
            if (doctor == null)
            {
                throw new NullReferenceException("Doctor cannot be null");
            }

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
            if (doctor.DoctorEmailAddress == null)
            {
                throw new NullReferenceException("a doctor needs an email");
            }

            if (!Regex.IsMatch(doctor.DoctorEmailAddress, "^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$"))
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


        public void ValidateEmail(string email)
        {
            if (!Regex.IsMatch(email, "^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$"))
            {
                throw new ArgumentException("This is not a valid email address");
            }
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("a doctor needs a valid password");
            }
        }
    }
}