using System;
using Core.Entities.Entities.BE;
using Core.Services.Validators.Implementations;
using Core.Services.Validators.Interfaces;
using Xunit;
using FluentAssertions;

namespace Infrastructure.UnitTests.ValidatorTests
{
    public class DoctorValidatorTest
    {
        [Fact]
        public void DoctorValidator_ShouldBeOfTypeIDoctorValidator()
        {
            new DoctorValidator().Should().BeAssignableTo<IDoctorValidator>();
        }

        [Fact]
        public void DefaultValidator_WithDoctorThatsNull_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(null as Doctor);
            action.Should().Throw<NullReferenceException>().WithMessage("Doctor cannot be null");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("t")]
        [InlineData(null)]
        public void DefaultValidator_WithDoctorInvalidFirstName_ShouldThrowException(string firstName)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = firstName,
                LastName = "Lumby",
                DoctorEmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a valid first name");
        }

        [Theory]
        [InlineData("")]
        [InlineData("t")]
        [InlineData(null)]
        public void DefaultValidator_WithDoctorInvalidLastName_ShouldThrowException(string lastName)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = lastName,
                DoctorEmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a valid last name");
        }

        [Fact]
        public void DefaultValidator_WithDoctorHasNoEmail_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<NullReferenceException>().WithMessage("a doctor needs an email");
        }

        [Theory]
        [InlineData("lumby98gmail.com")]
        [InlineData("lumby98@gmailcom")]
        [InlineData("lumby98@gmail.co.uk")]
        [InlineData("")]
        public void DefaultValidator_WithDoctorHasNoValidEmail_ShouldThrowException(string email)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                DoctorEmailAddress = email,
                PhoneNumber = "23115177",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a valid email address");
        }

        [Fact]
        public void DefaultValidator_WithDoctorHasValidEmail_ShouldNotThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                DoctorEmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().NotThrow<Exception>();
        }

        [Fact]
        public void DefaultValidator_WithDoctorHasNoPhoneNumber_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                DoctorEmailAddress = "lumby98@gmail.com",
            });
            action.Should().Throw<NullReferenceException>().WithMessage("a doctor needs a phone number");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("")]
        [InlineData("235689562014")]
        [InlineData("23-11-51-77")]
        public void DefaultValidator_WithDoctorHasNoValidPhoneNumber_ShouldThrowException(string phoneNumber)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                DoctorEmailAddress = "lumby98@gmail.com",
                PhoneNumber = phoneNumber
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a valid phone number");
        }

        [Theory]
        [InlineData("23115177")]
        [InlineData("23 11 51 77")]
        public void DefaultValidator_WithDoctorHasValidPhoneNumber_ShouldNotThrowException(string phoneNumber)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.DefaultValidator(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                DoctorEmailAddress = "lumby98@gmail.com",
                PhoneNumber = phoneNumber
            });
            action.Should().NotThrow<Exception>();
        }

        [Theory]
        [InlineData("lumby98gmail.com")]
        [InlineData("lumby98@gmailcom")]
        [InlineData("lumby98@gmail.co.uk")]
        [InlineData("")]
        public void EmailValidation_WithNoValidId_ShouldThrowException(string email)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.ValidateEmail(email);
            action.Should().Throw<ArgumentException>().WithMessage("This is not a valid email address");
        }

    }
}