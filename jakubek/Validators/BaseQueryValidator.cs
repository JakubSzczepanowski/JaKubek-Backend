using FluentValidation;
using jakubek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Validators
{
    public class BaseQueryValidator : AbstractValidator<BaseQuery>
    {
        private int[] allowedPageSizes = new[] { 5,10,15};
        public BaseQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                    context.AddFailure("PageSize", $"PageSize musi zawierać się w [{string.Join(",", allowedPageSizes)}]");
            });
        }
    }
}
