using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.DTOs
{
    public class BaseResponse
    {
        public bool IsSuccess { get; set; }
        public List<string>? ErrorMessages { get; set; } = new List<string>();
    }
}
