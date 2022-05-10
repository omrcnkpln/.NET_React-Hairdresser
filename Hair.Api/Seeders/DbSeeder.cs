using Hair.Core.Models.User;
using Hair.Repository.Context;
using Hair.Service.Abstract.Helpers;
using Hair.Service.Abstract.UserService;

namespace Hair.Api
{
    public static class DbSeeder
    {
        //önce migrationlar yapılmalı sonra veri basılmalı
        public static async void Seed(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var _context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var _dataAcces = serviceScope.ServiceProvider.GetService<IDataAccessService>();
                var _credentialService = serviceScope.ServiceProvider.GetService<IUserService>();
                //Log.Information("Seeding database");

                //var company = CreateCompany(_context);
                var role = CreateRole(_context);
                var user = await CreateUser(_context, _credentialService, role);
                AddRoleToUser(_context, user, role);
                CreatePages(_context, _dataAcces, role);

            }
        }

        public static void AddRoleToUser(ApplicationDbContext _context, List<User> user, List<Role> role)
        {
            var userAdmin = user.FirstOrDefault(x => x.UserName == "admin");
            var userDefault = user.FirstOrDefault(x => x.UserName == "omrcnkpln");
            var roleAdmin = role.FirstOrDefault(x => x.Name == "admin");
            var roleDefault = role.FirstOrDefault(x => x.Name == "default");

            if (!_context.UserRoles.Any(x => x.UserId == userAdmin.Id && x.RoleId == roleAdmin.Id))
            {
                _context.UserRoles.Add(new UserRole()
                {
                    UserId = userAdmin.Id,
                    RoleId = roleAdmin.Id,
                    CreatedDate = DateTime.Now,
                    LogicalRef = Guid.NewGuid(),
                });

                _context.SaveChanges();
            }

            if (!_context.UserRoles.Any(x => x.UserId == userDefault.Id && x.RoleId == roleDefault.Id))
            {
                _context.UserRoles.Add(new UserRole()
                {
                    UserId = userDefault.Id,
                    RoleId = roleDefault.Id,
                    CreatedDate = DateTime.Now,
                    LogicalRef = Guid.NewGuid(),
                });

                _context.SaveChanges();
            }

            //return _context.UserRoles.FirstOrDefault(x => x.UserId == user.Id && x.RoleId == role.Id);
        }

        private static async Task<List<User>> CreateUser(ApplicationDbContext _context, IUserService _userService, List<Role> role)
        {
            if (!_context.Users.Any(x => x.UserName == "admin"))
            {
                var adminUser = new User()
                {
                    UserName = "admin",
                    Email = "admin@abrateknoloji.com",
                    Name = "ABRA TEKNOLOJI",
                    Password = _userService.PasswordToHash("admin0"),
                    Role = role.FirstOrDefault(x => x.Name == "admin"),
                    CreatedDate = DateTime.Now,
                    LogicalRef = Guid.NewGuid(),
                };

                _context.Users.Add(adminUser);
                //_context.SaveChanges();
                //await _credentialService.Register(user);
            }

            if (!_context.Users.Any(x => x.UserName == "omrcnkpln"))
            {
                var defaultUser = new User()
                {
                    UserName = "omrcnkpln",
                    Email = "o@o.com",
                    Name = "Omer Can KAPLAN",
                    Password = _userService.PasswordToHash("omrcnkpln0"),
                    Role = role.FirstOrDefault(x => x.Name == "default"),
                    CreatedDate = DateTime.Now,
                    LogicalRef = Guid.NewGuid(),

                };

                _context.Users.Add(defaultUser);
                _context.SaveChanges();
                //await _credentialService.Register(user);
            }

            //return _context.Users.FirstOrDefault(x => x.UserName == "admin");
            return _context.Users.ToList();
        }

        private static List<Role> CreateRole(ApplicationDbContext _context)
        {
            List<Role> roles = new List<Role>();

            if (!_context.Roles.Any(x => x.Name == "admin"))
            {
                var role = new Role()
                {
                    Name = "admin",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    LogicalRef = Guid.NewGuid(),
                };

                _context.Roles.Add(role);
                roles.Add(role);
                _context.SaveChanges();
            }

            if (!_context.Roles.Any(x => x.Name == "default"))
            {
                var role = new Role()
                {
                    Name = "default",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    LogicalRef = Guid.NewGuid()
                };

                _context.Roles.Add(role);
                roles.Add(role);
                _context.SaveChanges();
            }

            //return roles.ToList();
            return _context.Roles.ToList(); ;
        }

        //private static Company CreateCompany(ApplicationDbContext _context)
        //{
        //    if (!_context.Companies.Any(x => x.Name == "ABRA TEKNOLOJI"))
        //    {
        //        _context.Companies.Add(new Company()
        //        {
        //            Name = "ABRA TEKNOLOJI",
        //            Email = "info@abrateknoloji.com",
        //            CreatedDate = DateTime.Now,
        //            LogicalRef = Guid.NewGuid(),
        //            ProfitRate = 1,
        //            TotalWorkingHoursPerWeek = 45
        //        });

        //        _context.SaveChanges();
        //    }

        //    return _context.Companies.FirstOrDefault(x => x.Name == "ABRA TEKNOLOJI");
        //}

        public static void CreatePages(ApplicationDbContext _context, IDataAccessService _dataAccess, List<Role> role)
        {
            var Pages = _dataAccess.ReadAllPAges();

            foreach (var page in Pages)
            {
                if (!_context.Pages.Any(x => x.ActionName == page.ActionName && x.ControllerName == page.ControllerName))
                {
                    _context.Pages.Add(page);

                    var roleAdmin = role.FirstOrDefault(x => x.Name == "admin");

                    if (!_context.RoleMenus.Any(x => x.Role.Id == roleAdmin.Id && x.Page.Id == page.Id))
                    {
                        _context.RoleMenus.Add(new RoleMenu()
                        {
                            Role = roleAdmin,
                            Page = page,
                            LogicalRef = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                        });
                    }
                }
            }

            _context.SaveChanges();
        }
    }
}