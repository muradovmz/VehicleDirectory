using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;
using API.Helpers;
using API.Validators;

namespace API.Controllers
{
    public class VehiclesController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;
        public VehiclesController(IUnitOfWork unitOfWork, IMapper mapper, IPhotoService photoService)
        {
            _photoService = photoService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        //სატრანსპორტო საშუალებების სიის მიღება  - სწრაფი და დეტალური ძებნის მეშვეობით, paging -ისა და ordering- ის ფუნქციით
        //Query Params: manufacturerId, modelId, fuelTypeId, colorId, sort, search, pageSize, pageIndex
        [HttpGet]
        public async Task<ActionResult<Pagination<VehicleToReturnDto>>> GetVehicles(
            [FromQuery] VehicleSpecParams vehicleParams)
        {
            var spec = new VehiclesSpecification(vehicleParams);

            var countSpec = new VehicleWithFiltersForCountSpecification(vehicleParams);

            var totalItems = await _unitOfWork.Repository<Vehicle>().CountAsync(countSpec);

            var vehicles = await _unitOfWork.Repository<Vehicle>().ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Vehicle>, IReadOnlyList<VehicleToReturnDto>>(vehicles);

            return Ok(new Pagination<VehicleToReturnDto>(vehicleParams.PageIndex,
                vehicleParams.PageSize, totalItems, data));
        }

        //სატრანსპორტო საშუალების დეტალური ინფორმაციის მიღება იდენტიფიკატორის მეშვეობით
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VehicleToReturnDto>> GetVehicle(int id)
        {
            var spec = new VehiclesSpecification(id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicle == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);
        }

        //სატრანსპორტო საშუალების დამატება
        [HttpPost]
        public async Task<ActionResult<VehicleToReturnDto>> CreateVehicle(VehicleCreateDto vehicleToCreate)
        {
            var validator = new VehicleValidator();
            var res = validator.Validate(vehicleToCreate);

            if (res.IsValid)
            {
                var vehicle = _mapper.Map<VehicleCreateDto, Vehicle>(vehicleToCreate);
                _unitOfWork.Repository<Vehicle>().Add(vehicle);

                var result = await _unitOfWork.Complete();

                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating vehicle"));

                vehicle.AddFuelType(vehicleToCreate.FuelTypeId, vehicle.Id);
                _unitOfWork.Repository<Vehicle>().Update(vehicle);
                var result2 = await _unitOfWork.Complete();
                if (result2 <= 0) return BadRequest(new ApiResponse(400, "Problem adding fuel type"));

                return _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);
            }
            else
                return BadRequest(res.Errors);
        }


