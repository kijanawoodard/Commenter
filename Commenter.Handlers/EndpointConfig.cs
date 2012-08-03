using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

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
}
