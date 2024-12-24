using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace VOEConsulting.Flame.BasketContext.Api.Extensions
{
    public static class ProblemDetailsExtensions
    {
        public static ProblemDetails CreateNotFound(
            this ProblemDetailsFactory detailsFactory,
            HttpContext context,
            string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status404NotFound, context, details, errors);
        }

        public static ProblemDetails CreateBadRequest(
            this ProblemDetailsFactory detailsFactory,
            HttpContext context,
            string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status400BadRequest, context, details, errors);
        }

        public static ProblemDetails CreateConflict(
            this ProblemDetailsFactory detailsFactory,
            HttpContext context,
            string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status409Conflict, context, details, errors);
        }

        public static ProblemDetails CreateValidation(
           this ProblemDetailsFactory detailsFactory,
           HttpContext context,
           string? details = null,
            IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status400BadRequest, context, details, errors);
        }

        public static ProblemDetails CreateUnexpectedResponse(
           this ProblemDetailsFactory detailsFactory,
           HttpContext context,
           string? details = null,
           IEnumerable<string>? errors = null)
        {
            return CreateProblemDetailsWith(detailsFactory, StatusCodes.Status500InternalServerError, context, details, errors);
        }


        private static ProblemDetails CreateProblemDetailsWith(ProblemDetailsFactory detailsFactory, int statusCode,
           HttpContext context,
           string? message = null,
           IEnumerable<string>? errors = null)
        {
            if (errors != null && errors.Any())
            {
                StringBuilder errorList = new StringBuilder();
                errorList.AppendJoin(",", errors);

                return detailsFactory.CreateProblemDetails(context, statusCode: statusCode, detail: errorList.ToString());
            }
            else
                return detailsFactory.CreateProblemDetails(context, statusCode: statusCode, detail: message);
        }
    }
}
