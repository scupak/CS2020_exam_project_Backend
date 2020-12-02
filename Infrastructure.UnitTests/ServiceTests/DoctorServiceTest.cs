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
                .Returns<Doctor>(doctor => _allDoctors[doctor.DoctorId]);

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
            action.Should().Throw<KeyNotFoundException>().WithMessage("Id does not exist");
            
            _doctorRepoMock.Verify(repo => repo
                .GetById(It.Is<int>(id => id == d1.DoctorId)), Times.Once);

            _doctorValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(id => id == d1.DoctorId)), Times.Once);
        }


        #endregion

        #region Add

        

        #endregion
    }
}