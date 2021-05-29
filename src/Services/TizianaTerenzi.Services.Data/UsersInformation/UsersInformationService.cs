using System.Collections.Generic;
using System.Threading.Tasks;
using TizianaTerenzi.Data.Common.Repositories;
using TizianaTerenzi.Data.Models;
using TizianaTerenzi.Web.ViewModels.UsersInformation;
using TizianaTerenzi.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace TizianaTerenzi.Services.Data.UsersInformation
{
    public class UsersInformationService : IUsersInformationService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersInformationService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<IEnumerable<ApplicationUserViewModel>> GetAllUsersAsync()
        {
            var allUsers = await this.usersRepository
                .AllAsNoTracking()
                .To<ApplicationUserViewModel>()
                .ToListAsync();

            return allUsers;
        }
    }
}
