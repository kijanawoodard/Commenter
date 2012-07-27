using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml.Serialization;
using Commenter.Documents;
using Commenter.Messages.Commands;
using Comments.Web.Indexes;
using Comments.Web.ViewModels;
using NServiceBus;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Linq;

namespace Comments.Web.Controllers
{
    public class CommentsController : Controller
    {
        public static DocumentStore DocumentStore { get; set; }
        public static IBus Bus { get; set; }

        public ActionResult Get(string id)
        {
            using (var session = DocumentStore.OpenSession())
            {

                var transformResult =
                    session
                        .Query<Comment, CommentIndex>()
                        .Where(x => x.PostId == id)
                        .As<CommentIndex.TransformResult>()
                        .FirstOrDefault();

                var comments = transformResult != null ? transformResult.Comments : new List<Comment>();
                var model = ThreadedComment.ParseComments(comments);

                return View(model);
            }
        }

        public ActionResult Post(CreateComment command)
        {
            Bus.Send(command);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
