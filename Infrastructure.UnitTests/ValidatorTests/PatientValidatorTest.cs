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
        public void  DefaultValidation_WithNullEmail_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "402040"} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient e-mail cannot be null or empty!");

           

        }


        [Fact]
        public void  DefaultValidation_WithNullCPR_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "402040" , PatientEmail = "hans@hotmail.com"} as Patient);

            action.Should().Throw<NullReferenceException>().WithMessage("Patient CPR cannot be null or empty!");

           

        }


        [Fact]
        public void  DefaultValidation_WithNormalCPRSouldThrowNoExeption()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "402040" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"} as Patient);

            action.Should().NotThrow<NullReferenceException>();

           

        }

        [Fact]
        public void  DefaultValidation_WithInvalidCPR_ShouldThrowException()
        {
            IPatientValidator validator = new PatientValidator();

            Action action = () => validator.DefaultValidator(new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "402040" , PatientEmail = "hans@hotmail.com" ,PatientCPR = "400429-0677"} as Patient);

            action.Should().Throw<InvalidDataException>().WithMessage("Patient CPR has to be a valid CPR number");

           

        }



    }
}
