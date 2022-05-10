using Hair.Repository.Abstract;
using Hair.Repository.Abstract.SystemRepo;
using Hair.Repository.Abstract.UserRepo;
using Hair.Repository.Concrete.System;
using Hair.Repository.Concrete.user;
using Hair.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hair.Repository.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly ApplicationDbContext _context;
        public readonly PageRepository _pageRepository;
        public readonly UserRepository _userRepository;
        public readonly RoleRepository _roleRepository;
        public readonly UserRoleRepository _userRoleRepository;
        public readonly RoleMenuRepository _roleMenuRepository;
        

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IPageRepository Page => _pageRepository ?? new PageRepository(_context);
        public IUserRepository User => _userRepository ?? new UserRepository(_context);
        public IRoleRepository Role => _roleRepository ?? new RoleRepository(_context);
        public IUserRoleRepository UserRole => _userRoleRepository ?? new UserRoleRepository(_context);
        public IRoleMenuRepository RoleMenu => _roleMenuRepository ?? new RoleMenuRepository(_context);
        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
