using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
  
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }
        [AllowAnonymous]
        [HttpGet("[action]")]
        public ActionResult<List<ClientDto?>> GetAllClients()
        {
            return _clientService.GetAllClients();
        }

        [HttpGet("[action]/{id}")]
        public ActionResult<ClientDto?> GetClientById([FromRoute] int id)
        {
            try
            {
                return _clientService.GetClientById(id);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public ActionResult<ClientDto> CreateNewClient([FromBody] ClientCreateRequest clientCreateRequest)
        {
            try
            {
                return _clientService.CreateNewClient(clientCreateRequest);
            }
            catch (ValidationException ex) //por si hay datos inválidos
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex) //por si el email suministrado ya existe
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpPut("[action]")]
        public ActionResult ModifyClientData([FromBody] ClientUpdateRequest clientUpdateRequest)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            try
            {
                _clientService.ModifyClientData(userId, clientUpdateRequest);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpDelete("[action]/{id}")]
        public ActionResult PermanentDeletionClient([FromRoute] int id)
        {
            try
            {
                _clientService.PermanentDeletionClient(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("[action]/{id}")]
        public ActionResult LogicalDeletionClient([FromRoute] int id)
        {
            try
            {
                _clientService.LogicalDeletionClient(id);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPut("[action]")]
        public ActionResult RequestPasswordReset([FromBody] string email)
        {
            try
            {
                _clientService.RequestPassReset(email);
                return Ok();
            }
            catch(NotFoundException ex)
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
                _clientService.ResetPassword(request); 
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}