        //სატრანსპორტო საშუალების რედაქტირება
        [HttpPut("{id}")]
        public async Task<ActionResult<VehicleToReturnDto>> UpdateVehicle(int id, VehicleCreateDto vehicleToUpdate)
        {
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(id);

            _mapper.Map(vehicleToUpdate, vehicle);

            _unitOfWork.Repository<Vehicle>().Update(vehicle);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem updating vehicle"));

            return _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);
        }


        //სატრანსპორტო საშუალების წაშლა
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(id);

            foreach (var photo in vehicle.Photos)
            {
                _photoService.DeleteFromDisk(photo);
            }

            _unitOfWork.Repository<Vehicle>().Delete(vehicle);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting vehicle"));

            return Ok();
        }

        //სატრანსპორტო საშუალების სურათის დამატება
        [HttpPut("{id}/photo")]
        public async Task<ActionResult<VehicleToReturnDto>> AddVehiclePhoto(int id, [FromForm] VehiclePhotoDto photoDto)
        {
            var spec = new VehiclesSpecification(id);
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (photoDto.Photo.Length > 0)
            {
                var photo = await _photoService.SaveToDiskAsync(photoDto.Photo);

                if (photo != null)
                {
                    vehicle.AddPhoto(photo.PictureUrl, photo.FileName);

                    _unitOfWork.Repository<Vehicle>().Update(vehicle);

                    var result = await _unitOfWork.Complete();

                    if (result <= 0) return BadRequest(new ApiResponse(400, "Problem adding photo product"));
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "problem saving photo to disk"));
                }
            }

            return _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);
        }


        //სატრანსპორტო საშუალების სურათის წაშლა
        [HttpDelete("{id}/photo/{photoId}")]
        public async Task<ActionResult> DeleteVehiclePhoto(int id, int photoId)
        {
            var spec = new VehiclesSpecification(id);
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            var photo = vehicle.Photos.SingleOrDefault(x => x.Id == photoId);

            if (photo != null)
            {
                if (photo.IsMain)
                    return BadRequest(new ApiResponse(400,
                        "You cannot delete the main photo"));

                _photoService.DeleteFromDisk(photo);
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Photo does not exist"));
            }

            vehicle.RemovePhoto(photoId);

            _unitOfWork.Repository<Vehicle>().Update(vehicle);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting photo vehicle"));

            return Ok();
        }

        //სატრანსპორტო საშუალების სურათის შეცვლა
        [HttpPost("{id}/photo/{photoId}")]
        public async Task<ActionResult<VehicleToReturnDto>> SetMainPhoto(int id, int photoId)
        {
            var spec = new VehiclesSpecification(id);
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicle.Photos.All(x => x.Id != photoId)) return NotFound();

            vehicle.SetMainPhoto(photoId);

            _unitOfWork.Repository<Vehicle>().Update(vehicle);

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem setting main photo vehicle"));

            return _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);
        }

        //სატრანსპორტო საშუალების მფლობელის დამატება
        [HttpPost("{id}/owner")]
        public async Task<ActionResult<VehicleToReturnDto>> AddVehicleOwner(int id, OwnerCreateDto ownerCreateDto)
        {
            var validator = new OwnerValidator();
            var res = validator.Validate(ownerCreateDto);
            if (res.IsValid)
            {
                var owner = _mapper.Map<OwnerCreateDto, Owner>(ownerCreateDto);
                var spec = new VehiclesSpecification(id);
                var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

                _unitOfWork.Repository<Owner>().Add(owner);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem creating owner"));

                vehicle.AddOwner(owner.Id, id);
                _unitOfWork.Repository<Vehicle>().Update(vehicle);
                var result2 = await _unitOfWork.Complete();
                if (result2 <= 0) return BadRequest(new ApiResponse(400, "Problem adding owner vehicle"));

                return _mapper.Map<Vehicle, VehicleToReturnDto>(vehicle);
            }
            else return BadRequest(res.Errors);
        }


        //სატრანსპორტო საშუალების მფლობელის წაშლა
        [HttpDelete("{id}/owner/{ownerId}")]
        public async Task<ActionResult> DeleteVehicleOwner(int id, int ownerId)
        {
            var spec = new VehiclesSpecification(id);
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            var owner = vehicle.VehicleOwners.SingleOrDefault(x => x.OwnerId == ownerId);

            if (owner != null)
            {
                vehicle.RemoveOwner(ownerId);

                _unitOfWork.Repository<Vehicle>().Update(vehicle);
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Owner does not exist"));
            }

            var result = await _unitOfWork.Complete();

            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem deleting owner vehicle"));

            return Ok();
        }

        //სატრანსპორტო საშუალების მფლობელთა სიის მიღება
        [HttpGet("{id}/owner")]
        public async Task<ActionResult<OwnerToReturnDto>> GetVehicleOwner(int id)
        {
            var spec = new VehicleOwnerSpecification(id);

            var vehicle = await _unitOfWork.Repository<Vehicle>().GetEntityWithSpec(spec);

            if (vehicle == null) return NotFound(new ApiResponse(404, "Vehicle not exists"));

            List<OwnerToReturnDto> owners = new List<OwnerToReturnDto>();
            foreach (var item in vehicle.VehicleOwners)
            {
                owners.Add(new OwnerToReturnDto
                {
                    Id = item.Owner.Id,
                    PrivateNumber = item.Owner.PrivateNumber,
                    FirstName = item.Owner.FirstName,
                    LastName = item.Owner.LastName
                });
            }

            return Ok(owners);
        }

    }
}