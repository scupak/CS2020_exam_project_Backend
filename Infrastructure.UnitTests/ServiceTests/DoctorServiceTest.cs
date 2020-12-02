using System.Collections.Generic;
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
                    .Get(It.IsAny<int>()))
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

    }
}