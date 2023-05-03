using P013EStore.Core.Entities;
using P013EStore.Data.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P013EStore.Service.Abstract
{
    public interface IService<T> : IRepository<T> where T : class, IEntity, new()  // Where sonrası belirttiklerimiz, olması gereken, varsayılan şartları söyler.
    {

    }
}
