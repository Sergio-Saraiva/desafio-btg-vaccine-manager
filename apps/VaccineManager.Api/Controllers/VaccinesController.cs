using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using VaccineManager.Application.Common.Responses;
using VaccineManager.Application.Vaccines.Commands.CreateVaccine;
using VaccineManager.Application.Vaccines.Commands.DeleteVaccine;
using VaccineManager.Application.Vaccines.Commands.UpdateVaccine;
using VaccineManager.Application.Vaccines.Queries.ListVaccines;

namespace VaccineManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VaccinesController : ApiBaseController
    {
        public VaccinesController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateVaccineResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create([FromBody] CreateVaccineCommand command)
        {
            return await SendRequest(command);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<ListVaccineResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> List([FromQuery] SieveModel sieveModel)
        {
            var query = new ListVaccineQuery(sieveModel);
            return await SendRequest(query);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<UpdateVaccineResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update([FromBody] UpdateVaccineCommand command)
        {
            return await SendRequest(command);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteVaccineCommand(id);
            return await SendRequest(command);
        }
    }
}
