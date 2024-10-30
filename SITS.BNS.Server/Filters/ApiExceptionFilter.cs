using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SITS.BNS.Common.Exceptions;

namespace SITS.BNS.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {

        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilter()
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                 { typeof(RecordExitsException), HandleRecordExitsException },
                  { typeof(CustomException), HandleCustomException },
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            HandleUnknownException(context);
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var details = new ValidationProblemDetails(exception.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = exception.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Title = exception.ErrorMessage,
                Detail = exception.Message
            };

            // context.Result = new NotFoundObjectResult(details);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status404NotFound
            };

            context.ExceptionHandled = false;
        }

        private void HandleCustomException(ExceptionContext context)
        {
            var exception = context.Exception as CustomException;

            var details = new ProblemDetails()
            {
                Status = exception.StatusCode,
                Title = exception.ErrorMessage,
                Detail = exception.Message
            };

            // context.Result = new NotFoundObjectResult(details);

            context.Result = new ObjectResult(details)
            {
                StatusCode = exception.StatusCode
            };

            context.ExceptionHandled = true;
        }

        private void HandleRecordExitsException(ExceptionContext context)
        {
            var exception = context.Exception as RecordExitsException;

            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status409Conflict,
                Title = exception.ErrorMessage,
                Detail = exception.Message
            };

            //context.Result = new NotFoundObjectResult(details);

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status409Conflict
            };

            context.ExceptionHandled = true;
        }
    }
}
