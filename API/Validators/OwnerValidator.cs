using API.Dtos;
using Core.Entities;
using FluentValidation;

namespace API.Validators
{
    public class OwnerValidator : AbstractValidator<OwnerCreateDto>
    {
        public OwnerValidator()
        {
            RuleFor(m => m.FirstName).NotEmpty().WithMessage("Enter FirstName!");
            RuleFor(m => m.LastName).NotEmpty().WithMessage("Enter LastName!");
            RuleFor(m => m.PrivateNumber).NotEmpty().Length(11).WithMessage("Enter PrivateNumber");
        }
    }
}