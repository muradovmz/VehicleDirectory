namespace Core.Entities
{
    public class VehicleFuelType
    {
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int FuelTypeId { get; set; }
        public FuelType FuelType { get; set; }
    }
}