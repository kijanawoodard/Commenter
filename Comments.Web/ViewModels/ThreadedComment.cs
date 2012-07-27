using System;
using System.Collections.Generic;
using System.Linq;
using Commenter.Documents;

namespace Comments.Web.ViewModels
{
    public class ThreadedComment
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Who { get; set; }
        public DateTime When { get; set; }
        public string What { get; set; }
        public List<Comment> Replies { get; set; }

        public static List<ThreadedComment> ParseComments(List<Comment> comments)
        {
            var root =
                comments
                    .Where(x => x.InReplyToCommentId == null)
                    .Select(x => new ThreadedComment
                                     {
                                         Id = x.Id,
                                         PostId = x.PostId,
                                         Who = x.Who,
                                         When = x.When,
                                         What = x.What
                                     })
                    .ToList();

            root.ForEach(x =>
                             {
                                 x.Replies = comments.Where(y => y.InReplyToCommentId == x.Id).ToList();
                                 if (x.Replies.Count == 0)
                                     x.Replies = null;
                             });
            return root;
        }
    }
}