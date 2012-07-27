using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Commenter.Documents;
using Commenter.Messages.Commands;
using NServiceBus;
using Raven.Client;

namespace Commenter.Handlers
{
    class CreateCommentHandler : IHandleMessages<CreateComment>
    {
        private readonly IDocumentSession _session;

        public CreateCommentHandler(IDocumentSession session)
        {
            _session = session;
        }

        public void Handle(CreateComment message)
        {
            var comment =
                new Comment
                    {
                        PostId = message.PostId,
                        Who = message.Who,
                        When = DateTime.UtcNow,
                        What = message.What
                    };

            _session.Store(comment);
            
            Console.WriteLine("Comment Saved");
        }
    }
}
