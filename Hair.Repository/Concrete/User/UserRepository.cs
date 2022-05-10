using Hair.Core.Models.User;
using Hair.Core.Repositories.Concrete;
using Hair.Repository.Abstract.UserRepo;
using Microsoft.EntityFrameworkCore;

namespace Hair.Repository.Concrete.user
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }
    }
}