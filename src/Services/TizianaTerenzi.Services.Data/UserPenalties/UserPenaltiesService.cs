namespace TizianaTerenzi.Services.Data.UserPenalties
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Common.Repositories;
    using TizianaTerenzi.Data.Models;
    using Z.EntityFramework.Plus;

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

        public async Task<bool> BlockUserAsync(string userId, string reasonToBeBlocked)
        {
            var user = await this.usersRepository
                .All()
                .SingleOrDefaultAsync(u => u.Id == userId && u.IsBlocked == false);

            if (user != null)
            {
                user.IsBlocked = true;
                user.ReasonToBeBlocked = reasonToBeBlocked ?? NotificationMessages.BannedUserDefaultMessage;
                user.IsDeleted = true;
                user.DeletedOn = DateTime.UtcNow;

                var userResult = await this.usersRepository.SaveChangesAsync();

                var logoutResult = await this.userManager.UpdateSecurityStampAsync(user);

                return userResult > 0 && logoutResult.Succeeded;
            }

            return false;
        }

        public async Task<bool> UnblockUserAsync(string userId)
        {
            var affectedRows = await this.usersRepository
                .AllWithDeleted()
                .Where(u => u.Id == userId && u.IsBlocked == true)
                .UpdateAsync(u => new ApplicationUser
                {
                    IsBlocked = false,
                    ReasonToBeBlocked = null,
                    IsDeleted = false,
                    DeletedOn = null,
                });

            return affectedRows == 1;
        }

        public async Task<IEnumerable<SelectListItem>> GetAllBlockedUsersAsync()
        {
            var blockedUsersUsernames = await this.usersRepository
                .AllWithDeleted()
                .Where(u => u.IsBlocked == true)
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
