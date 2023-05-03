
using System;
using System.Collections.Generic;
using System.Linq.Expressions; // kENDİ LAMBDA eXpression kullanabileceğimiz metotları yazmamızı sağlayan kütüphane

namespace P013EStore.Data.Abstract
{
    public interface IRepository <T> where T: class // IRepositoryInterface'i dışarıdan alacağı T tipinde bir parametreyle çalışacak ve where şartı ile bu t'nin veri tipi bir class olmalıdır.
    {

        // Senkron Metotlar

        List<T> GetAll(); // Dbdeki tüm kayıtları çekmemizi sağlayacak metot imzası.

        List<T> GetAll(Expression <Func<T, bool>> expression); // "expression" Uygulamada verileri listelerken p=> p.IsActive vb gibi sorgulama ve filtreleme kodları kullanabilmemizi sağlar.

        T Get(Expression<Func<T, bool>> expression);

        T Find(int id);

        void Add(T entity);

        void Delete(T entity);

        void Update(T entity);

        int Save();

        // Asenkron metotlar

        Task<T> FindAsync(int id);

        Task<T> GetAsync(Expression<Func<T, bool>> expression); // Lambda expression kullanarak db de filtreleme yapıp geriye 1 tane kayıt döndürür

        Task<List<T>> GetAllAsync();

        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression); // Lambda expression kullanarak db de filtreleme yapıp geriye liste döndürür


        Task AddAsync(T entity);

        Task<int> SaveAsync(); // Asenkron kaydetme




    }
}
