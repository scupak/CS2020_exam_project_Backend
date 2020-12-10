using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
using Core.Services.ApplicationServices.Implementations;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Implementations;
using Core.Services.Validators.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Infrastructure.UnitTests.ServiceTests
{
    public class DoctorServiceTest
    {
        private readonly Mock<IRepository<Doctor, string>> _doctorRepoMock;
        private readonly Mock<IDoctorValidator> _doctorValidatorMock;

        private SortedDictionary<string, Doctor> _allDoctors;

        public DoctorServiceTest()
        {

            _allDoctors = new SortedDictionary<string, Doctor>();
            _doctorRepoMock = new Mock<IRepository<Doctor, string>>();
            _doctorValidatorMock = new Mock<IDoctorValidator>();
            
            _doctorRepoMock
                .Setup(repo => repo
                    .Add(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor => _allDoctors
                    .Add(doctor.DoctorEmailAddress, doctor))
                .Returns<Doctor>(doctor => _allDoctors[doctor.DoctorEmailAddress]);

            _doctorRepoMock
                .Setup(repo => repo
                    .Edit(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor => _allDoctors[doctor.DoctorEmailAddress] = doctor)
                .Returns<Doctor>(doctor => _allDoctors[doctor.DoctorEmailAddress]);

            _doctorRepoMock
                .Setup(repo => repo
                    .Remove(It
                        .IsAny<string>()))
                .Callback<string>(email => _allDoctors.Remove(email))
                .Returns<string>((email) => _allDoctors.ContainsKey(email) ? _allDoctors[email] : null);

            _doctorRepoMock
                .Setup(repo => repo
                    .GetAll(It.IsAny<Filter>()))
                .Returns<Filter>((filter) => new FilteredList<Doctor>() { List = _allDoctors.Values.ToList(), TotalCount = _allDoctors.Count, FilterUsed = filter });

            _doctorRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<string>()))
                .Returns<string>((email) => _allDoctors
                    .ContainsKey(email) ? _allDoctors[email] : null);

            _doctorRepoMock
                .Setup(repo => repo
                    .Count())
                .Returns(() => _allDoctors.Count);
        }

        [Fact]
        public void CreateDoctorService_ValidCompanyRepository()
        {
            // arrange
            IRepository<Doctor, string> repo = _doctorRepoMock.Object;
            IDoctorValidator validator = _doctorValidatorMock.Object;

            // act
            IService<Doctor, string> service = new DoctorService(repo, validator);

            // assert
            Assert.NotNull(service);
        }

        [Fact]
        public void DoctorService_IsOfTypeIService()
        {
            
            IRepository<Doctor, string> repo = _doctorRepoMock.Object;
            IDoctorValidator validator = _doctorValidatorMock.Object;

            new DoctorService(repo, validator).Should().BeAssignableTo<IService<Doctor, string>>();
        }

        #region GetAll

        [Fact]
        public void GetAllDoctors()
        {
            //arrange
            Doctor d1 = new Doctor(){DoctorEmailAddress = "lumby98@gmail.com"};
            Doctor d2 = new Doctor(){DoctorEmailAddress = "michael@hotmail.com"};
            var doctors = new List<Doctor>() { d1, d2};
            Filter filter = new Filter();

            _allDoctors.Add(d1.DoctorEmailAddress, d1);
            _allDoctors.Add(d2.DoctorEmailAddress, d2);


            // the doctors in the repository
            var expected = new FilteredList<Doctor>()
                { List = _allDoctors.Values.ToList(), TotalCount = _allDoctors.Count, FilterUsed = filter };

            expected.TotalCount = _allDoctors.Count;

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            var result = service.GetAll(filter);

            // assert
            Assert.Equal(expected.List, result.List);
            _doctorRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(dFilter => dFilter == filter)), Times.Once);

        }

        [Fact]
        public void GetAllDoctorsNegativPagginh_ShoudlThrowException()
        {
            //arrange
            Doctor d1 = new Doctor() { DoctorEmailAddress = "lumby98@gmail.com" };
            Doctor d2 = new Doctor() { DoctorEmailAddress = "michael@hotmail.com" };
            var doctors = new List<Doctor>() { d1, d2 };
            Filter filter = new Filter() {CurrentPage = -1};

            _allDoctors.Add(d1.DoctorEmailAddress, d1);
            _allDoctors.Add(d2.DoctorEmailAddress, d2);


            // the doctors in the repository
            var expected = new FilteredList<Doctor>()
                { List = _allDoctors.Values.ToList(), TotalCount = _allDoctors.Count, FilterUsed = filter };

            expected.TotalCount = _allDoctors.Count;

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.GetAll(filter);

            // assert
            action.Should().Throw<InvalidDataException>().WithMessage("current page and items pr page can't be negative");
            _doctorRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(dFilter => dFilter == filter)), Times.Never);

        }



        #endregion

        #region get by id

        [Fact]
        public void GetById_WithValidId_ShouldNotThrowException()
        {
            // arrange
            var d = new Doctor() {DoctorEmailAddress = "lumby98@gmail.com"};
            _allDoctors.Add(d.DoctorEmailAddress, d);

            IService<Doctor, string> service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            var result = service.GetById(d.DoctorEmailAddress);

            Assert.Equal(d, result);

            _doctorRepoMock.Verify(repo => repo
                .GetById(It.Is<string>(id => id == d.DoctorEmailAddress)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .ValidateEmail(It.Is<string>(id => id == d.DoctorEmailAddress)), Times.Once);
        }

        [Fact]
        public void GetById_DoctorDoesNotExist_shouldThrowException()
        {
            // arrange
            var d1 = new Doctor(){ DoctorEmailAddress = "lumby98@gmail.com"};
            var d2 = new Doctor(){ DoctorEmailAddress = "lumby56@hotmail.com"};

            // only d2 exists in the doctor repository
            _allDoctors.Add(d2.DoctorEmailAddress, d2);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.GetById(d1.DoctorEmailAddress);

            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("Doctor does not exist");
            
            _doctorRepoMock.Verify(repo => repo
                .GetById(It.Is<string>(id => id == d1.DoctorEmailAddress)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .ValidateEmail(It.Is<string>(id => id == d1.DoctorEmailAddress)), Times.Once);
        }


        #endregion

        #region Add

        [Theory]
        [InlineData("Karl", "Mason", "doctor@gmail.com", "23115177", true)]
        [InlineData("Peter", "Holt", "Porter@hotmail.dk", "12345678", false)]
        [InlineData("Sandra", "Bullock", "SB@Yahoo.uk", "09876543", false)]
        public void Add_WithValidDoctor_shouldNotThrowException( string firstname, string lastname, string emailAddress, string phoneNumber, bool isAdmin)
        {
            // arrange
            var d1 = new Doctor() { DoctorEmailAddress = emailAddress, FirstName = firstname, LastName = lastname, PhoneNumber = phoneNumber, IsAdmin = isAdmin};

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Add(d1);
            // assert
            action.Should().NotThrow<Exception>();
            Assert.Contains(d1, _allDoctors.Values);

            _doctorRepoMock.Verify(repo => repo
                .Add(It.Is<Doctor>(doctor => doctor == d1)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .DefaultValidator(It.Is<Doctor>(doctor => doctor == d1)), Times.Once);
        }

        #endregion

        #region Edit

        [Theory]
        [InlineData("Karl", "Mason", "email@gmail.com", "23115177", true)]
        [InlineData("Peter", "Holt", "email@gmail.com", "12345678", false)]
        [InlineData("Sandra", "Bullock", "email@gmail.com", "09876543", false)]
        public void Edit_WithValidDoctor_shouldNotThrowException(string firstname, string lastname, string emailAddress, string phoneNumber, bool isAdmin)
        {
            // arrange
            var dNew = new Doctor() { DoctorEmailAddress = emailAddress, FirstName = firstname, LastName = lastname, PhoneNumber = phoneNumber, IsAdmin = isAdmin };
            var dOld = new Doctor() { DoctorEmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };

            _allDoctors.Add(dOld.DoctorEmailAddress, dOld);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Edit(dNew);
            // assert
            action.Should().NotThrow<Exception>();
            Assert.Equal(_doctorRepoMock.Object.GetById(dNew.DoctorEmailAddress), dNew);

            _doctorRepoMock.Verify(repo => repo
                .Edit(It.Is<Doctor>(doctor => doctor == dNew)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .DefaultValidator(It.Is<Doctor>(doctor => doctor == dNew)), Times.Once);
        }

        [Fact]
        public void Edit_WithInValidDoctor_shouldThrowException()
        {
            // arrange
            var dOld = new Doctor() { DoctorEmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };
            var dNew = new Doctor() { DoctorEmailAddress = "emai@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };
            _allDoctors.Add(dOld.DoctorEmailAddress, dOld);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Edit(dNew);
            // assert
            action.Should().Throw<ArgumentException>().WithMessage("A doctor with this email does not exist");

            _doctorRepoMock.Verify(repo => repo
                .Edit(It.Is<Doctor>(doctor => doctor == dNew)), Times.Never);

            _doctorValidatorMock.Verify(validator => validator
                .DefaultValidator(It.Is<Doctor>(doctor => doctor == dNew)), Times.Once);
        }


        #endregion

        #region Remove
        [Fact]
        public void Remove_WithValidDoctor_shouldNotThrowException()
        {
            // arrange
            var dOld = new Doctor() { DoctorEmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };

            _allDoctors.Add(dOld.DoctorEmailAddress, dOld);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Remove(dOld.DoctorEmailAddress);
            // assert
            action.Should().NotThrow<Exception>();
            Assert.Null(_doctorRepoMock.Object.GetById(dOld.DoctorEmailAddress));

            _doctorRepoMock.Verify(repo => repo
                .Remove(It.Is<string>(id => id == dOld.DoctorEmailAddress)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .ValidateEmail(It.Is<string>(id => id == dOld.DoctorEmailAddress)), Times.Once);
        }

        [Fact]
        public void Remove_WithNonExistingDoctor_shouldThrowException()
        {
            // arrange
            var dOld = new Doctor() { DoctorEmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Remove(dOld.DoctorEmailAddress);
            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("This doctor does not exist");

            _doctorRepoMock.Verify(repo => repo
                .Remove(It.Is<string>(id => id == dOld.DoctorEmailAddress)), Times.Never);

            _doctorValidatorMock.Verify(validator => validator
                .ValidateEmail(It.Is<string>(email => email == dOld.DoctorEmailAddress)), Times.Once);
        }


        #endregion
    }
}