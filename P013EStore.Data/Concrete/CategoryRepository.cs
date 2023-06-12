using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using P013EStore.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Data.Concrete
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(DatabaseContext context) : base(context)
        {
        }

        public async Task<Category> GetCategoryByIncludeAsync(int id)
        {
            return await _dbSet.Where(c => c.Id == id).AsNoTracking().Include(c=> c.Products).FirstOrDefaultAsync();
        }
    }
}
