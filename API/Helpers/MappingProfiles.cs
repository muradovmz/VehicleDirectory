using System.Linq;
using API.Dtos;
using AutoMapper;
using Core.Entities;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Vehicle, VehicleToReturnDto>()
                .ForMember(d => d.ManufacturerNameGE, o => o.MapFrom(s => s.Model.Manufacturer.ManufacturerNameGE))
                .ForMember(d => d.ManufacturerNameEN, o => o.MapFrom(s => s.Model.Manufacturer.ManufacturerNameEN))
                .ForMember(d => d.ModelNameGE, o => o.MapFrom(s => s.Model.ModelNameGE))
                .ForMember(d => d.ModelNameEN, o => o.MapFrom(s => s.Model.ModelNameEN))
                .ForMember(d => d.Color, o => o.MapFrom(s => s.Color.ColorName))
                .ForMember(d => d.VehicleFuelTypes, o => o.MapFrom(s =>s.VehicleFuelTypes.Select(y => y.FuelType.FuelTypeName).ToList()))
                .ForMember(d => d.VehicleOwners, o => o.MapFrom(s =>s.VehicleOwners.Select(x => x.Owner.FirstName + " " + x.Owner.LastName).ToList()))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<VehicleUrlResolver>());
            CreateMap<VehicleCreateDto,Vehicle>();
            CreateMap<Photo, PhotoToReturnDto>()
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PhotoUrlResolver>());
            CreateMap<OwnerCreateDto,Owner>();
            CreateMap<VehicleOwnerCreateDto,VehicleOwner>();
            CreateMap<Vehicle, OwnerToReturnDto>()
                .ForMember(d => d.PrivateNumber, o => o.MapFrom(s => s.VehicleOwners.Select(x => x.Owner.PrivateNumber)))
                .ForMember(d => d.FirstName, o => o.MapFrom(s => s.VehicleOwners.Select(x => x.Owner.FirstName)))
                .ForMember(d => d.LastName, o => o.MapFrom(s => s.VehicleOwners.Select(x => x.Owner.LastName)));
        }
    }
}