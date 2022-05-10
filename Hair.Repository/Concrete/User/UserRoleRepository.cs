using Hair.Core.Models.User;
using Hair.Core.Repositories.Concrete;
using Hair.Repository.Abstract.UserRepo;
using Microsoft.EntityFrameworkCore;

namespace Hair.Repository.Concrete.user
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext context) : base(context)
        {
        }
    }
}
