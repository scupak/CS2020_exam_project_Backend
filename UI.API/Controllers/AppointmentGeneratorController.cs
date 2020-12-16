using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace UI.API.Controllers

{   [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentGeneratorController : Controller
    {
        private readonly AppointmentGenerator _appointmentGenerator;


        public AppointmentGeneratorController(AppointmentGenerator appointmentGenerator)
        {
            _appointmentGenerator = appointmentGenerator;
        }
        [Authorize(Roles = "Administrator")]
        [Route("stop")]
        [HttpGet]
        public IActionResult stop()
        {
            if (_appointmentGenerator.Runnning)
            {
            

             _appointmentGenerator.StopAsync(new System.Threading.CancellationToken());
                return Ok("generator stopped");
            }
            else
            {
                return StatusCode(400, "the generator is already stopped");
            }
        }
        [Authorize(Roles = "Administrator")]
        [Route("start")]
        [HttpGet]
        public IActionResult start()
        {
            if (!_appointmentGenerator.Runnning)
            {

                _appointmentGenerator.StartAsync(new System.Threading.CancellationToken());
            return Ok("generator started");
            }
            else
            {
                return StatusCode(400, "the generator is already started");
            }
        }
    }
}
