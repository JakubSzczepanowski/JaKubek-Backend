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
        private string[] allowedSortByColumnNames = new[] { nameof(Entities.File.Name), nameof(Entities.File.Description) };
        public BaseQueryValidator()
        {
            RuleFor(r => r.PageNumber).GreaterThanOrEqualTo(1);
            RuleFor(r => r.PageSize).Custom((value, context) =>
            {
                if (!allowedPageSizes.Contains(value))
                    context.AddFailure("PageSize", $"PageSize musi zawierać się w [{string.Join(",", allowedPageSizes)}]");
            });
            RuleFor(r => r.SortBy)
                .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                .WithMessage($"Sortowanie jest opcjonalne lub musi zawierać się w [{string.Join(",", allowedSortByColumnNames)}]");
        }
    }
}
