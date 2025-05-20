namespace TizianaTerenzi.Identity.Web.Services
{
    using Google.Protobuf.WellKnownTypes;
    using Grpc.Core;
    using gRPCProfileServer;
    using Microsoft.AspNetCore.Identity;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Services.Identity;
    using TizianaTerenzi.Identity.Data.Models;
    using TizianaTerenzi.Identity.Services.Data.Profile;

    public class ProfileGrpcService : Profile.ProfileBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IProfileService profileService;
        private readonly ICurrentUserService currentUserService;

        public ProfileGrpcService(
            UserManager<ApplicationUser> userManager,
            IProfileService profileService,
            ICurrentUserService currentUserService)
        {
            this.userManager = userManager;
            this.profileService = profileService;
            this.currentUserService = currentUserService;
        }

        public override async Task<StatusResponse> GetUsersPersonalDataForExport(PersonalDataForExportRequest request, ServerCallContext context)
        {
            var user = await this.userManager.FindByIdAsync(request.UserId);
            var passwordValid = !await this.userManager.HasPasswordAsync(user) ||
                                (request.Password != null &&
                                await this.userManager.CheckPasswordAsync(user, request.Password));

            if (!passwordValid)
            {
                return new StatusResponse { Succeeded = false, FailedMessage = NotificationMessages.InvalidPassword };
            }

            var usersCommentsAndVotes = await this.profileService.GetPersonalDataForUserJsonAsync(user.Id);

            return new StatusResponse { Succeeded = true, Data = new PersonalDataForExportResponse { FirstName = usersCommentsAndVotes.FirstName, LastName = usersCommentsAndVotes.LastName, Email = usersCommentsAndVotes.Email, CreatedOn = usersCommentsAndVotes.CreatedOn.ToString(), Town = usersCommentsAndVotes.Town, PostalCode = usersCommentsAndVotes.PostalCode, CountryName = usersCommentsAndVotes.CountryName, Address = usersCommentsAndVotes.Address } };
        }
    }
}
