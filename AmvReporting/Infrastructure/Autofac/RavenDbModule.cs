using AmvReporting.Infrastructure.Configuration;
using Autofac;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Embedded;
using Raven.Database.Server;


namespace AmvReporting.Infrastructure.Autofac
{
    public class RavenDbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the document store as single instance,
            // initializing it on first use.
            builder.Register(x => CreateDocumentStore()).As<IDocumentStore>()
              .SingleInstance();

            // Register the session, opening a new session per lifetime scope.
            builder.Register(x => x.Resolve<IDocumentStore>().OpenSession())
                 .As<IDocumentSession>()
                 .InstancePerLifetimeScope();


            base.Load(builder);
        }


        public static EmbeddableDocumentStore CreateDocumentStore()
        {
            var store = new EmbeddableDocumentStore
                        {
                            DataDirectory = ConfigurationContext.Current.GetRavenDataPath(),
                        };

            if (ConfigurationContext.Current.EnableRavenStudio())
            {
                store.UseEmbeddedHttpServer = true;
                store.Configuration.VirtualDirectory = "raven_database";
                NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(80);
            }

            // on queries get all the written results.
            store.Conventions.DefaultQueryingConsistency = ConsistencyOptions.AlwaysWaitForNonStaleResultsAsOfLastWrite;

            store.Initialize();

            return store;
        }
    }
}