using FluentValidation;
using web_API_MongoDB.Models;

namespace web_API_MongoDB.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(s => s.FirstName)
                .NotEmpty().WithMessage("Ім'я є обов'язковим.")
                .MaximumLength(50).WithMessage("Ім'я не може бути довшим за 50 символів.");

            RuleFor(s => s.LastName)
                .NotEmpty().WithMessage("Прізвище є обов'язковим.");

            RuleFor(s => s.Email)
                .NotEmpty().WithMessage("Email є обов'язковим.")
                .EmailAddress().WithMessage("Некоректний формат email.");

            RuleFor(s => s.Course)
                .InclusiveBetween(1, 6).WithMessage("Номер курсу повинен бути в діапазоні від 1 до 6.");

            RuleFor(s => s.PhoneNumber)
                .NotEmpty().WithMessage("Номер телефону є обов'язковим.")
                .Matches(@"^\+?380\d{9}$").WithMessage("Некоректний формат телефону. Очікується +380XXXXXXXXX.");
        }
    }
}