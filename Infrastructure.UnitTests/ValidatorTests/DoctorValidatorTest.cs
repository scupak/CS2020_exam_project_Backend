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
        public void CreateValidation_WithDoctorThatsNull_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(null as Doctor);
            action.Should().Throw<NullReferenceException>().WithMessage("Doctor cannot be null");
        }

        [Fact]
        public void CreateValidation_WithDoctorHasAnId_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                DoctorId = 1,
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("A doctor should not have an id");
        }

        [Fact]
        public void CreateValidation_WithDoctorHasNoFirstName_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a first name");
        }

        [Fact]
        public void CreateValidation_WithDoctorHasNoLastName_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = "Mads",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a last name");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("t")]
        public void CreateValidation_WithDoctorFirstNameMin_ShouldThrowException(string firstName)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = firstName,
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a first name");
        }

        [Theory]
        [InlineData("")]
        [InlineData("t")]
        public void CreateValidation_WithDoctorLastNameMin_ShouldThrowException(string lastName)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = lastName,
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a last name");
        }

        [Fact]
        public void CreateValidation_WithDoctorHasNoEmail_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs an email");
        }

        [Theory]
        [InlineData("lumby98gmail.com")]
        [InlineData("lumby98@gmailcom")]
        [InlineData("lumby98@gmail.co.uk")]
        [InlineData("")]
        public void CreateValidation_WithDoctorHasNoValidEmail_ShouldThrowException(string email)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = email,
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a valid email address");
        }

        [Fact]
        public void CreateValidation_WithDoctorHasNoPhoneNumber_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a phone number");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("")]
        [InlineData("235689562014")]
        public void CreateValidation_WithDoctorHasNoValidPhoneNumber_ShouldThrowException(string phoneNumber)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = phoneNumber
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a phone number");
        }

        [Fact]
        public void EditValidation_WithDoctorHasNoId_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("A doctor should have an id");
        }

        [Fact]
        public void EditValidation_WithDoctorHasNoFirstName_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a first name");
        }

        [Fact]
        public void EditValidation_WithDoctorHasNoLastName_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a last name");
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("t")]
        public void EditValidation_WithDoctorFirstNameMin_ShouldThrowException(string firstName)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = firstName,
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a first name");
        }

        [Theory]
        [InlineData("")]
        [InlineData("t")]
        public void EditValidation_WithDoctorLastNameMin_ShouldThrowException(string lastName)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = lastName,
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a last name");
        }

        [Fact]
        public void EditValidation_WithDoctorHasNoEmail_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs an email");
        }

        [Theory]
        [InlineData("lumby98gmail.com")]
        [InlineData("lumby98@gmailcom")]
        [InlineData("lumby98@gmail.co.uk")]
        [InlineData("")]
        public void EditValidation_WithDoctorHasNoValidEmail_ShouldThrowException(string email)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = email,
                PhoneNumber = "11554477",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a valid email address");
        }

        [Fact]
        public void EditValidation_WithDoctorHasNoPhoneNumber_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a phone number");
        }

        [Theory]
        [InlineData("1")]
        [InlineData("")]
        [InlineData("235689562014")]
        public void EditValidation_WithDoctorHasNoValidPhoneNumber_ShouldThrowException(string phoneNumber)
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.EditValidation(new Doctor()
            {
                FirstName = "Mads",
                LastName = "Lumby",
                EmailAddress = "lumby98@gmail.com",
                PhoneNumber = phoneNumber
            });
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a phone number");
        }

        [Fact]
        public void IdValidation_WithNoValidId_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.IdValidation(0);
            action.Should().Throw<ArgumentException>().WithMessage("id cannot be lower than 1");
        }
    }
}