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
    [Route("api/token")]
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
                //check if the username is a valid email. 
                _doctorValidator.ValidateEmail(model.Username);
                //get the doctor
                Doctor doctor = _doctorService.GetById(model.Username);

                // check if doctor exists
                if (doctor == null)
                {
                    return Unauthorized();
                }
                // check if password is correct
                if (!authenticationHelper.VerifyPasswordHash(model.Password, doctor.PasswordHash,
                    doctor.PasswordSalt))
                {
                    return Unauthorized();
                }


                // Authentication successful
                return Ok(new
                {
                    username = doctor.DoctorEmailAddress,
                    token = authenticationHelper.GenerateToken(doctor)
                });
            }
            catch(ArgumentException ex)
            {
                try
                {
                    //check if the username is a valid cpr. 
                    _PatientValidator.ValidateCPR(model.Username);

                    //get the patient
                    Patient patient = PatientService.GetById(model.Username);

                    // check if patient exists
                    if (patient == null)
                    {
                        return Unauthorized();
                    }

                    // check if password is correct
                    if (!authenticationHelper.VerifyPasswordHash(model.Password, patient.PasswordHash,
                        patient.PasswordSalt))
                    {
                        return Unauthorized();
                    }

                    // Authentication successful
                    return Ok(new
                    {
                        username = patient.PatientCPR,
                        token = authenticationHelper.GenerateToken(patient)
                    });

                }
                catch (KeyNotFoundException e)
                {
                    return Unauthorized();
                }
                catch (NullReferenceException e)
                {
                    return Unauthorized();
                }
                catch(InvalidDataException x)
                {
                    return Unauthorized();
                }
                catch (Exception x)
                {
                    return Unauthorized();
                }
            }
            catch (KeyNotFoundException e)
            {
                return Unauthorized();
            }
            catch (NullReferenceException e)
            {
                return Unauthorized();
            }
            catch (InvalidDataException x)
            {
                return Unauthorized();
            }
            catch (Exception x)
            {
                return Unauthorized();
            }
            /*
            // Authentication successful
            return Ok(new
            {
                username = user.Username,
                token = authenticationHelper.GenerateToken(user)
            });
            */
        }

    }
}
