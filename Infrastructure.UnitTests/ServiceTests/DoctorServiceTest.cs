using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Entities.Entities.BE;
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
        private readonly Mock<IRepository<Doctor, int>> _doctorRepoMock;
        private readonly Mock<IDoctorValidator> _doctorValidatorMock;

        private SortedDictionary<int, Doctor> _allDoctors;

        public DoctorServiceTest()
        {

            _allDoctors = new SortedDictionary<int, Doctor>();
            _doctorRepoMock = new Mock<IRepository<Doctor, int>>();
            _doctorValidatorMock = new Mock<IDoctorValidator>();
            
            _doctorRepoMock
                .Setup(repo => repo
                    .Add(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor => _allDoctors
                    .Add(doctor.DoctorId, doctor))
                .Returns<Doctor>(doctor => _allDoctors[doctor.DoctorId]);

            _doctorRepoMock
                .Setup(repo => repo
                    .Edit(It.IsAny<Doctor>()))
                .Callback<Doctor>(doctor => _allDoctors[doctor.DoctorId] = doctor)
                .Returns<Doctor>(doctor => _allDoctors[doctor.DoctorId]);

            _doctorRepoMock
                .Setup(repo => repo
                    .Remove(It
                        .IsAny<int>()))
                .Callback<int>(id => _allDoctors.Remove(id))
                .Returns<int>((id) => _allDoctors
                    .ContainsKey(id) ? _allDoctors[id] : null);

            _doctorRepoMock
                .Setup(repo => repo
                    .GetAll())
                .Returns(() => _allDoctors.Values.ToList());

            _doctorRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<int>()))
                .Returns<int>((id) => _allDoctors
                    .ContainsKey(id) ? _allDoctors[id] : null);
        }

        [Fact]
        public void CreateDoctorService_ValidCompanyRepository()
        {
            // arrange
            IRepository<Doctor, int> repo = _doctorRepoMock.Object;
            IDoctorValidator validator = _doctorValidatorMock.Object;

            // act
            IService<Doctor, int> service = new DoctorService(repo, validator);

            // assert
            Assert.NotNull(service);
        }

        [Fact]
        public void DoctorService_IsOfTypeIService()
        {
            
            IRepository<Doctor, int> repo = _doctorRepoMock.Object;
            IDoctorValidator validator = _doctorValidatorMock.Object;

            new DoctorService(repo, validator).Should().BeAssignableTo<IService<Doctor, int>>();
        }

        #region GetAll

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllDoctors(int doctorCount)
        {
            //arrange
            Doctor d1 = new Doctor() {DoctorId = 1};
            Doctor d2 = new Doctor(){DoctorId =  2};
            var doctors = new List<Doctor>() { d1, d2};

            // the doctors in the repository
            var expected = doctors.GetRange(0, doctorCount);
            foreach (var d in expected)
            {
                _allDoctors.Add(d.DoctorId, d);
            }

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            var result = service.GetAll();

            // assert
            Assert.Equal(expected, result);
            _doctorRepoMock.Verify(repo => repo.GetAll(), Times.Once);

        }

        #endregion

        #region get by id

        [Fact]
        public void GetById_WithValidId_ShouldNotThrowException()
        {
            // arrange
            var d = new Doctor() {DoctorId = 1};
            _allDoctors.Add(d.DoctorId, d);

            IService<Doctor, int> service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            var result = service.GetById(d.DoctorId);

            Assert.Equal(d, result);

            _doctorRepoMock.Verify(repo => repo
                .GetById(It.Is<int>(id => id == d.DoctorId)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(id => id == d.DoctorId)), Times.Once);
        }

        [Fact]
        public void GetById_DoctorDoesNotExist_shouldThrowException()
        {
            // arrange
            var d1 = new Doctor(){ DoctorId = 1};
            var d2 = new Doctor(){ DoctorId = 2};

            // only d2 exists in the doctor repository
            _allDoctors.Add(d2.DoctorId, d2);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.GetById(d1.DoctorId);

            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("Doctor does not exist");
            
            _doctorRepoMock.Verify(repo => repo
                .GetById(It.Is<int>(id => id == d1.DoctorId)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(id => id == d1.DoctorId)), Times.Once);
        }


        #endregion

        #region Add

        [Theory]
        [InlineData(1, "Karl", "Mason", "doctor@gmail.com", "23115177", true)]
        [InlineData(4, "Peter", "Holt", "Porter@hotmail.dk", "12345678", false)]
        [InlineData(10, "Sandra", "Bullock", "SB@Yahoo.uk", "09876543", null)]
        public void Add_WithValidDoctor_shouldNotThrowException(int id, string firstname, string lastname, string emailAddress, string phoneNumber, bool isAdmin)
        {
            // arrange
            var d1 = new Doctor() { DoctorId = id, EmailAddress = emailAddress, FirstName = firstname, LastName = lastname, PhoneNumber = phoneNumber, IsAdmin = isAdmin};

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Add(d1);
            // assert
            action.Should().NotThrow<Exception>();
            Assert.Contains(d1, _allDoctors.Values);

            _doctorRepoMock.Verify(repo => repo
                .Add(It.Is<Doctor>(doctor => doctor == d1)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .CreateValidation(It.Is<Doctor>(doctor => doctor == d1)), Times.Once);
        }

        #endregion

        #region Edit

        [Theory]
        [InlineData(1, "Karl", "Mason", "doctor@gmail.com", "23115177", true)]
        [InlineData(4, "Peter", "Holt", "Porter@hotmail.dk", "12345678", false)]
        [InlineData(10, "Sandra", "Bullock", "SB@Yahoo.uk", "09876543", null)]
        public void Edit_WithValidDoctor_shouldNotThrowException(int id, string firstname, string lastname, string emailAddress, string phoneNumber, bool isAdmin)
        {
            // arrange
            var dNew = new Doctor() { DoctorId = id, EmailAddress = emailAddress, FirstName = firstname, LastName = lastname, PhoneNumber = phoneNumber, IsAdmin = isAdmin };
            var dOld = new Doctor() { DoctorId = id, EmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };

            _allDoctors.Add(dOld.DoctorId, dOld);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Edit(dNew);
            // assert
            action.Should().NotThrow<Exception>();
            Assert.Equal(_doctorRepoMock.Object.GetById(dNew.DoctorId), dNew);

            _doctorRepoMock.Verify(repo => repo
                .Edit(It.Is<Doctor>(doctor => doctor == dNew)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .EditValidation(It.Is<Doctor>(doctor => doctor == dNew)), Times.Once);
        }

        [Fact]
        public void Edit_WithInValidDoctor_shouldThrowException()
        {
            // arrange
            var dOld = new Doctor() { DoctorId = 1, EmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };
            var dNew = new Doctor() { DoctorId = 2, EmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };
            _allDoctors.Add(dOld.DoctorId, dOld);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Edit(dNew);
            // assert
            action.Should().Throw<ArgumentException>().WithMessage("A doctor with this id does not exist");

            _doctorRepoMock.Verify(repo => repo
                .Edit(It.Is<Doctor>(doctor => doctor == dNew)), Times.Never);

            _doctorValidatorMock.Verify(validator => validator
                .EditValidation(It.Is<Doctor>(doctor => doctor == dNew)), Times.Once);
        }


        #endregion

        #region Remove
        [Fact]
        public void Remove_WithValidDoctor_shouldNotThrowException()
        {
            // arrange
            var dOld = new Doctor() { DoctorId = 1, EmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };

            _allDoctors.Add(dOld.DoctorId, dOld);

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Remove(dOld.DoctorId);
            // assert
            action.Should().NotThrow<Exception>();
            Assert.Null(_doctorRepoMock.Object.GetById(dOld.DoctorId));

            _doctorRepoMock.Verify(repo => repo
                .Remove(It.Is<int>(id => id == dOld.DoctorId)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(id => id == dOld.DoctorId)), Times.Once);
        }

        [Fact]
        public void Remove_WithNonExistingDoctor_shouldThrowException()
        {
            // arrange
            var dOld = new Doctor() { DoctorId = 1, EmailAddress = "email@gmail.com", FirstName = "doctor", LastName = "doctor", PhoneNumber = "22222222", IsAdmin = false };

            var service = new DoctorService(_doctorRepoMock.Object, _doctorValidatorMock.Object);

            // act
            Action action = () => service.Remove(dOld.DoctorId);
            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("This doctor does not exist");

            _doctorRepoMock.Verify(repo => repo
                .Remove(It.Is<int>(id => id == dOld.DoctorId)), Times.Never);

            _doctorValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(id => id == dOld.DoctorId)), Times.Once);
        }


        #endregion
    }
}