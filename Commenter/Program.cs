using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Raven.Client.Document;
using Raven.Client.Extensions;

namespace Commenter
{
    class Program
    {
        static void Main(string[] args)
        {
            const string database = "comments";
            const string postId = "blogs/1/posts/1";
                
            var store = new DocumentStore
                                    {
                                        Url = "http://localhost:8080",
                                        DefaultDatabase = database
                                    };


            store.Initialize();
            store.DatabaseCommands.EnsureDatabaseExists(database);

            using (var session = store.OpenSession())
            {
                var comments = new List<Comment>
                                   {
                                       new Comment
                                           {
                                               PostId = postId,
                                               Who = "Jim",
                                               When = DateTime.Now,
                                               What = "This is a comment"
                                           },

                                       new Comment
                                           {
                                               PostId = postId,
                                               Who = "Bill",
                                               When = DateTime.Now,
                                               What = "This is another comment",
                                           },
                                       new Comment
                                           {
                                               PostId = postId,
                                               Who = "Jim",
                                               When = DateTime.Now,
                                               What = "You're crazy",
                                           },
                                   };

                comments.ForEach(session.Store);

                comments.Last().InReplyToCommentId = comments[1].Id;
                session.SaveChanges();
            }

            using(var session = store.OpenSession())
            {
                var comments =
                    session
                        .Query<Comment>()
                        .Where(x => x.PostId == postId)
                        .ToList();

                var c = comments.Count;
            }
        }

        public class Comment
        {
            public string Id { get; set; }
            public string PostId { get; set; }
            public string Who { get; set; }
            public DateTime When { get; set; }
            public string What { get; set; }
            public string InReplyToCommentId { get; set; }            
        }
    }
}
