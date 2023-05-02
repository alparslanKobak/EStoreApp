
using System;
using System.Collections.Generic;
using System.Linq.Expressions; // kENDİ LAMBDA eXpression kullanabileceğimiz metotları yazmamızı sağlayan kütüphane

namespace P013EStore.Data.Abstract
{
    public interface IReposityory <T> where T: class // IRepositoryInterface'i dışarıdan alacağı T tipinde bir parametreyle çalışacak ve where şartı ile bu t'nin veri tipi bir class olmalıdır.
    {
        List<T> GetAll(); // Dbdeki tüm kayıtları çekmemizi sağlayacak metot imzası.

        List<T> GetAll(Expression <Func<T, bool>> expression); // "expression" Uygulamada verileri listelerken p=> p.IsActive vb gibi sorgulama ve filtreleme kodları kullanabilmemizi sağlar.

    }
}
