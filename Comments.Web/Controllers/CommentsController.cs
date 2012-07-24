using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using Raven.Client;
using Raven.Client.Document;

namespace Comments.Web.Controllers
{
    public class CommentsController : ApiController
    {
        public static DocumentStore DocumentStore { get; set; }

        public IQueryable<Comment> Get(string id)
        {
            using (var session = DocumentStore.OpenSession())
            {

                var comments =
                    session
                        .Query<Comment>()
                        .Where(x => x.PostId == "blogs/1/posts/1");

                return comments;
            }
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
