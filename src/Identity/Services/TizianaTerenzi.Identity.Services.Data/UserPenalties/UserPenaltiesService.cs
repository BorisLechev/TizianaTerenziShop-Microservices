namespace TizianaTerenzi.Identity.Services.Data.UserPenalties
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Common.Data.Repositories;
    using TizianaTerenzi.Common.Messages.Administration;
    using TizianaTerenzi.Common.Services.ServiceRegistrationAttributes;
    using TizianaTerenzi.Identity.Data.Models;

    [TransientRegistration]
    public class UserPenaltiesService : IUserPenaltiesService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserPenaltiesService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
        }

        public async Task<bool> BlockUserAsync(UserBlockedMessage message)
        {
            var user = await this.usersRepository
                            .All()
                            .SingleOrDefaultAsync(u => u.Id == message.UserId && u.IsBlocked == false);

            if (user != null)
            {
                user.IsBlocked = true;
                user.ReasonToBeBlocked = message.ReasonToBeBlocked ?? NotificationMessages.BannedUserDefaultMessage;
                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;

                var userResult = await this.usersRepository.SaveChangesAsync();

                var logoutResult = await this.userManager.UpdateSecurityStampAsync(user);

                return userResult > 0 && logoutResult.Succeeded;
            }

            return false;
        }

        public async Task<bool> UnblockUserAsync(UserUnblockedMessage message)
        {
            var affectedRows = await this.usersRepository
                                   .AllWithDeleted()
                                   .Where(u => u.Id == message.UserId && u.IsBlocked == true)
                                   .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(u => u.IsBlocked, false)
                                        .SetProperty(u => u.ReasonToBeBlocked, (string?)null)
                                        .SetProperty(u => u.IsDeleted, false)
                                        .SetProperty(u => u.DeletedOn, (DateTime?)null));

            return affectedRows == 1;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllBlockedUsersAsync()
        {
            var blockedUsersUsernames = await this.usersRepository
                                            .AllWithDeleted()
                                            .Where(u => u.IsBlocked)
                                            .Select(u => new SelectListItem
                                            {
                                                Value = u.Id,
                                                Text = u.UserName,
                                            })
                                            .ToListAsync();

            return blockedUsersUsernames;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllUnblockedUsersAsync()
        {
            var unblockedUsersUsernames = await this.usersRepository
                                                .All()
                                                .Where(u => u.IsBlocked == false)
                                                .Select(u => new SelectListItem
                                                {
                                                    Value = u.Id,
                                                    Text = u.UserName,
                                                })
                                                .ToListAsync();

            return unblockedUsersUsernames;
        }
    }
}
