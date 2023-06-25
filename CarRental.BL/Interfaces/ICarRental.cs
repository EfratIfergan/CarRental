using CarRental.Common.DbEntities;
using CarRental.Common.DTOs;

namespace CarRental.BL.Interfaces
{
    public interface ICarRental
    {
        Task<BaseResponseWithData<List<Car>>> GetAllCarsAsync(CancellationToken cancellationToken);
        Task<BaseResponseWithData<List<Car>>> SearchCarsByParamatersAsync(SearchCarsByParamatersDTO searchCarsByParamatersDTO, CancellationToken cancellationToken);
        Task<BaseResponse> ReantACarByCarIdAsync(RentCarDTO rentCarDTO, CancellationToken cancellationToken);
        Task<BaseResponseWithData<List<CarHistory>>> GetAllCarHistoriesAsync(CancellationToken cancellationToken);
    }
}
