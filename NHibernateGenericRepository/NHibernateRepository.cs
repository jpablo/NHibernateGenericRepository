using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace NHibernateGenericRepository
{
	public delegate ISession SessionSource();

	public class NHibernateRepository : IRepository
	{
		private readonly ISessionFactory _sessionFactory;

		public NHibernateRepository(ISessionFactory factory)
		{
			_sessionFactory = factory;
		}

		public ISession Session
		{
			get { return _sessionFactory.GetCurrentSession(); }
		}


		public int Save<T>(T obj)
		{
			return (int)Session.Save(obj);
		}

		public T Load<T>(int id)
		{
			return Session.Load<T>(id);
		}

		public T Get<T>(int id)
		{
			return Session.Get<T>(id);
		}

		public void Delete<T>(T obj)
		{
			Session.Delete(obj);
		}

		public IEnumerable<T> All<T>()
		{
			return Session.Query<T>();
		}

		public T FindBy<T>(Func<T, bool> selector)
		{
			return Session.Query<T>().FirstOrDefault(selector);
		}

		public IQueryable<T> Query<T>()
		{
			return Session.Query<T>();
		}

	}
}