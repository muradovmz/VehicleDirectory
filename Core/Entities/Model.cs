using System.Collections.Generic;

namespace Core.Entities
{
    public class Model:BaseEntity
    {
        public string ModelNameGE { get; set; }
        public string ModelNameEN { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public int ManufacturerId { get; set; }
    }
}