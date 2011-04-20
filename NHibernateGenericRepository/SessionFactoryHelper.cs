using System;
using NHibernate;
using NHibernate.Context;

namespace Tests
{
	public class SessionFactoryHelper
	{
		public static ISessionFactory SessionFactory { get; set; }

		public static void BindSession()
		{
			var session = SessionFactory.OpenSession();
			session.BeginTransaction();
			CurrentSessionContext.Bind(session);
		}

		public static void UnbindSession()
		{
			var session = CurrentSessionContext.Unbind(SessionFactory);

			if (session == null) return;

			try
			{
				session.Transaction.Commit();
			}
			catch (Exception)
			{
				session.Transaction.Rollback();
				throw;
			}
			finally
			{
				session.Close();
				session.Dispose();
			}
		}

	}
}