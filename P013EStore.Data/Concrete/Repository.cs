using Microsoft.EntityFrameworkCore;
using P013EStore.Core.Entities;
using P013EStore.Data.Abstract;
using System.Linq.Expressions;

namespace P013EStore.Data.Concrete
{ // T = (Product olabilir, Brand olabilir vs. class'tır.)
    public class Repository<T> : IRepository<T> where T : class, IEntity, new() // Repository class'ına gönderilecek T'nin şartları : T(brand, category, product vb.) bir class olmalı, IEntity den implemente almalı ve new'lenebilir olmalı. 
    {
        internal DatabaseContext _context; // _context'in yazım sebebi _ ile başlayan farklı bir element olmadığı için, çağırırken bizlere kolaylık sağlaması adına oluşturuldu.

        internal DbSet<T> _dbSet; // Boş bir dbset tanımladık. Repository'e gönderilecek T class'ını parametre verdik.

        public Repository(DatabaseContext context) // S.O.L.I.D prensipleri
        {
            _context = context; // _context'i burada doldurduk. Aşağıda kullanabilmek için... (Doldurmadan kullanmak istesek null reference hatası olur.)

            _dbSet = context.Set<T>(); // Repository'e gönderilecek T class'ı için context üzerindeki db sete göre kendini ayarla.
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList(); // Eğer sadece listeleme yapacaksak yani liste üzerinde kayıt güncelleme vb. işlem yapmayacaksak entity frameworkdeki AsNoTracking yöntemi ile listeyi daha performanslı çekebiliriz.
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().Where(expression).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public int Save()
        {
            return _context.SaveChanges(); // Entity Framework'de SaveChanges(ekle,güncelle,sil vb işlemleri db ye işleyen metot) metodu direk context üzerinden çalışır, dbset in böyle bir metodu olmadığı için _context.SaveChanges() diyerek database contextimiz üzerindeki tüm işlemleri (ekleme, güncelleme, silme vb.) veritabanına yansıtmamızı sağlayan bu metodu 1 kere çağırmanız gerekli yoksa işlemler db ye işlemez!!
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
             _dbSet.Update(entity);
        }

       
    }
}
