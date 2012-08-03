using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Commenter.Documents
{
    public class Comment
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string Who { get; set; }
        public DateTime When { get; set; }
        public string What { get; set; }
        public string InReplyToCommentId { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, PostId: {1}, Who: {2}, When: {3}, InReplyToCommentId: {4}", Id, PostId, Who, When, InReplyToCommentId);
        }
    }
}
