using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commenter.Messages.Commands;
using NServiceBus;

namespace Commenter.Handlers
{
    class CreateCommentHandler : IHandleMessages<CreateComment>
    {
        public void Handle(CreateComment message)
        {
            Console.WriteLine("Hello");
        }
    }
}
