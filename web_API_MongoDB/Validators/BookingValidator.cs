using FluentValidation;
using web_API_MongoDB.Models;

namespace web_API_MongoDB.Validators
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator()
        {
            RuleFor(b => b.StudentId)
                .NotEmpty().WithMessage("StudentId є обов'язковим.")
                .Length(24).WithMessage("StudentId має бути коректним ObjectId (24 символи).");
            

            RuleFor(b => b.RoomId)
                .NotEmpty().WithMessage("RoomId є обов'язковим.")
                .Length(24).WithMessage("RoomId має бути коректним ObjectId (24 символи).");
        }
    }
}