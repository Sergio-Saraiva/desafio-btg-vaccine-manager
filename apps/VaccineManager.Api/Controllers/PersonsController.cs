using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineManager.Application.Persons.Commands.CreatePerson;

namespace VaccineManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ApiBaseController
    {
        public PersonsController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreatePersonCommand command)
        {
            return await SendRequest(command);
        }
    }
}
