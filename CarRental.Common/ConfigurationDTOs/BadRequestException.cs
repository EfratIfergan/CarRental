using CarRental.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.ConfigurationDTOs
{
    public class BadRequestException : Exception
    {
        public int StatusCode { get; }

        public BadRequestException(string message, int statusCode = (int)HttpStatusCode.BadRequest) : base(message)
        {
            StatusCode = statusCode;
        }
    }

}
