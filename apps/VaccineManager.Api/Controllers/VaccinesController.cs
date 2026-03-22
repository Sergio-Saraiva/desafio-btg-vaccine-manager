using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using VaccineManager.Application.Vaccines.Commands.CreateVaccine;
using VaccineManager.Application.Vaccines.Commands.DeleteVaccine;
using VaccineManager.Application.Vaccines.Commands.UpdateVaccine;
using VaccineManager.Application.Vaccines.Queries.ListVaccines;

namespace VaccineManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinesController : ApiBaseController
    {
        public VaccinesController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVaccineCommand command)
        {
            return await SendRequest(command);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] SieveModel sieveModel)
        {
            var query = new ListVaccineQuery(sieveModel);
            return await SendRequest(query);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateVaccineCommand command)
        {
            return await SendRequest(command);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteVaccineCommand(id);
            return await SendRequest(command);
        }
    }
}
