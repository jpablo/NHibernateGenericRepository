namespace NHibernateGenericRepository
{
	public static class RepositoryContainerFactory
	{
		public static IRepository Repository { get; set; }

		public static RepositoryContainer<T> Build<T>()
		{
			return new RepositoryContainer<T>(Repository);
		}
	}
}