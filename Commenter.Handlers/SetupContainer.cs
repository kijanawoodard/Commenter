using NServiceBus;
using NServiceBus.UnitOfWork;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;

namespace Commenter.Handlers
{
    public class SetupContainer : IWantCustomInitialization
    {
        public void Init()
        {
            const string database = "comments";
            var store = new DocumentStore
                            {
                                Url = "http://localhost:8080",
                                DefaultDatabase = database
                            };
            store.Initialize();

            ObjectFactory.Configure(c =>
                                        {
                                            c.For<IDocumentStore>()
                                                .Singleton()
                                                .Use(store);

                                            c.For<IDocumentSession>()
                                                .Use(ctx => ctx.GetInstance<IDocumentStore>().OpenSession());

                                            c.For<IManageUnitsOfWork>()
                                                .Use<RavenUnitOfWork>();

                                            c.SetAllProperties(x => x.OfType<IDocumentSession>());
                                        });
        }
    }
}