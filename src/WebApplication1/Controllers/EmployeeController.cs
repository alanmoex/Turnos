using Application;
using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Application.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        [HttpGet ("[action]")]
        public ActionResult<List<EmployeeResponseDTO>> GetAll()
        {
            return Ok(_employeeService.GetAll());
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<EmployeeResponseDTO> GetById(int id)
        {
            try
            {
                return Ok(_employeeService.GetById(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpGet("[action]/{shopId}")]
        public ActionResult<List<EmployeeResponseDTO>> GetAvailables(int shopId)
        {
            try
            {
                return Ok(_employeeService.GetAvailables(shopId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }

        }


        [HttpGet("[action]/{shopId}")]
        public ActionResult<List<EmployeeResponseDTO>> GetAllByShopId(int shopId)
        {
            try
            {
                return Ok(_employeeService.GetAllByShopId(shopId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }


        [HttpPost("[action]")]
        public IActionResult Create([FromBody] EmployeeCreateRequestDTO request)
        {
            try
            {
                var service = _employeeService.Create(request);
                return Ok("Se creo correctamente");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _employeeService.Delete(id);
                return Ok("Se elimino correctamente");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPut("[action]/{id}")]
        public IActionResult Update(int id, [FromBody] EmployeeUpdateRequest request)
        {
            try
            {
                _employeeService.Update(id, request);
                return Ok("Se actualizo con exito");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public ActionResult RequestPasswordReset([FromBody] string email)
        {
            try
            {
                _employeeService.RequestPassReset(email);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public ActionResult ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                _employeeService.ResetPassword(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
