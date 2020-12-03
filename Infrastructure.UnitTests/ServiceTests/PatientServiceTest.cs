using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Implementations;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Interfaces;
using Xunit;
using FluentAssertions;
using Moq;
using Xunit.Sdk;

namespace Infrastructure.UnitTests.ServiceTests
{

    public class PatientServiceTest
    {
        private SortedDictionary<string, Patient> allPatients;
        private Mock<IRepository<Patient, string>> patientRepoMock;
        private Mock<IPatientValidator> validatorMock;

        public PatientServiceTest()
        {
            allPatients = new SortedDictionary<string, Patient>();

            patientRepoMock = new Mock<IRepository<Patient, string>>();

            validatorMock = new Mock<IPatientValidator>();


            patientRepoMock
                .Setup(repo => repo
                    .Add(It.IsAny<Patient>()))
                .Callback<Patient>(patient => allPatients
                    .Add(patient.PatientCPR, patient))
                .Returns<Patient>(patient => allPatients[patient.PatientCPR]);

            patientRepoMock
                .Setup(repo => repo
                    .Edit(It.IsAny<Patient>()))
                .Callback<Patient>(patient => allPatients[patient.PatientCPR] = patient)
                .Returns<Patient>(patient => allPatients[patient.PatientCPR]);

            patientRepoMock
                .Setup(repo => repo
                    .Remove(It
                        .IsAny<string>()))
                .Callback<string>(CPR => allPatients.Remove(CPR))
                .Returns<Patient>(patient => allPatients[patient.PatientCPR]);

            patientRepoMock
                .Setup(repo => repo
                    .GetAll())
                .Returns(() => allPatients.Values.ToList());

            patientRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<string>()))
                .Returns<string>((CPR) => allPatients
                    .ContainsKey(CPR)
                    ? allPatients[CPR]
                    : null);

        }

        [Fact]
        public void PatientService_ShouldBeOfTypeIService()
        {
            new PatientService(patientRepoMock.Object, validatorMock.Object).Should()
                .BeAssignableTo<IService<Patient, string>>();



        }

        #region Getall

        

        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetallTest(int patientCount)
        {
            //arrange
            Patient p1 = new Patient() {PatientCPR = "011200-4106"};
            Patient p2 = new Patient() {PatientCPR = "011200-4107"};

            var patients = new List<Patient>() {p1, p2};

            var expected = patients.GetRange(0, patientCount);

            foreach (var p in expected)
            {
                allPatients.Add(p.PatientCPR, p);
            }

            IService<Patient, string> service = new PatientService(patientRepoMock.Object, validatorMock.Object);

            var result = service.GetAll();

            // assert
            Assert.Equal(expected, result);
            patientRepoMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        #endregion

        #region GetPatientById

        [Fact]
        public void GetPatientById_PatientExists()
        {
            // arrange
            
            // company c1 exists in the Company repository
            var c1 = new Patient() {PatientCPR = "011200-4106"};
            allPatients.Add(c1.PatientCPR, c1);

            var service = new PatientService(patientRepoMock.Object,validatorMock.Object);

            // act
            var result = service.GetById(c1.PatientCPR);

            // assert
            Assert.Equal(c1, result);
            patientRepoMock.Verify(repo => repo.GetById(It.Is<string>(id => id == c1.PatientCPR)), Times.Once);
        }

       #endregion


       #region Add

       [Theory]
       [InlineData("011200-4106" ,"mike" , "mikeowsky", "mike@hotmail.com" , "40506090" )]
       public void AddPatient_ValidPatient(string PatientCPR ,string FirstName , string Lastname , string Email , string phone)
       {
           // arrange
           var Patient = new Patient()
           {
               PatientCPR = PatientCPR, 
               PatientEmail = Email, 
               PatientLastName = Lastname, 
               PatientPhone = phone, 
               PatientFirstName = FirstName



           };

           // act
           PatientService service = new PatientService(patientRepoMock.Object, validatorMock.Object);

           service.Add(Patient);

           // assert
           Assert.Contains(Patient, allPatients.Values);
           patientRepoMock.Verify(repo => repo.Add(It.Is<Patient>(c => c == Patient)), Times.Once);
       }

       [Fact]
       public void Add_ShouldCallPatientValidatorDefaultValidationWithPatientParam_Once()
       {
           IService<Patient, string> service = new PatientService(patientRepoMock.Object,validatorMock.Object);
            var patient = new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"};
            service.Add(patient);
            validatorMock.Verify(validator => validator.DefaultValidator(patient), Times.Once);

       }


       [Fact]
       public void Add_PatientAlreadyInTheDatabase_ShouldThrowException()
       {
           //arrange
           IService<Patient, string> service = new PatientService(patientRepoMock.Object,validatorMock.Object);
           var patient = new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"};
           
            allPatients.Add(patient.PatientCPR,patient);

            //act + assert
            Action action = () => service.Add(patient);
            action.Should().Throw<InvalidDataException>().WithMessage("Patient is already in the database");



       }


       #endregion


       #region Edit

       [Fact]
       public void Edit_ShouldCallPatientValidatorDefaultValidationWithPatientParam_Once()
       {
           IService<Patient, string> service = new PatientService(patientRepoMock.Object,validatorMock.Object);
           var patient = new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"};
           service.Edit(patient);
           validatorMock.Verify(validator => validator.DefaultValidator(patient), Times.Once);

       }

       [Theory]
       [InlineData("011200-4106" ,"mike" , "mikeowsky", "mike@hotmail.com" , "40506090" )]
       public void EditPatient_ValidPatientWithPatientInDatabse(string PatientCPR ,string FirstName , string Lastname , string Email , string phone)
       {
           // arrange
           var PatientOld = new Patient()
           {
               PatientCPR = PatientCPR, 
               PatientEmail = "jake@hotmail.com", 
               PatientLastName = "jakeowsky", 
               PatientPhone = "20201090", 
               PatientFirstName = "jake"



           };
           var Patientnew = new Patient()
           {
               PatientCPR = PatientCPR, 
               PatientEmail = Email, 
               PatientLastName = Lastname, 
               PatientPhone = phone, 
               PatientFirstName = FirstName



           };

            allPatients.Add(PatientOld.PatientCPR,PatientOld);

           // act
           PatientService service = new PatientService(patientRepoMock.Object, validatorMock.Object);
           service.Edit(Patientnew);

           // assert
           var updatedPatient = allPatients[PatientCPR];

           updatedPatient.Should().Be(Patientnew);

       }


       [Fact]
       public void EditPatient_WithPatientNotInTheDatabase_ShouldThrowException()
       {
           // arrange
           var Patient = new Patient()
           {
               PatientCPR = "011200-4106", 
               PatientEmail = "jake@hotmail.com", 
               PatientLastName = "jakeowsky", 
               PatientPhone = "20201090", 
               PatientFirstName = "jake"



           };

           // act + assert
           PatientService service = new PatientService(patientRepoMock.Object, validatorMock.Object);

           Action action = () => service.Edit(Patient);

           action.Should().Throw<KeyNotFoundException>().WithMessage("Patient is not in the database");

       }



       #endregion
    }
}
