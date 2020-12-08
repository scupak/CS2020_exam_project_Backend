using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Services.Validators.Interfaces;

namespace Core.Services.Validators.Implementations
{
    public class AppointmentValidator: IAppointmentValidator
    {
        public void CreateValidation(Appointment appointment)
        {
            if (appointment == null)
            {
                throw new NullReferenceException("Appointment cannot be null");
            }

            CreateIdValidation(appointment);
            DateValidation(appointment);
        }

        private void DateValidation(Appointment appointment)
        {
            if (appointment.AppointmentDateTime == DateTime.Parse("0001 - 01 - 01T00: 00:00"))
            {
                throw new ArgumentException("an appointment needs a dateTime");
            }
        }

        private void CreateIdValidation(Appointment appointment)
        {
            if (appointment.AppointmentId > 0)
            {
                throw new ArgumentException("A new appointment should not have an id");
            }
        }

        public void EditValidation(Appointment appointment)
        {
            throw new NotImplementedException();
        }

        public void IdValidation(int id)
        {
            throw new NotImplementedException();
        }
    }
}
