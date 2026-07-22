using System.ComponentModel.DataAnnotations;
using System.Reflection;
using FluentValidation;

namespace PcBuilderBackend.Api.Filters;

public static class ValidationFilter
{
    public static EndpointFilterDelegate ValidationFilterFactory(EndpointFilterFactoryContext context, EndpointFilterDelegate next)
    {
        var validationDescriptors = GetValidators(context.MethodInfo, context.ApplicationServices).ToList();
        
        return validationDescriptors.Count != 0 ? invocationContext => Validate(validationDescriptors, invocationContext, next) : next;
    }

    private static async ValueTask<object?> Validate(IEnumerable<ValidationDescriptor> validationDescriptors, EndpointFilterInvocationContext invocationContext, EndpointFilterDelegate next)
    {
        foreach (var descriptor in validationDescriptors)
        {
            var argument = invocationContext.Arguments[descriptor.ArgumentIndex];

            if (argument is null) continue;
            var validationResult = await descriptor.Validator.ValidateAsync(
                new ValidationContext<object>(argument)
            );

            if (!validationResult.IsValid)
            {
                return TypedResults.ValidationProblem(validationResult.ToDictionary());
            }
        }

        return await next.Invoke(invocationContext);
    }
    
    private static IEnumerable<ValidationDescriptor> GetValidators(MethodInfo methodInfo,
        IServiceProvider serviceProvider)
    {
        foreach (var item in methodInfo.GetParameters().Select((parameter, index) => new { parameter, index }))
        {
            if (item.parameter.GetCustomAttribute<ValidateAttribute>() is null) continue;
            var validatorType = typeof(IValidator<>).MakeGenericType(item.parameter.ParameterType);

            if (serviceProvider.GetService(validatorType) is IValidator validator)
            {
                yield return new ValidationDescriptor { ArgumentIndex = item.index, ArgumentType = item.parameter.ParameterType, Validator = validator };
            }
        }
    }
}