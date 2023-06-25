using CarRental.Common.DbEntities;
using CarRental.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.BL.Interfaces
{
    public interface IBackOffice
    {
        Task<BaseResponse> CreateNewCarAsync(Car carToInsert, CancellationToken cancellationToken);
        Task<BaseResponse> DeleteCarAsync(Guid carId, CancellationToken cancellationToken);
        Task<BaseResponse> UpdateCarAsync(Car carToUpdate, CancellationToken cancellationToken);
    }
}
