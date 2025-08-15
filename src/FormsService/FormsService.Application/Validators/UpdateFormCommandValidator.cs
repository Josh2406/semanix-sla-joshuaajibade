using FluentValidation;
using FormsService.Application.Commands;
using FormsService.Application.Extensions;

namespace FormsService.Application.Validators
{
    public class UpdateFormCommandValidator: AbstractValidator<UpdateFormCommand>
    {
        public UpdateFormCommandValidator()
        {
            RuleFor(x => x.FormId)
                .NotNull().WithMessage("FormId is required.")
                .Must(IsValidGuid).WithMessage("FormId is not valid.");

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

        private bool IsValidGuid(Guid guid)
        {
            return guid != Guid.Empty;
        }
    }
}
