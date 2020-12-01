using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Entities
{
    public class Vehicle : BaseEntity
    {
        public Model Model { get; set; }
        public int ModelId { get; set; }
        public string VinCode { get; set; }
        public string StateNumberPlate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public Color Color { get; set; }
        public int ColorId { get; set; }
        private List<VehicleFuelType> _vehicleFuelTypes = new List<VehicleFuelType>();
        public ICollection<VehicleFuelType> VehicleFuelTypes => _vehicleFuelTypes;
        private List<VehicleOwner> _vehicleOwners = new List<VehicleOwner>();
        public ICollection<VehicleOwner> VehicleOwners => _vehicleOwners;
        private readonly List<Photo> _photos = new List<Photo>();
        public IReadOnlyList<Photo> Photos => _photos.AsReadOnly();

        public void AddPhoto(string pictureUrl, string fileName, bool isMain = false)
        {
            var photo = new Photo
            {
                FileName = fileName,
                PictureUrl = pictureUrl
            };

            if (_photos.Count == 0) photo.IsMain = true;

            _photos.Add(photo);
        }


        public void RemovePhoto(int id)
        {
            var photo = _photos.Find(x => x.Id == id);
            _photos.Remove(photo);
        }

        public void SetMainPhoto(int id)
        {
            var currentMain = _photos.SingleOrDefault(item => item.IsMain);
            foreach (var item in _photos.Where(item => item.IsMain))
            {
                item.IsMain = false;
            }

            var photo = _photos.Find(x => x.Id == id);
            if (photo != null)
            {
                photo.IsMain = true;
                if (currentMain != null) currentMain.IsMain = false;
            }
        }

        public void AddOwner(int ownerId, int vehicleId)
        {
            var vehicleOwner = new VehicleOwner
            {
                OwnerId = ownerId,
                VehicleId = vehicleId
            };
            _vehicleOwners.Add(vehicleOwner);
        }

        public void RemoveOwner(int id)
        {
            var owner = _vehicleOwners.Find(x => x.OwnerId == id);
            _vehicleOwners.Remove(owner);
        }

         public void AddFuelType(int fuelTypeId, int vehicleId)
        {
            var vehicleFuelType = new VehicleFuelType
            {
                FuelTypeId=fuelTypeId,
                VehicleId=vehicleId
            };
            _vehicleFuelTypes.Add(vehicleFuelType);
        }

        public void RemoveFuelType(int id)
        {
            var fuelType = _vehicleFuelTypes.Find(x => x.FuelTypeId == id);
            _vehicleFuelTypes.Remove(fuelType);
        }

    }
}