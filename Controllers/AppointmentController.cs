
using MedicalAppointmentApi.Extentions;
using MedicalAppointmentApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MedicalAppointmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.GetUserId();
            var role = User.GetUserRole(); // helper extension
            var result = await _appointmentService.CancelAsync(id, userId, role);
            return Ok(result);
        }
    }
}