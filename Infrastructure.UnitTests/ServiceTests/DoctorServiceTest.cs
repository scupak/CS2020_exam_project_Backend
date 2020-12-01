using System.Collections.Generic;
using Core.Entities.Entities.BE;
using Core.Services.DomainServices;
using Moq;

namespace Infrastructure.UnitTests.ServiceTests
{
    public class DoctorServiceTest
    {
        private readonly Mock<IRepository<Doctor, int>> _doctorRepoMock;

        private SortedDictionary<int, Doctor> _allDoctors;

        public DoctorServiceTest(Mock<IRepository<Doctor, int>> doctorRepoMock, SortedDictionary<int, Doctor> allDoctors)
        {
            _doctorRepoMock = doctorRepoMock;
            _allDoctors = allDoctors;
        }
    }
}