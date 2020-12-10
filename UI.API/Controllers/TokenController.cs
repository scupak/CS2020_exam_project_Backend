using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Entities.BE;
using Core.Services.ApplicationServices.Interfaces;
using Core.Services.Validators.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UI.API.Controllers
{
    [Route("/token")]
    [ApiController]
    public class TokenController : Controller
    {
        private IService<Patient, string> PatientService;
        private readonly IService<Doctor, string> _doctorService;
        private IAuthenticationHelper authenticationHelper;
        private IPatientValidator _PatientValidator;
        private IDoctorValidator _doctorValidator;

        public TokenController(IService<Patient, string> patientService, IService<Doctor, string> doctorService, IAuthenticationHelper authenticationHelper, IPatientValidator patientValidator, IDoctorValidator doctorValidator)
        {
            PatientService = patientService;
            _doctorService = doctorService;
            this.authenticationHelper = authenticationHelper;
            _PatientValidator = patientValidator;
            _doctorValidator = doctorValidator;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginInputModel model)
        {
            //var user = repository.GetAll().FirstOrDefault(u => u.Username == model.Username);
            try
            {
                _doctorValidator.ValidateEmail(model.Username);
                Doctor doctor = _doctorService.GetAll().FirstOrDefault(doc => doc.DoctorEmailAddress == model.Username);
            }
            catch(ArgumentException ex)
            {
                try
                {
                    _PatientValidator.ValidateCPR(model.Username);
                    Patient patient = PatientService.GetAll().FirstOrDefault(patient => patient.PatientCPR == model.Username);
                }
                catch(NullReferenceException e)
                {
                    return Unauthorized();
                }
                catch(InvalidDataException x)
                {
                    return Unauthorized();
                }
            }

            // check if username exists
            if (user == null)
                return Unauthorized();

            // check if password is correct
            if (!authenticationHelper.VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt))
                return Unauthorized();

            // Authentication successful
            return Ok(new
            {
                username = user.Username,
                token = authenticationHelper.GenerateToken(user)
            });
        }

    }
}
