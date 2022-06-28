using Microsoft.AspNetCore.Authorization;

namespace Authentication.Requirements
{
    public class MinimumWorkYearsRequirement : IAuthorizationRequirement
    {
        public int MinimumYears { get; set; }

        public MinimumWorkYearsRequirement(int minimumYears) => MinimumYears = minimumYears;
    }

    public class MinimumWorkYearsRequirementHandler : AuthorizationHandler<MinimumWorkYearsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumWorkYearsRequirement requirement)
        {
            // return if there is no user claim
            if(!context.User.HasClaim(x => x.Type == "User")) return Task.CompletedTask; ;

            var minYears = DateTime.Parse(context.User.FindFirst(x => x.Type == "WorkYears").Value);
            var period = DateTime.Now - minYears;

            if (period.Days > 365 * requirement.MinimumYears) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
