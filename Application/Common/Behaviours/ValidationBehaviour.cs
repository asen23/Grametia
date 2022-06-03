#region

using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

#endregion

namespace Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TResponse : class
    where TRequest : IValidateable, IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        if (!_validators.Any()) return await next();
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .Select(r => r.ToString())
            .ToList();

        if (!failures.Any()) return await next();
        var responseType = typeof(TResponse);

        if (!responseType.IsGenericType) return await next();
        var resultType = responseType.GetGenericArguments()[0];
        var invalidResponseType = typeof(ValidateableResponse<>).MakeGenericType(resultType);
 
        var invalidResponse =
            Activator.CreateInstance(invalidResponseType, null, string.Join("\n", failures)) as TResponse;
 
        return invalidResponse ?? throw new Exception("An error has occured");

    }
}