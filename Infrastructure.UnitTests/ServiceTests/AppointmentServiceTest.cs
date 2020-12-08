using System.Collections.Generic;
using System.Linq;
using Core.Entities.Entities.BE;
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
        private SortedDictionary<string, Patient> allPatients;

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
                    .GetAll())
                .Returns(() => _allAppointments.Values.ToList());

            _appointmentRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<int>()))
                .Returns<int>((id) => _allAppointments
                    .ContainsKey(id) ? _allAppointments[id] : null);
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
                    .GetAll())
                .Returns(() => _allDoctors.Values.ToList());

            _doctorRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<string>()))
                .Returns<string>((email) => _allDoctors
                    .ContainsKey(email) ? _allDoctors[email] : null);
            #endregion

            #region patient

            allPatients = new SortedDictionary<string, Patient>();

            _patientRepoMock = new Mock<IRepository<Patient, string>>();

           


            _patientRepoMock
                .Setup(repo => repo
                    .Add(It.IsAny<Patient>()))
                .Callback<Patient>(patient => allPatients
                    .Add(patient.PatientCPR, patient))
                .Returns<Patient>(patient => allPatients[patient.PatientCPR]);

            _patientRepoMock
                .Setup(repo => repo
                    .Edit(It.IsAny<Patient>()))
                .Callback<Patient>(patient => allPatients[patient.PatientCPR] = patient)
                .Returns<Patient>(patient => allPatients[patient.PatientCPR]);

            _patientRepoMock
                .Setup(repo => repo
                    .Remove(It
                        .IsAny<string>()))
                .Callback<string>(id => allPatients.Remove(id))
                .Returns<string>((id) => allPatients
                    .ContainsKey(id) ? allPatients[id] : null);

            _patientRepoMock
                .Setup(repo => repo
                    .GetAll())
                .Returns(() => allPatients.Values.ToList());

            _patientRepoMock
                .Setup(repo => repo
                    .GetById(It.IsAny<string>()))
                .Returns<string>((CPR) => allPatients
                    .ContainsKey(CPR)
                    ? allPatients[CPR]
                    : null);

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

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllDoctors(int doctorCount)
        {
            //arrange
            Appointment a1 = new Appointment() { AppointmentId = 1};
            Appointment a2 = new Appointment(){AppointmentId = 2};
            var appointments = new List<Appointment>() { a1, a2 };

            // the doctors in the repository
            var expected = appointments.GetRange(0, doctorCount);
            foreach (var a in expected)
            {
                _allAppointments.Add(a.AppointmentId, a);
            }

            var service = new AppointmentService(_appointmentRepoMock.Object, _doctorRepoMock.Object, _patientRepoMock.Object, _appointmentValidatorMock.Object);

            // act
            var result = service.GetAll();

            // assert
            Assert.Equal(expected, result);
            _appointmentRepoMock.Verify(repo => repo.GetAll(), Times.Once);

        }

        #endregion
    }
}
