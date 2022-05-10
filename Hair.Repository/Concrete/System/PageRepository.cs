using Hair.Core.Models.System;
using Hair.Core.Repositories.Concrete;
using Hair.Repository.Abstract.SystemRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Hair.Repository.Concrete.System
{
    public class PageRepository : GenericRepository<Page>, IPageRepository
    {
        public PageRepository(DbContext context) : base(context)
        {
        }
    }
}
