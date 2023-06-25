using CarRental.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Common.DbEntities
{
    public class Car
    {
        public Guid CarId { get; set; }
        public string Description { get; set; }
        public string AvailableExtras { get; set; }
        public decimal Price { get; set; }
        public decimal Discounts { get; set; }
        public int MinimumDriverAge { get; set; }
        public bool Status { get; set; }
        public CarGroups CarGroup { get; set; }
        public DriverAgeGroups DriverAgeGroup { get; set; }
        public Locations Location { get; set; }
    }
}
