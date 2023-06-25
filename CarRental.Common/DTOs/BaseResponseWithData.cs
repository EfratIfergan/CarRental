using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.DTOs
{
    public class BaseResponseWithData<T> : BaseResponse
    {
        public T? Data { get; set; }
    }
}
