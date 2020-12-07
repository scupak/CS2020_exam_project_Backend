using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Services.Validators.Implementations;
using Core.Services.Validators.Interfaces;
using Xunit;
using FluentAssertions;

namespace Infrastructure.UnitTests.ValidatorTests
{
    public class AppointmentValidatorTest
    {

        [Fact]
        public void AppointmentValidator_ShouldBeOfTypeIAppointmentValidator()
        {
            new AppointmentValidator().Should().BeAssignableTo<IAppointmentValidator>();
        }

        [Fact]
        public void CreateValidation_WithAppointmentThatsNull_shouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(null as Appointment);
            action.Should().Throw<NullReferenceException>().WithMessage("Appointment cannot be null");
        }

        [Fact]
        public void CreateValidation_withAppointmentHasAnId_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentId = 1,
                AppointmentDateTime = DateTime.Now,
                Description = "my knee hurt",
                DoctorId = 1,
                DurationInMin = 15,
                PatientCpr = "150498-5522"
            });

            action.Should().Throw<ArgumentException>().WithMessage("An appointment should not have an id");
        }

        [Fact]
        public void CreateValidation_AppointmentWithNoDate_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                Description = "my knee hurt",
                DoctorId = 1,
                DurationInMin = 15,
                PatientCpr = "150498-5522"
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a dateTime");
        }

        [Fact]
        public void CreateValidation_AppointmentWIthNoDescription_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now,
                DoctorId = 1,
                DurationInMin = 15,
                PatientCpr = "150498-5522"
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a description");
        }

        [Fact]
        public void CreateValidation_AppointmentWithNoDoctorId_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now,
                Description = "my knee hurt",
                DurationInMin = 15,
                PatientCpr = "150498-5522"
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a doctor");
        }

        [Fact]
        public void CreateValidation_AppointmentWithNoDuration_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now,
                Description = "my knee hurt",
                DoctorId = 1,
                PatientCpr = "150498-5522"
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a duration");
        }

        [Fact]
        public void CreateValidation_AppointmentWIthNoPatient_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now,
                Description = "my knee hurt",
                DoctorId = 1,
                DurationInMin = 15
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a patient");
        }

        [Fact]
        public void CreationValidation_AppointmentHasToLongDescription_shouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now,
                Description =
                    "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohello" +
                    "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohello" +
                    "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohelloh",
                DoctorId = 1,
                DurationInMin = 15,
                PatientCpr = "150498-5522"
            });
            action.Should().Throw<ArgumentException>().WithMessage("description is to long");
        }


    }
}
