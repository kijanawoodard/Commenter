namespace Commenter.Messages.Events
{
    public interface ICommentCreated
    {
        string CommentId { get; set; }
        string UserId { get; set; }
    }
}