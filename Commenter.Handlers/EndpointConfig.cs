using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using NServiceBus.Persistence.Raven;
using Raven.Client;
using Raven.Client.Document;
using NServiceBus.UnitOfWork;
using StructureMap;

namespace Commenter.Handlers
{
    class EndpointConfig : IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure
                .With()
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Events"))
                .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith(".Commands"))
                .DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith(".Messages"))
                .StructureMapBuilder()
                .JsonSerializer();
        }
    }

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
            });
        }
    }

    public class StartStop : IWantToRunAtStartup
    {
        public void Run()
        {
//            Console.TreatControlCAsInput = false;
//            Console.CancelKeyPress += OnCancelKeyPress;
        }

        public void Stop()
        {
            
        }

        internal void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
//            Console.WriteLine("hello");
//            var store = ObjectFactory.GetInstance<IDocumentStore>();
//            if (store != null)
//            {
//                Console.WriteLine("disposing raven store");
//                store.Dispose();
//            }
//
//            e.Cancel = true;
//           Stop();
        }
    }
}
