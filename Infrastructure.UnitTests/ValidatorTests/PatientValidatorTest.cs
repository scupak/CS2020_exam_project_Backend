using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Services.Validators.Implementations;
using Core.Services.Validators.Interfaces;
using Xunit;
using FluentAssertions;

namespace Infrastructure.UnitTests.ValidatorTests
{
   public class PatientValidatorTest
    {
        [Fact]
        public void PatientValidator_IsOfTypeIPatientValidator()
        {
            new PatientValidator().Should().BeAssignableTo<IPatientValidator>();

        }

        [Fact]
        public void  DefaultValidation_WithNullPatient_ShouldThrowException()
        {
           IPatientValidator validator = new PatientValidator();

           Action action = () => validator.DefaultValidator(null as Patient);

           action.Should().Throw<NullReferenceException>().WithMessage("Patient cannot be null!");

           

        }

        [Fact]
        public void  DefaultValidation_WithNullFirstName_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = null} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient Firstname cannot be null or empty!");

           

        }
        [Fact]
        public void  DefaultValidation_WithNullLastName_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = null} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient Lastname cannot be null or empty!");

           

        }


        [Fact]
        public void  DefaultValidation_WithNullPhone_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = null} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient phone number cannot be null or empty!");

           

        }

        [Fact]
        public void  DefaultValidation_WithValidPhone_ShouldNotThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "23115177" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677" } as Patient);

            action.Should().NotThrow<Exception>();



        }

        [Theory]
        [InlineData("4020405")]
        [InlineData("40204055555555555")]
        [InlineData("0000000000")]
        [InlineData("9999999p")]
        public void  DefaultValidation_WithInvalidPhone_ShouldThrowException(string phoneNumber)
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = phoneNumber} as Patient);

            action.Should().Throw<InvalidDataException>().WithMessage("Patient Phone number has to be a valid Phone number");



        }

        [Fact]
        public void  DefaultValidation_WithNullEmail_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050"} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient e-mail cannot be null or empty!");

           

        }

        [Fact]
        public void  DefaultValidation_WithValidEmail_ShouldNotThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com"} as Patient);

            action.Should().NotThrow<InvalidDataException>();

           

        }

        [Theory]
        [InlineData("hanshotmail.com")]
        [InlineData("hans@@hotmail.com")]
        [InlineData("hanshotmai@.com")]
        [InlineData("hans@hot")]
        public void  DefaultValidation_WithInvalidEmail_ShouldThrowException(string email)
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" ,PatientEmail = email} as Patient);

            action.Should().Throw<InvalidDataException>().WithMessage("Patient Email has to be a valid Email");

           

        }


        [Fact]
        public void  DefaultValidation_WithNullCPR_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com"} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient CPR cannot be null or empty!");

           

        }


        [Fact]
        public void  DefaultValidation_WithNormalCPRSouldNotThrowExeption()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"} as Patient);

            action.Should().NotThrow<InvalidDataException>();

           

        }

        [Fact]
        public void  DefaultValidation_WithInvalidCPR_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" ,PatientCPR = "400429-0677"} as Patient);

            action.Should().Throw<InvalidDataException>().WithMessage("Patient CPR has to be a valid CPR number");

           

        }



    }
}
