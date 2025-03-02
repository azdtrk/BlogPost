using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Application.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<ValidationBehaviour<TRequest, TResponse>> _logger;

        public ValidationBehaviour(
            IEnumerable<IValidator<TRequest>> validators,
            ILogger<ValidationBehaviour<TRequest, TResponse>> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                if (failures.Count > 0)
                {
                    _logger.LogError("Validation failures: {RequestType} {@Request} {@ValidationErrors}",
                        typeof(TRequest).Name,
                        request,
                        failures.Select(f => new { Property = f.PropertyName, Error = f.ErrorMessage }));

                    throw new ValidationException(failures);
                }
            }

            return await next();
        }
    }
} 