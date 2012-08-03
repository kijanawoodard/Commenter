using System;
using Commenter.Documents;
using Commenter.Messages.Commands;
using Commenter.Messages.Events;
using NServiceBus;
using Raven.Client;

namespace Commenter.Handlers
{
    class CreateCommentHandler : IHandleMessages<CreateComment>
    {
        public IBus Bus { get; set; }
        public IDocumentSession Session { get; set; }

        public void Handle(CreateComment command)
        {
            var comment =
                new Comment
                    {
                        PostId = command.PostId,
                        Who = command.Who,
                        When = DateTime.UtcNow,
                        What = command.What
                    };

            Session.Store(comment);
            
            Bus.Publish<ICommentCreated>(m =>
                                             {
                                                 m.CommentId = comment.Id;
                                                 m.UserId = comment.Who;
                                             });

            Console.WriteLine("Comment Saved: {0}", comment);
        }
    }
}
