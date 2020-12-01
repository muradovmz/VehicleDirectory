namespace Core.Entities
{
    public class VehicleOwner
    {
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}