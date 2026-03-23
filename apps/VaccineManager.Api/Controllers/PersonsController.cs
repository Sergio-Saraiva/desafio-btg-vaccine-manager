using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using VaccineManager.Application.Common.Responses;
using VaccineManager.Application.Persons.Commands.CreatePerson;
using VaccineManager.Application.Persons.Commands.DeletePerson;
using VaccineManager.Application.Persons.Commands.UpdatePerson;
using VaccineManager.Application.Persons.Queries.GetPersonVaccinationCard;
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
        [ProducesResponseType(typeof(ApiResponse<CreatePersonResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)] 
        public async Task<IActionResult> Post([FromBody] CreatePersonCommand command)
        {
            return await SendRequest(command);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ListPersonsResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List([FromQuery] SieveModel sieveModel)
        {
            return await SendRequest(new ListPersonsQuery(sieveModel));
        }

        [HttpGet("{id:guid}/vaccination-card")]
        [ProducesResponseType(typeof(ApiResponse<GetPersonVaccinationCardResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVaccinationCard([FromRoute] Guid id)
        {
            var query = new GetPersonVaccinationCardQuery(id);
            return await SendRequest(query);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<UpdatePersonResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]                                          
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromBody] UpdatePersonCommand command)
        {
            return await SendRequest(command);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]                                         
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var command = new DeletePersonCommand(id);
            return await SendRequest(command);
        }
    }
}
