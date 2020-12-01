using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Owner:BaseEntity
    {
        public string PrivateNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<VehicleOwner> VehicleOwners { get; set; }
    }
}