using FluentValidation;
using web_API_MongoDB.Models;

namespace web_API_MongoDB.Validators
{
    public class RoomValidator : AbstractValidator<Room>
    {
        public RoomValidator()
        {
            RuleFor(r => r.RoomNumber)
                .GreaterThan(0).WithMessage("Номер кімнати має бути додатним числом.");

            RuleFor(r => r.DormitoryNumber)
                .GreaterThan(0).WithMessage("Номер гуртожитку має бути додатним числом.");

            RuleFor(r => r.Capacity)
                .InclusiveBetween(1, 10).WithMessage("Місткість кімнати має бути від 1 до 10.");
        }
    }
}