using Hair.Repository.Abstract.SystemRepo;
using Hair.Repository.Abstract.UserRepo;

namespace Hair.Repository.Abstract
{
    public interface IUnitOfWork
    {
        IPageRepository Page { get; }
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IUserRoleRepository UserRole { get; }
        IRoleMenuRepository RoleMenu { get; }
        
        Task<int> SaveChanges();
    }
}
