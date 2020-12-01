using System;
namespace Infrastructure.Data
{
    public class VehicleSeedModel
    {
        public int ModelId { get; set; }
        public string VinCode { get; set; }
        public string StateNumberPlate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int ColorId { get; set; }
        public string PictureUrl { get; set; }
    }
}