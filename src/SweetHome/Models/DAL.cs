using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
 
namespace SweetHome.Models
{
    public static class DAL
    {
        public static ISessionFactory CreateSessionFactory()
        {
            return Fluently
                .Configure()
                    .Database(
                        PostgreSQLConfiguration.Standard
                        .ConnectionString(c =>
                            c.Host("localhost")
                            .Port(5432)
                            .Database("sweet_home")
                            .Username("sweet_home")
                            .Password("1q2w3e4r")))
                    .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Shelter.ShelterMap>())
                    .ExposeConfiguration(TreatConfiguration)
                .BuildSessionFactory();
        }
        private static void TreatConfiguration(Configuration configuration)
        {
            // dump sql file for debug
            Action<string> updateExport = x =>
            {
                using (var file = new System.IO.FileStream(@"update.sql", System.IO.FileMode.Append, System.IO.FileAccess.Write))
                using (var sw = new System.IO.StreamWriter(file))
                {
                    sw.Write(x);
                    sw.Close();
                }
            };
            var update = new SchemaUpdate(configuration);
            update.Execute(updateExport, true);
        }
    }
}