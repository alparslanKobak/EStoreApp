using P013EStore.Data;
using P013EStore.Data.Concrete;
using P013EStore.Service.Abstract;

namespace P013EStore.Service.Concrete
{
    public class ProductService : ProductRepository, IProductService 
    {
        // Interface'ler kumandanın tuş takımıdır. Tanımlanan classlar ise işi yapan içerideki mekanizmadır...
        public ProductService(DatabaseContext context) : base(context)
        {

        }
    }
}
