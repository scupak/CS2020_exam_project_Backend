using Core.Entities.Entities.BE;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Services.Validators.Interfaces
{
    public interface IPatientValidator
    {
        public void DefaultValidator(Patient patient);

        public void ValidateCPR(string CPR);

        public void ValidatePassword(string password);
    }
}
