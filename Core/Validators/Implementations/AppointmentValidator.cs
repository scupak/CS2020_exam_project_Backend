using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            DurationValidator(appointment);
            DescriptionValidator(appointment);
            EmailValidator(appointment);

        }

        private void EmailValidator(Appointment appointment)
        {
            if (appointment.DoctorEmailAddress == null)
            {
                throw new ArgumentException("Appointments needs a doctor");
            }

        }

        private void DescriptionValidator(Appointment appointment)
        {
            if (appointment.Description != null)
            {

                if (appointment.Description.Length > 280)
                {
                    throw new ArgumentException("description is too long");
                }
            }
        }

        private void DurationValidator(Appointment appointment)
        {
            if (appointment.DurationInMin < 1)
            {
                throw new ArgumentException("an appointment needs a duration");
            }

            if (appointment.DurationInMin > 1440)
            {
                throw new ArgumentException("The duration cannot be longer than one day");
            }
        }

        private void DateValidation(Appointment appointment)
        {
            if (appointment.AppointmentDateTime == DateTime.Parse("0001 - 01 - 01T00: 00:00"))
            {
                throw new ArgumentException("an appointment needs a dateTime");
            }

            if (appointment.AppointmentDateTime.CompareTo(DateTime.Now) == -1)
            {
                throw new ArgumentException("The date is invalid, you cant set an appointment in the past");
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
            if (appointment == null)
            {
                throw new NullReferenceException("Appointment cannot be null");
            }

            EditIdValidation(appointment);
            DateValidation(appointment);
            DurationValidator(appointment);
            DescriptionValidator(appointment);
            EmailValidator(appointment);
        }

        private void EditIdValidation(Appointment appointment)
        {
            if (appointment.AppointmentId < 1)
            {
                throw new ArgumentException("When updating an appointment you need an id");
            }
        }

        public void IdValidation(int id)
        {
            if (id < 1)
            {
                throw new ArgumentException("Id cannot be negative");
            }
        }
    }
}
