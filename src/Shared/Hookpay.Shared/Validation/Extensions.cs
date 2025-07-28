
using FluentValidation;

namespace Hookpay.Shared.Validation
{
    //ref: https://stackoverflow.com/questions/71941109/fluent-validation-custom-response-asp-net-core-web-api
    public static class Extensions
    {
        public static async Task HandleValidationAsync<TRequest>(
            this IValidator<TRequest> validator, 
            TRequest request)
           
        {
            var validations = await validator.ValidateAsync(request);

            if(!validations.IsValid)
            {
                throw new ValidationException(validations.Errors?.First()?.ErrorMessage);
            }
        }
    }
}
