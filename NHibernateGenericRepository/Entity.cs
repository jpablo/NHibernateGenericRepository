namespace NHibernateGenericRepository
{
	abstract public class MinimalEntity<T> where T : MinimalEntity<T>
	{
		private static RepositoryContainer<T> _repository;
		public static RepositoryContainer<T> Repository
		{
			get { return _repository ?? (_repository = RepositoryContainerFactory.Build<T>()); }
		}

		virtual public int Save()
		{
			return Repository.Save((T)this);
		}
	}

	abstract public class Entity<T> : MinimalEntity<T> where T : Entity<T>
	{
		virtual public int Id { get; private set; }
	}
}