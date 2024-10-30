using System.ComponentModel.DataAnnotations;

namespace MBS.Application.ValidationAttributes;

public class EnumValidation : ValidationAttribute
{
    private readonly Type _enumType;

    public EnumValidation(Type enumType)
    {
        if (!enumType.IsEnum)
            throw new ArgumentException("T must be an enum type");
        _enumType = enumType;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null && !Enum.IsDefined(_enumType, value))
        {
            return new ValidationResult($"Invalid value for {validationContext.DisplayName}");
        }
        return ValidationResult.Success;
    }
}