using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
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
        private SortedDictionary<string, Patient> _allPatients;
        private Mock<IRepository<Patient, string>> _patientRepoMock;
        private Mock<IPatientValidator> _validatorMock;

        public PatientServiceTest()
        {
            _allPatients = new SortedDictionary<string, Patient>();

            _patientRepoMock = new Mock<IRepository<Patient, string>>();

            _validatorMock = new Mock<IPatientValidator>();


            _patientRepoMock
                .Setup(repo => repo
                    .Add(It.IsAny<Patient>()))
                .Callback<Patient>(patient => _allPatients
                    .Add(patient.PatientCPR, patient))
                .Returns<Patient>(patient => _allPatients[patient.PatientCPR]);

            _patientRepoMock
                .Setup(repo => repo
                    .Edit(It.IsAny<Patient>()))
                .Callback<Patient>(patient => _allPatients[patient.PatientCPR] = patient)
                .Returns<Patient>(patient => _allPatients[patient.PatientCPR]);

            _patientRepoMock
                .Setup(repo => repo
                    .Remove(It
                        .IsAny<string>()))
                .Callback<string>(id => _allPatients.Remove(id))
                .Returns<string>((id) => _allPatients
                    .ContainsKey(id) ? _allPatients[id] : null);

            _patientRepoMock
                .Setup(repo => repo
                    .GetAll(It.IsAny<Filter>()))
                .Returns<Filter>((filter) => new FilteredList<Patient>() { List = _allPatients.Values.ToList(), TotalCount = _allPatients.Count, FilterUsed = filter });


            _patientRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<string>()))
                .Returns<string>((CPR) => _allPatients
                    .ContainsKey(CPR)
                    ? _allPatients[CPR]
                    : null);

            _patientRepoMock
                .Setup(repo => repo
                    .Count())
                .Returns(() => _allPatients.Count);
        }

        [Fact]
        public void PatientService_ShouldBeOfTypeIService()
        {
            new PatientService(_patientRepoMock.Object, _validatorMock.Object).Should()
                .BeAssignableTo<IService<Patient, string>>();



        }

        #region Getall

        

        
        [Fact]
        public void GetallTest()
        {
            //arrange
            Patient p1 = new Patient() {PatientCPR = "011200-4106"};
            Patient p2 = new Patient() {PatientCPR = "011200-4107"};

            var patients = new List<Patient>() {p1, p2};
            Filter filter = new Filter();

            _allPatients.Add(p1.PatientCPR, p1);
            _allPatients.Add(p2.PatientCPR, p2);
            // the doctors in the repository
            var expected = new FilteredList<Patient>()
                { List = _allPatients.Values.ToList(), TotalCount = _allPatients.Count, FilterUsed = filter };

            expected.TotalCount = _allPatients.Count;
            IService<Patient, string> service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

            var result = service.GetAll(filter);

            // assert
            Assert.Equal(expected.List, result.List);
            _patientRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(pFilter => pFilter == filter)), Times.Once);
        }

        [Fact]
        public void GetallTestNegativPagging_ShouldThrowException()
        {
            //arrange
            Patient p1 = new Patient() { PatientCPR = "011200-4106" };
            Patient p2 = new Patient() { PatientCPR = "011200-4107" };

            var patients = new List<Patient>() { p1, p2 };
            Filter filter = new Filter() { CurrentPage = -1};

            _allPatients.Add(p1.PatientCPR, p1);
            _allPatients.Add(p2.PatientCPR, p2);
            // the doctors in the repository
            var expected = new FilteredList<Patient>()
                { List = _allPatients.Values.ToList(), TotalCount = _allPatients.Count, FilterUsed = filter };

            expected.TotalCount = _allPatients.Count;
            IService<Patient, string> service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

            Action action = () => service.GetAll(filter);

            // assert
            action.Should().Throw<InvalidDataException>()
                .WithMessage("current page and items pr page can't be negative");
            _patientRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(pFilter => pFilter == filter)), Times.Never);
        }

        #endregion

        #region GetPatientById

        [Fact]
        public void GetPatientById_PatientExists()
        {
            // arrange
            
            // company c1 exists in the Company repository
            var c1 = new Patient() {PatientCPR = "011200-4106"};
            _allPatients.Add(c1.PatientCPR, c1);

            var service = new PatientService(_patientRepoMock.Object,_validatorMock.Object);

            // act
            var result = service.GetById(c1.PatientCPR);

            // assert
            Assert.Equal(c1, result);
            _patientRepoMock.Verify(repo => repo.GetById(It.Is<string>(id => id == c1.PatientCPR)), Times.Once);
        }

        [Fact]
        public void GetPatientByIdShouldCallPatientValidatorCPRValidatorWithCPRParam_Once()
        {
            // arrange

            // company c1 exists in the Company repository
            var c1 = new Patient() { PatientCPR = "011200-4106" };
            _allPatients.Add(c1.PatientCPR, c1);

            var service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

            // act
            var result = service.GetById(c1.PatientCPR);

            _validatorMock.Verify(validator => validator.ValidateCPR(c1.PatientCPR), Times.Once);
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
           PatientService service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

           service.Add(Patient);

           // assert
           Assert.Contains(Patient, _allPatients.Values);
           _patientRepoMock.Verify(repo => repo.Add(It.Is<Patient>(c => c == Patient)), Times.Once);
       }

       [Fact]
       public void Add_ShouldCallPatientValidatorDefaultValidationWithPatientParam_Once()
       {
           IService<Patient, string> service = new PatientService(_patientRepoMock.Object,_validatorMock.Object);
            var patient = new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"};
            service.Add(patient);
            _validatorMock.Verify(validator => validator.DefaultValidator(patient), Times.Once);

       }


       [Fact]
       public void Add_PatientAlreadyInTheDatabase_ShouldThrowException()
       {
           //arrange
           IService<Patient, string> service = new PatientService(_patientRepoMock.Object,_validatorMock.Object);
           var patient = new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"};
           
            _allPatients.Add(patient.PatientCPR,patient);

            //act + assert
            Action action = () => service.Add(patient);
            action.Should().Throw<InvalidDataException>().WithMessage("Patient is already in the database");



       }


       #endregion


       #region Edit

       [Fact]
       public void Edit_ShouldCallPatientValidatorDefaultValidationWithPatientParam_Once()
       {
           IService<Patient, string> service = new PatientService(_patientRepoMock.Object,_validatorMock.Object);
           var patient = new Patient(){PatientFirstName = "name" , PatientLastName = "lastname", PatientPhone = "40204050" , PatientEmail = "hans@hotmail.com" , PatientCPR = "150429-0677"};
           _allPatients.Add(patient.PatientCPR,patient);
           service.Edit(patient);
           _validatorMock.Verify(validator => validator.DefaultValidator(patient), Times.Once);

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

            _allPatients.Add(PatientOld.PatientCPR,PatientOld);

           // act
           PatientService service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);
           service.Edit(Patientnew);

           // assert
           var updatedPatient = _allPatients[PatientCPR];

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
           PatientService service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

           Action action = () => service.Edit(Patient);

           action.Should().Throw<ArgumentException>().WithMessage("Patient is not in the database");

       }



        #endregion

        #region RemovePatientById
        [Fact]
        public void RemovePatientById()
        {
            // arrange

            // company c1 exists in the Company repository
            var c1 = new Patient() { PatientCPR = "011200-4106" };
            _allPatients.Add(c1.PatientCPR, c1);

            var service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

            // act
            var result = service.Remove(c1.PatientCPR);

            _validatorMock.Verify(validator => validator.ValidateCPR(c1.PatientCPR), Times.Once);
        }

        [Fact]
        public void RemovePatientById_VerifyRemoved()
        {
            // arrange
            

            // company c1 exists in the Company repository
            var c1 = new Patient() { PatientCPR = "011200-4106" };
            _allPatients.Add(c1.PatientCPR, c1);

            var service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

            // act
            var result = service.Remove(c1.PatientCPR);

            Assert.Null(_patientRepoMock.Object.GetById(c1.PatientCPR));
            _patientRepoMock.Verify(repo => repo.Remove(It.Is<string>(c => c == c1.PatientCPR )),Times.Once());

        }

        [Fact]
        public void RemoveNonexistantPatient_ShouldThrowException()
        {
            // arrange

            // company c1 exists in the Company repository
            var c1 = new Patient() { PatientCPR = "011200-4106" };

            var service = new PatientService(_patientRepoMock.Object, _validatorMock.Object);

            // act
            Action action = () => service.Remove(c1.PatientCPR);

            action.Should().Throw<ArgumentException>().WithMessage("Nonexistant patient cannot be removed!");
        }

        #endregion
    }
}
