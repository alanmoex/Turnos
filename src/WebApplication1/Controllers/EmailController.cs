using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.Models;
using Domain.Exceptions;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("sendEmail")]
        public IActionResult SendEmail ([FromBody]EmailDTO request)
        {
            try
            {
            _emailService.SendMail(request);
            return Ok();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
         
        [HttpPost("sendConfirmationEmail")]
        public IActionResult Sendconfirmation([FromBody]string email, [FromRoute] string nameUser)
        {
            try
            {
            _emailService.AccountCreationConfirmationEmail(email, nameUser );
            return Ok("");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        } 
    }

}
