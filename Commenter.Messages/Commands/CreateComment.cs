using System;

namespace Commenter.Messages.Commands
{
    public class CreateComment
    {
        public string PostId { get; set; }
        public string What { get; set; }

        public string Who { get; set; }
        public DateTime When { get; set; }
    }
}
