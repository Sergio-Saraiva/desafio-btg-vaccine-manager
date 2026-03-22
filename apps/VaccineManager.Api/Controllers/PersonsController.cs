using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using VaccineManager.Application.Persons.Commands.CreatePerson;
using VaccineManager.Application.Persons.Commands.DeletePerson;
using VaccineManager.Application.Persons.Commands.UpdatePerson;
using VaccineManager.Application.Persons.Queries.ListPersons;

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

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] SieveModel sieveModel)
        {
            return await SendRequest(new ListPersonsQuery(sieveModel));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand command)
        {
            return await SendRequest(command);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeletePersonCommand(id);
            return await SendRequest(command);
        }
    }
}
