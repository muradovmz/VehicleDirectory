using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class VehicleCreateDto
    {
        public int ModelId { get; set; }
        public string VinCode { get; set; }
        public string StateNumberPlate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int ColorId { get; set; }
        public int FuelTypeId { get; set; }
        //public string PictureUrl { get; set; }
    }
}
