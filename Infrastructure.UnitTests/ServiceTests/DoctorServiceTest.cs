using System.Collections.Generic;
using Core.Entities.Entities.BE;
using Core.Services.DomainServices;
using Moq;

namespace Infrastructure.UnitTests
{
    public class DoctorServiceTest
    {
        private readonly Mock<IRepository<Doctor>> _doctorRepoMock;

        private SortedDictionary<int, Doctor> _allDoctors;

        public DoctorServiceTest(Mock<IRepository<Doctor>> doctorRepoMock, SortedDictionary<int, Doctor> allDoctors)
        {
            _doctorRepoMock = doctorRepoMock;
            _allDoctors = allDoctors;
        }
    }
}