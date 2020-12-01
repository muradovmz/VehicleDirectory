using System;
using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class VehicleToReturnDto
    {
        public int Id { get; set; }
        public string ManufacturerNameGE { get; set; }
        public string ManufacturerNameEN { get; set; }
        public string ModelNameGE { get; set; }
        public string ModelNameEN { get; set; }
        public string VinCode { get; set; }
        public string StateNumberPlate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public string Color { get; set; }
        public string PictureUrl { get; set; }
        public ICollection<string> VehicleFuelTypes { get; set; }
        public ICollection<string> VehicleOwners { get; set; }
        public IEnumerable<PhotoToReturnDto> Photos { get; set; }
    }
}