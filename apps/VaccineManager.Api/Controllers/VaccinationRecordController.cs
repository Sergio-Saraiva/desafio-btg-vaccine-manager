using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineManager.Application.Common;
using VaccineManager.Application.VaccinationRecord.CreateVaccinationRecord;
using VaccineManager.Application.VaccinationRecord.DeleteVaccinationRecord;

namespace VaccineManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationRecordController : ApiBaseController
    {
        public VaccinationRecordController(ISender sender) : base(sender)
        {
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateVaccinationRecordResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]                                          
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post([FromBody] CreateVaccinationRecordCommand command)
        {
            return await SendRequest(command);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]   
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteVaccinationRecordCommand(id);
            return await SendRequest(command);
        }
    }
}
