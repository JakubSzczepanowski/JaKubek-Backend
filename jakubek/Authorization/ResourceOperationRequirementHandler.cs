using jakubek.Entities;
using jakubek.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jakubek.Authorization
{
    public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Entities.File>
    {
        private readonly IUserContextService _userContextService;
        public ResourceOperationRequirementHandler(IUserContextService userContextService)
        {
            _userContextService = userContextService;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ResourceOperationRequirement requirement, File resource)
        {
            if (requirement.ResourceOperation == ResourceOperation.Create ||
                requirement.ResourceOperation == ResourceOperation.Read)
                context.Succeed(requirement);

            int userId = _userContextService.GetUserId;
            if (resource.UserId == userId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
