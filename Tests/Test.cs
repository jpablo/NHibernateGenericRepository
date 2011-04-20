using System;
using System.IO;
using System.Linq;
using Data;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NHibernateGenericRepository;
using NUnit.Framework;

namespace Data
{
	public class Derived : Entity<Derived>
	{
		virtual public string Name { get; set; }
	}
}

namespace Tests
{

	public class DefaultNHConfiguration : DefaultAutomappingConfiguration
	{
		public override bool ShouldMap(Type type)
		{
			return type.Namespace == "Data";
		}
	}

	public static class NHibernateConfig
	{
		private const string DbFile = "firstProject.db";

		public static ISessionFactory CreateSessionFactory()
		{
			var cfg = new DefaultNHConfiguration();

			return Fluently.Configure()
			  .Database(SQLiteConfiguration.Standard.UsingFile(DbFile))
			  .Mappings(m => m.AutoMappings.Add(AutoMap.AssemblyOf<Derived>(cfg).IgnoreBase<Entity<Derived>>()))
			  .ExposeConfiguration(BuildSchema)
			  .BuildSessionFactory();
		}
		private static void BuildSchema(Configuration config)
		{
			// delete the existing db on each run
			if (File.Exists(DbFile))
				File.Delete(DbFile);

			// this NHibernate tool takes a configuration (with mapping info in)
			// and exports a database schema from it
			new SchemaExport(config).Create(false, true);

			config.SetProperty("current_session_context_class", "thread_static");
		}
	}

	[TestFixture]
	public class TestIRepository
	{

		public TestIRepository()
		{
			RepositoryContainerFactory.Repository = new MemRepository();
		}


		[SetUp]
		public void SetUp()
		{
			MemRepository.Data.Clear();
		}

		[TearDown]
		public void TearDown()
		{

		}

		[Test]
		public void Save()
		{

			var d1 = new Derived { Name = "xxx" };
			var d2 = new Derived { Name = "yyy" };
			d1.Save();
			d2.Save();

			Assert.AreEqual(2, Derived.Repository.All().Count());
			Assert.Contains(d1, Derived.Repository.All().ToArray());
			Assert.Contains(d2, Derived.Repository.All().ToArray());
			Assert.That(d1, Has.Property("Name").EqualTo("xxx"));
		}
	}


	[TestFixture]
	public class TestNHibernateRepository
	{

		public TestNHibernateRepository()
		{
			SessionFactoryHelper.SessionFactory = NHibernateConfig.CreateSessionFactory();
			RepositoryContainerFactory.Repository = new NHibernateRepository(SessionFactoryHelper.SessionFactory);
		}

		[SetUp]
		public void SetUp()
		{
			SessionFactoryHelper.BindSession();
		}

		[TearDown]
		public void TearDown()
		{
			SessionFactoryHelper.UnbindSession();
		}

		[Test]
		public void Save()
		{
			var d = new Derived { Name = "xxx" };
			d.Save();

			(new Derived { Name = "yyy" }).Save();

			Assert.That(1,Is.EqualTo(d.Id));

			var ds = Derived.Repository.All();
			Assert.That(2, Is.EqualTo(ds.Count()));

			var xxx = Derived.Repository.Query.Where(x => x.Name == "xxx");
			Assert.That(1, Is.EqualTo(xxx.Count()));
		}
	}
}
