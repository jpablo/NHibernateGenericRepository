using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernateGenericRepository
{
	public interface IRepository
	{
		int Save<T>(T obj);
		T Load<T>(int id);
		T Get<T>(int id);
		void Delete<T>(T obj);
		IEnumerable<T> All<T>();
		T FindBy<T>(Func<T, bool> selector);
		IQueryable<T> Query<T>();
	}
}