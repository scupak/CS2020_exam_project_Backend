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
        public void CreateValidation_withValidAppointment_ShouldNotThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Description = "my knee hurt",
                DurationInMin = 15
            });

            action.Should().NotThrow<Exception>();
        }


        [Fact]
        public void CreateValidation_withAppointmentHasAnId_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentId = 1,
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Description = "my knee hurt",
                DurationInMin = 15
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
                DurationInMin = 15
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a dateTime");
        }

        [Fact]
        public void CreateValidation_AppointmentWithNoDuration_ShouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Description = "my knee hurt"
            });
            action.Should().Throw<ArgumentException>().WithMessage("an appointment needs a duration");
        }

        [Fact]
        public void CreationValidation_AppointmentHasToLongDescription_shouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Description =
                    "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohello" +
                    "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohello" +
                    "hellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohellohelloh",
                DurationInMin = 15
            });
            action.Should().Throw<ArgumentException>().WithMessage("description is to long");
        }

        [Fact]
        public void CreationValidation_AppointmentExpiredDate_shouldThrowException()
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(-1),
                Description = "my knee hurt",
                DurationInMin = 15
            });
            action.Should().Throw<ArgumentException>().WithMessage("The date it invalid, you cant set an appointment in the past");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreationValidation_AppointmentNegativeDuration_shouldThrowException(int duration)
        {
            IAppointmentValidator appointmentValidator = new AppointmentValidator();
            Action action = () => appointmentValidator.CreateValidation(new Appointment()
            {
                AppointmentDateTime = DateTime.Now.AddDays(1),
                Description = "my knee hurt",
                DurationInMin = 0
            });
            action.Should().Throw<ArgumentException>().WithMessage("The date it invalid, you cant set an appointment in the past");
        }


    }
}
