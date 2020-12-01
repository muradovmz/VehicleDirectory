using System.Linq;
using System;
using Core.Entities;
using FluentValidation;
using API.Dtos;

namespace API.Validators
{
    public class VehicleValidator : AbstractValidator<VehicleCreateDto>
    {
        public VehicleValidator()
        {
            RuleFor(m => m.ModelId).NotEmpty().WithMessage("Set ModelId!");
            RuleFor(m => m.VinCode).NotEmpty().MinimumLength(10).MaximumLength(100).WithMessage("Enter VIN Code");
            RuleFor(m => m.StateNumberPlate).NotEmpty().WithMessage("Enter StateNumberPlate");
            RuleFor(m => m.ManufactureDate).NotEmpty().Must(date => date != default(DateTime)).WithMessage("Invalid date/time");
            RuleFor(m => m.ColorId).NotEmpty().WithMessage("Set color!");
            RuleFor(m => m.FuelTypeId).NotEmpty().WithMessage("Set FuelType!");
        }
    }
}