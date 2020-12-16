using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Entities.BE;

namespace Core.Services.Validators.Interfaces
{
    public interface IAppointmentValidator
    {
        public void CreateValidation(Appointment appointment);
        public void EditValidation(Appointment appointment);
        public void IdValidation(int id);
    }
}
