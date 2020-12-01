using System.Collections.Generic;

namespace Core.Entities
{
    public class Manufacturer : BaseEntity
    {
        public string ManufacturerNameGE { get; set; }
        public string ManufacturerNameEN { get; set; }
        public ICollection<Model> Models { get; set; }
    }
}