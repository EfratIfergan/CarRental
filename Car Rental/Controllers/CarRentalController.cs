using CarRental.BL.Interfaces;
using CarRental.Common.ConfigurationDTOs;
using CarRental.Common.DTOs;
using CarRental.Common.Enumerations;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarRentalController : ControllerBase
    {
        private readonly ICarRental _iCarRental;

        public CarRentalController(ICarRental ICarRental)
        {
            _iCarRental = ICarRental;
        }

        [HttpGet("GetAllCars")]
        public async Task<ActionResult> GetAllCars(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iCarRental.GetAllCarsAsync(cancellationToken));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("SearchCarsByParamaters")]
        public async Task<ActionResult> SearchCarByParamaters([FromBody] SearchCarsByParamatersDTO searchCarsByParamatersDTO, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iCarRental.SearchCarsByParamatersAsync(searchCarsByParamatersDTO, cancellationToken));
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

        [HttpPost("ReantACarByCarId")]
        public async Task<ActionResult> ReantACarByCarId([FromBody] RentCarDTO rentCarDTO, CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iCarRental.ReantACarByCarIdAsync(rentCarDTO, cancellationToken));
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

        [HttpGet("GetAllCarHistories")]
        public async Task<ActionResult> GetAllCarHistories(CancellationToken cancellationToken)
        {
            try
            {
                return Ok(await _iCarRental.GetAllCarHistoriesAsync(cancellationToken));
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
