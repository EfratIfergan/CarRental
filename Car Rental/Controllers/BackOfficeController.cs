using CarRental.BL.Interfaces;
using CarRental.Common.ConfigurationDTOs;
using CarRental.Common.DbEntities;
using CarRental.Common.DTOs;
using CarRental.Common.Enumerations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackOfficeController : ControllerBase
    {
        private readonly IBackOffice _iBackOffice;

        public BackOfficeController(IBackOffice IBackOffice)
        {
            _iBackOffice = IBackOffice;
        }

        [HttpPost("CreateNewCar")]
        public async Task<ActionResult> CreateNewCar([FromBody] Car carToCreate, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iBackOffice.CreateNewCarAsync(carToCreate, cancellationToken));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("UpdateExistsCar")]
        public async Task<ActionResult> UpdateExistsCar([FromBody] Car carToUpdate, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iBackOffice.UpdateCarAsync(carToUpdate, cancellationToken));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("DeleteCar")]
        public async Task<ActionResult> DeleteCar([FromBody] Guid carId, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iBackOffice.DeleteCarAsync(carId, cancellationToken));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}
