using P013EStore.Core.Entities;
using System.Linq.Expressions;

namespace P013EStore.Data.Abstract
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductByIncludeAsync(int id); // bu metot bize ürüne marka ve kategori include edilmiş 1 tane kayıt getirecek

        Task<List<Product>> GetProductsByIncludeAsync(); // Tüm ürünleri marka ve kategorisiyle getirecek metot
        
        Task<List<Product>> GetProductsByIncludeAsync(Expression<Func<Product, bool>> expression);  // Tüm ürünleri marka ve kategorisiyle lambda expression filtre uygulayarak getirecek metot
    }
}
