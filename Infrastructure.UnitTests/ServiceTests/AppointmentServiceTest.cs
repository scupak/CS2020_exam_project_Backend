using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Core.Entities.Entities.BE;
using Core.Entities.Entities.Filter;
using Core.Services.ApplicationServices.Implementations;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.DomainServices;
using Core.Services.Validators.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Infrastructure.UnitTests.ServiceTests
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IRepository<Appointment, int>> _appointmentRepoMock;
        private readonly Mock<IAppointmentValidator> _appointmentValidatorMock;

        private readonly Mock<IRepository<Doctor, string>> _doctorRepoMock;

       
        private Mock<IRepository<Patient, string>> _patientRepoMock;

        private SortedDictionary<int, Appointment> _allAppointments;
        private SortedDictionary<string, Doctor> _allDoctors;
        private SortedDictionary<string, Patient> _allPatients;

        public AppointmentServiceTest()
        {
            #region Appointment

            

                
            _allAppointments = new SortedDictionary<int, Appointment>();
            _appointmentRepoMock = new Mock<IRepository<Appointment, int>>();
            _appointmentValidatorMock = new Mock<IAppointmentValidator>();

            _appointmentRepoMock
                .Setup(repo => repo
                    .Add(It.IsAny<Appointment>()))
                .Callback<Appointment>(appointment => _allAppointments
                    .Add(appointment.AppointmentId, appointment))
                .Returns<Appointment>(appointment => _allAppointments[appointment.AppointmentId]);

            _appointmentRepoMock
                .Setup(repo => repo
                    .Edit(It.IsAny<Appointment>()))
                .Callback<Appointment>(appointment => _allAppointments[appointment.AppointmentId] = appointment)
                .Returns<Appointment>(appointment => _allAppointments[appointment.AppointmentId]);

            _appointmentRepoMock
                .Setup(repo => repo
                    .Remove(It
                        .IsAny<int>()))
                .Callback<int>(id => _allAppointments.Remove(id))
                .Returns<int>((id) => _allAppointments.ContainsKey(id) ? _allAppointments[id] : null);

            _appointmentRepoMock
                .Setup(repo => repo
                    .GetAll(It.IsAny<Filter>()))
                .Returns<Filter>((filter) => new FilteredList<Appointment>(){List = _allAppointments.Values.ToList(), TotalCount = _allAppointments.Count, FilterUsed = filter});
            
            _appointmentRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<int>()))
                .Returns<int>((id) => _allAppointments
                    .ContainsKey(id) ? _allAppointments[id] : null);

            _appointmentRepoMock
                .Setup(repo => repo
                    .Count())
                .Returns(() => _allAppointments.Count);
            #endregion

            #region Doctor

            

                
            _allDoctors = new SortedDictionary<string, Doctor>();
            _doctorRepoMock = new Mock<IRepository<Doctor, string>>();

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
            #endregion

            #region patient

            _allPatients = new SortedDictionary<string, Patient>();

            _patientRepoMock = new Mock<IRepository<Patient, string>>();

           


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
                .Returns<Filter>((filter) => new FilteredList<Patient>() { List = _allPatients.Values.ToList(), TotalCount = _allAppointments.Count, FilterUsed = filter });

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
            #endregion

        }

        [Fact]
        public void CreateAppointment_ValidCompanyRepository()
        {
            // arrange
            IRepository<Appointment, int> appointmentRepo = _appointmentRepoMock.Object;
            IAppointmentValidator validator = _appointmentValidatorMock.Object;

            IRepository<Doctor, string> doctorRepo = _doctorRepoMock.Object;

            IRepository<Patient, string> patientRepo = _patientRepoMock.Object;
            // act
            IService<Appointment, int> service = new AppointmentService(appointmentRepo, doctorRepo, patientRepo, validator);

            // assert
            Assert.NotNull(service);

        }

        [Fact]
        public void AppointmentService_IsOfTypeIService()
        {
            IRepository<Appointment, int> appointmentRepo = _appointmentRepoMock.Object;
            IAppointmentValidator validator = _appointmentValidatorMock.Object;

            IRepository<Doctor, string> doctorRepo = _doctorRepoMock.Object;

            IRepository<Patient, string> patientRepo = _patientRepoMock.Object;
            // act
           new AppointmentService(appointmentRepo, doctorRepo, patientRepo, validator)
               .Should()
               .BeAssignableTo<IService<Appointment, int>>();
        }

        #region GetAll

        [Fact]
        public void GetAllAppointmentsTest_ShouldNotThrowException()
        {
            //arrange
            Appointment a1 = new Appointment() { AppointmentId = 1};
            Appointment a2 = new Appointment(){AppointmentId = 2};
            var appointments = new List<Appointment>() { a1, a2 };

            Filter filter = new Filter() {};

            _allAppointments.Add(a1.AppointmentId, a1);
            _allAppointments.Add(a2.AppointmentId, a2);
            // the doctors in the repository
            var expected = new FilteredList<Appointment>()
                {List = _allAppointments.Values.ToList(), TotalCount = _allAppointments.Count, FilterUsed = filter};
            

            expected.TotalCount = _allAppointments.Count;
            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            var result = service.GetAll(filter);

            // assert
           Assert.Equal(expected.List, result.List);
            _appointmentRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(aFilter => aFilter == filter)), Times.Once);

        }

        [Fact]
        public void GetAllAppointmentsNegativPaggingTest_ShouldThrowException()
        {
            //arrange
            Appointment a1 = new Appointment() { AppointmentId = 1 };
            Appointment a2 = new Appointment() { AppointmentId = 2 };
            var appointments = new List<Appointment>() { a1, a2 };

            Filter filter = new Filter() {CurrentPage = -1};

            _allAppointments.Add(a1.AppointmentId, a1);
            _allAppointments.Add(a2.AppointmentId, a2);
            // the doctors in the repository
            var expected = new FilteredList<Appointment>()
                { List = _allAppointments.Values.ToList(), TotalCount = _allAppointments.Count, FilterUsed = filter };


            expected.TotalCount = _allAppointments.Count;
            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            Action action = () => service.GetAll(filter);

            // assert
            action.Should().Throw<InvalidDataException>()
                .WithMessage("current page and items pr page can't be negative");
            _appointmentRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(aFilter => aFilter == filter)), Times.Never);

        }

        [Fact]
        public void GetAllAppointmentsIndexOutOfBounds_ShouldThrowException()
        {
            //arrange
            Appointment a1 = new Appointment() { AppointmentId = 1 };
            Appointment a2 = new Appointment() { AppointmentId = 2 };
            Appointment a3 = new Appointment() { AppointmentId = 3 };

            Filter filter = new Filter() { CurrentPage = 2, ItemsPrPage = 3};

            _allAppointments.Add(a1.AppointmentId, a1);
            _allAppointments.Add(a2.AppointmentId, a2);
            _allAppointments.Add(a3.AppointmentId, a3);
            // the doctors in the repository
            var expected = new FilteredList<Appointment>()
                { List = _allAppointments.Values.ToList(), TotalCount = _allAppointments.Count, FilterUsed = filter };


            expected.TotalCount = _allAppointments.Count;
            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            Action action = () => service.GetAll(filter);

            // assert
            action.Should().Throw<ArgumentException>()
                .WithMessage("no more appointments");
            _appointmentRepoMock.Verify(repo => repo.GetAll(It.Is<Filter>(aFilter => aFilter == filter)), Times.Never);

        }


        #endregion

        #region GetById

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetById_WithValidId_ShouldNotThrowException(int id)
        {
            //arrange
            Appointment a = new Appointment() { AppointmentId = id };

            _allAppointments.Add(a.AppointmentId, a);
            

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            var result = service.GetById(a.AppointmentId);

            // assert
            Assert.Equal(a, result);

            _appointmentRepoMock.Verify(repo => repo
                .GetById(a.AppointmentId), Times.Once);

            _appointmentValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(appointmentId => appointmentId == a.AppointmentId)), Times.Once);

        }

        [Fact]
        public void GetById_AppointmentDoesNotExist_ShouldThrowException()
        {
            //arrange

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            Action action = () => service.GetById(1);

            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("An appointment with this id does not exist");
            
            _appointmentRepoMock.Verify(repo => repo
                .GetById(It.Is<int>(id => id == 1)), Times.Once);

            _appointmentValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(appointmentId => appointmentId == 1)), Times.Once);

        }

        #endregion

        #region Add

        [Theory]
        [InlineData(15, null, "Karl@gmail.com", "011200-4041")]
        [InlineData(15, null, "Karl@gmail.com", null)]
        [InlineData(15, "Knee checkup", "Charlie@gmail.uk", "110695-0004")]
        public void Add_WithValidAppointment_ShouldNotThrowException( int durationInMin, string description, string doctorEmailAddress, string patientCpr)
        {
            //arrange
            DateTime date = DateTime.Now.AddDays(3);
            Appointment a = new Appointment()
            {
                AppointmentDateTime = date,
                DurationInMin = durationInMin,
                Description = description,
                DoctorEmailAddress = doctorEmailAddress,
                PatientCpr = patientCpr
            };

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);
            _allDoctors.Add("Karl@gmail.com", new Doctor(){ DoctorEmailAddress = "Karl@gmail.com" });
            _allDoctors.Add("Charlie@gmail.uk", new Doctor() { DoctorEmailAddress = "Charlie@gmail.uk" });
            
            _allPatients.Add("011200-4041", new Patient() { PatientCPR = "011200-4041" });
            _allPatients.Add("110695-0004", new Patient() { PatientCPR = "110695-0004" });
            // act
            Action action = () => service.Add(a);

            // assert
            action.Should().NotThrow<Exception>();
            Assert.Contains(a, _allAppointments.Values);

            _appointmentRepoMock.Verify(repo => repo
                .Add(It.Is<Appointment>(appointment => appointment == a)), Times.Once);

            _appointmentValidatorMock.Verify(validator => validator
                .CreateValidation(It.Is<Appointment>(appointment => appointment == a)), Times.Once);

        }

        [Theory]
        [InlineData(15, null, "Karl@gmail.com", "011200-4041")]
        [InlineData(15, null, "Karl@gmail.com", null)]
        [InlineData(15, "Knee checkup", "Charlie@gmail.uk", "110695-0004")]
        public void Add_MissingRelations_ShouldThrowException(int durationInMin, string description, string doctorEmailAddress, string patientCpr)
        {
            //arrange
            DateTime date = DateTime.Now.AddDays(3);
            Appointment a = new Appointment()
            {
                AppointmentDateTime = date,
                DurationInMin = durationInMin,
                Description = description,
                DoctorEmailAddress = doctorEmailAddress,
                PatientCpr = patientCpr
            };

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);
           
            // act
            Action action = () => service.Add(a);

            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("This related entity does not exist");

            _appointmentRepoMock.Verify(repo => repo
                .Add(It.Is<Appointment>(appointment => appointment == a)), Times.Never);

            _appointmentValidatorMock.Verify(validator => validator
                .CreateValidation(It.Is<Appointment>(appointment => appointment == a)), Times.Once);

        }

        #endregion

        #region Edit

        [Theory]
        [InlineData(1, 15, null, null, null)]
        [InlineData(1, 15, null, "Karl@gmail.com", "011200-4041")]
        [InlineData(1, 15, null, null, "011200-4041")]
        [InlineData(1, 15, null, "Karl@gmail.com", null)]
        [InlineData(1, 15, "Knee checkup", "Charlie@gmail.uk", "110695-0004")]
        public void Edit_WithValidAppointment_ShouldNotThrowException(int id, int durationInMin, string description, string doctorEmailAddress, string patientCpr)
        {
            //arrange
            DateTime date = DateTime.Now.AddDays(3);
            Appointment aNew = new Appointment()
            {
                AppointmentId = id,
                AppointmentDateTime = date,
                DurationInMin = durationInMin,
                Description = description,
                DoctorEmailAddress = doctorEmailAddress,
                PatientCpr = patientCpr
            };

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);
            var aOld = new Appointment() { AppointmentId = id};
            _allAppointments.Add(aOld.AppointmentId, aOld);
            
            _allDoctors.Add("Karl@gmail.com", new Doctor() { DoctorEmailAddress = "Karl@gmail.com" });
            _allDoctors.Add("Charlie@gmail.uk", new Doctor() { DoctorEmailAddress = "Charlie@gmail.uk" });

            _allPatients.Add("011200-4041", new Patient() { PatientCPR = "011200-4041" });
            _allPatients.Add("110695-0004", new Patient() { PatientCPR = "110695-0004" });
            // act
            Action action = () => service.Edit(aNew);

            // assert
            action.Should().NotThrow<Exception>();
            Assert.Equal(_appointmentRepoMock.Object.GetById(aNew.AppointmentId), aNew);

            _appointmentRepoMock.Verify(repo => repo
                .Edit(It.Is<Appointment>(appointment => appointment == aNew)), Times.Once);

            _appointmentValidatorMock.Verify(validator => validator
                .EditValidation(It.Is<Appointment>(appointment => appointment == aNew)), Times.Once);

        }

        [Fact]
        public void Edit_WithNoExistingAppointment_ShouldThrowException()
        {
            //arrange
            DateTime date = DateTime.Now.AddDays(3);
            Appointment aNew = new Appointment()
            {
                AppointmentId = 1,
                AppointmentDateTime = date,
                DurationInMin = 15,
                Description = "description",
                DoctorEmailAddress = "Karl@gmail.com",
                PatientCpr = "011200-4041"
            };

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            _allDoctors.Add("Karl@gmail.com", new Doctor() { DoctorEmailAddress = "Karl@gmail.com" });
            _allDoctors.Add("Charlie@gmail.uk", new Doctor() { DoctorEmailAddress = "Charlie@gmail.uk" });

            _allPatients.Add("011200-4041", new Patient() { PatientCPR = "011200-4041" });
            _allPatients.Add("110695-0004", new Patient() { PatientCPR = "110695-0004" });
            // act
            Action action = () => service.Edit(aNew);

            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("appointment does not exists");

            _appointmentRepoMock.Verify(repo => repo
                .Edit(It.Is<Appointment>(appointment => appointment == aNew)), Times.Never);

            _appointmentValidatorMock.Verify(validator => validator
                .EditValidation(It.Is<Appointment>(appointment => appointment == aNew)), Times.Once);

        }

        [Theory]
        [InlineData(1, 15, null, "Karl@gmail.com", "011200-4041")]
        [InlineData(1, 15, null, null, "011200-4041")]
        [InlineData(1, 15, null, "Karl@gmail.com", null)]
        [InlineData(1, 15, "Knee checkup", "Charlie@gmail.uk", "110695-0004")]
        public void Edit_WithNonExitingRelation_ShouldThrowException(int id, int durationInMin, string description, string doctorEmailAddress, string patientCpr)
        {
            //arrange
            DateTime date = DateTime.Now.AddDays(3);
            Appointment aNew = new Appointment()
            {
                AppointmentId = id,
                AppointmentDateTime = date,
                DurationInMin = durationInMin,
                Description = description,
                DoctorEmailAddress = doctorEmailAddress,
                PatientCpr = patientCpr
            };

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);
            var aOld = new Appointment() { AppointmentId = id };
            _allAppointments.Add(aOld.AppointmentId, aOld);

            // act
            Action action = () => service.Edit(aNew);

            // assert
            action.Should().Throw<KeyNotFoundException>().WithMessage("This related entity does not exist");

            _appointmentRepoMock.Verify(repo => repo
                .Edit(It.Is<Appointment>(appointment => appointment == aNew)), Times.Never);

            _appointmentValidatorMock.Verify(validator => validator
                .EditValidation(It.Is<Appointment>(appointment => appointment == aNew)), Times.Once);

        }

        #endregion

        #region Remove

        [Fact]
        public void Remove_ShouldNotThrowException()
        {
            //arrange
            Appointment aNew = new Appointment() { AppointmentId = 1};

            _allAppointments.Add(aNew.AppointmentId, aNew);

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            Action action = () => service.Remove(aNew.AppointmentId);
            action.Should().NotThrow<Exception>();


            // assert
            Assert.Null(_appointmentRepoMock.Object.GetById(aNew.AppointmentId));

            _appointmentRepoMock.Verify(repo => repo
                .Remove(aNew.AppointmentId), Times.Once);

            _appointmentValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(appointmentId => appointmentId == aNew.AppointmentId)), Times.Once);

        }

        [Fact]
        public void RemoveWithNoAppointment_ShouldThrowException()
        {
            //arrange
            Appointment aNew = new Appointment() { AppointmentId = 1 };

            

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            Action action = () => service.Remove(aNew.AppointmentId);
            action.Should().Throw<KeyNotFoundException>().WithMessage("Appointment does not exist");


            // assert

            _appointmentRepoMock.Verify(repo => repo
                .Remove(aNew.AppointmentId), Times.Never);

            _appointmentValidatorMock.Verify(validator => validator
                .IdValidation(It.Is<int>(appointmentId => appointmentId == aNew.AppointmentId)), Times.Once);

        }

        #endregion
    }
}
