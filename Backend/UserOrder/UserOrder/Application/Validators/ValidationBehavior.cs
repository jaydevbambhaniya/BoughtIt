using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserOrder.Application.Validators
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validator)
        {
            _validators = validator;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators.Select(validator => validator.Validate(context))
                           .SelectMany(result=> result.Errors)
                           .Where(f => f!= null)
                           .ToList();
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
