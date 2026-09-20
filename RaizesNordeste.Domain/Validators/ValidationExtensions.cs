using FluentValidation;
using RaizesNordeste.Domain.Exceptions;

namespace RaizesNordeste.Domain.Validators;

public static class ValidationExtensions
{
    public static void ValidaOuLancaExcecao<T>(this IValidator<T> validator, T instance)
    {
        var result = validator.Validate(instance);

        if (!result.IsValid)
        {
            throw new DomainException(string.Join(" ", result.Errors.Select(e => e.ErrorMessage)));
        }
    }
}
