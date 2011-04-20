using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernateGenericRepository
{
    public class MemRepository : IRepository
    {
        static public readonly List<dynamic> Data = new List<dynamic>();

        public int Save<T>(T obj)
        {
            Data.Add(obj);
            return Data.Count();
        }

        public T Load<T>(int id)
        {
            return Data[id];
        }

        public T Get<T>(int id)
        {
            return Data[id];
        }

        public void Delete<T>(T obj)
        {
            Data.Remove(obj);
        }

        public IEnumerable<T> All<T>()
        {
            return Data.Select(x => (T) x);
        }

        public T FindBy<T>(Func<T, bool> selector)
        {
            return Query<T>().FirstOrDefault(selector);
        }

        public IQueryable<T> Query<T>()
        {
            return Data.Select(x => (T)x).AsQueryable();
        }
    }
}