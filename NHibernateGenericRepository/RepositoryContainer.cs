using System;
using System.Collections.Generic;
using System.Linq;

namespace NHibernateGenericRepository
{
	public class RepositoryContainer<T>
	{
		private readonly IRepository _repository;

		private IRepository Repository
		{
			get { return _repository; }
		}

		public RepositoryContainer(IRepository repository)
		{
			_repository = repository;
		}

		public T Get(int id)
		{
			return Repository.Get<T>(id);
		}

		public T Load(int id)
		{
			return Repository.Load<T>(id);
		}

		public int Save(T obj)
		{
			return Repository.Save(obj);
		}

		public void Delete(T obj)
		{
			Repository.Delete(obj);
		}

		public IEnumerable<T> All()
		{
			return Repository.Query<T>();
		}

		public T FindBy(Func<T, bool> selector)
		{
			return Repository.Query<T>().FirstOrDefault(selector);
		}

		public IQueryable<T> Query
		{
			get { return Repository.Query<T>(); }
		}

	}

}
