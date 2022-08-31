using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Services;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers.Api
{
    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentApiController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;

        public AppointmentApiController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(AppointmentVM data)
        {
            RequestResult<int> requestResult = new RequestResult<int>();
            try
            {
                requestResult.status = _appointmentService.AddUpdate(data).Result;
                if (requestResult.status == 1)
                {
                    requestResult.message = Helper.appointmentUpdated;
                }
                if (requestResult.status == 2)
                {
                    requestResult.message = Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {
                requestResult.message = e.Message;
                requestResult.status = Helper.failure_code;
            }

            return Ok(requestResult);
        }

        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string doctorId)
        {
            RequestResult<List<AppointmentVM>> requestResult = new RequestResult<List<AppointmentVM>>();
            try
            {
                if (role == Helper.Patient)
                {
                    requestResult.dataenum = _appointmentService.PatientsEventsById(loginUserId);
                    requestResult.status = Helper.success_code;
                }
                else if (role == Helper.Doctor)
                {
                    requestResult.dataenum = _appointmentService.DoctorsEventsById(loginUserId);
                    requestResult.status = Helper.success_code;
                }
                else
                {
                    requestResult.dataenum = _appointmentService.DoctorsEventsById(doctorId);
                    requestResult.status = Helper.success_code;
                }
            }
            catch (Exception e)
            {
                requestResult.message = e.Message;
                requestResult.status = Helper.failure_code;
            }
            return Ok(requestResult);
        }

        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            RequestResult<AppointmentVM> requestResult = new RequestResult<AppointmentVM>();
            try
            {

                requestResult.dataenum = _appointmentService.GetById(id);
                requestResult.status = Helper.success_code;

            }
            catch (Exception e)
            {
                requestResult.message = e.Message;
                requestResult.status = Helper.failure_code;
            }
            return Ok(requestResult);
        }

        [HttpGet]
        [Route("DeleteAppoinment/{id}")]
        public async Task<IActionResult> DeleteAppoinment(int id)
        {
            RequestResult<int> requestResult = new RequestResult<int>();
            try
            {
                requestResult.status = await _appointmentService.Delete(id);
                requestResult.message = requestResult.status == 1 ? Helper.appointmentDeleted : Helper.somethingWentWrong;

            }
            catch (Exception e)
            {
                requestResult.message = e.Message;
                requestResult.status = Helper.failure_code;
            }
            return Ok(requestResult);
        }

        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            RequestResult<int> requestResult = new RequestResult<int>();
            try
            {
                var result = _appointmentService.ConfirmEvent(id).Result;
                if (result > 0)
                {
                    requestResult.status = Helper.success_code;
                    requestResult.message = Helper.meetingConfirm;
                }
                else
                {

                    requestResult.status = Helper.failure_code;
                    requestResult.message = Helper.meetingConfirmError;
                }

            }
            catch (Exception e)
            {
                requestResult.message = e.Message;
                requestResult.status = Helper.failure_code;
            }
            return Ok(requestResult);
        }


    }
}
