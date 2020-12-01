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
        public void CreateValidation_WithDoctorHasNoName_ShouldThrowException()
        {
            IDoctorValidator doctorValidator = new DoctorValidator();
            Action action = () => doctorValidator.CreateValidation(new Doctor(){});
            action.Should().Throw<ArgumentException>().WithMessage("a doctor needs a name");
        }
    }
}