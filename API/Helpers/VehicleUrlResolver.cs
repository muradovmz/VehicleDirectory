using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Microsoft.Extensions.Configuration;

namespace API.Helpers
{
    public class VehicleUrlResolver : IValueResolver<Vehicle, VehicleToReturnDto, string>
    {
        private readonly IConfiguration _config;
        public VehicleUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Vehicle source, VehicleToReturnDto destination, string destMember, ResolutionContext context)
        {
            var photo =source.Photos.FirstOrDefault(x => x.IsMain);

            if (photo != null)
            {
                return _config["ApiUrl"] + photo.PictureUrl;
            }

            return _config["ApiUrl"] + "images/vehicles/auto1.png";
        }
    }
}