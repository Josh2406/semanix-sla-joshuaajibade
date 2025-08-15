using FluentValidation;
using FormsService.Application.Commands;
using FormsService.Application.Extensions;

namespace FormsService.Application.Validators
{
    public class CreateFormCommandValidator : AbstractValidator<CreateFormCommand>
    {
        public CreateFormCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

            RuleFor(x => x.JsonPayload)
                .NotEmpty().WithMessage("JsonPayload is required.")
                .Must(StringExtensions.IsValidJsonPayload)
                    .WithMessage("JsonPayload is not valid. Id, Label, Type and Order of all fields must be correctly inputted.");
        }
    }
}
