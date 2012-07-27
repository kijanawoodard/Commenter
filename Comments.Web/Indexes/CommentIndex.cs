using System.Collections.Generic;
using System.Linq;
using Commenter.Documents;
using Raven.Client.Indexes;

namespace Comments.Web.Indexes
{
    public class CommentIndex :
        AbstractIndexCreationTask<Comment, CommentIndex.ReduceResult>
    {
        public class ReduceResult
        {
            public string PostId { get; set; }
            public List<string> CommentIds { get; set; }
        }

        public class TransformResult
        {
            public string PostId { get; set; }
            public List<Comment> Comments { get; set; }
        }

        public CommentIndex()
        {
            Map = foos => from foo in foos
                          select new
                                     {
                                         foo.PostId,
                                         CommentIds = new string[] {foo.Id},
                                     };

            Reduce = results => from result in results
                                group result by result.PostId
                                into g
                                select new
                                           {
                                               PostId = g.Key,
                                               CommentIds = g.SelectMany(x => x.CommentIds),
                                           };

            TransformResults = (database, results) => from result in results
                                                      let comments = database.Load<Comment>(result.CommentIds)
                                                      select new
                                                                 {
                                                                     result.PostId,
                                                                     Comments = comments
                                                                 };
        }
    }
}