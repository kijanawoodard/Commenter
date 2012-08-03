using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Comments.Web.Controllers;
using Comments.Web.Indexes;
using NServiceBus;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Raven.Client.Document;
using Raven.Client.Extensions;
using Raven.Client.Indexes;

namespace Comments.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{*id}",
                defaults: new { id = RouteParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{*id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            BundleTable.Bundles.RegisterTemplateBundles();

            const string database = "comments";
            
            CommentsController.DocumentStore =
                new DocumentStore
                    {
                        Url = "http://localhost:8080",
                        DefaultDatabase = database
                    };

            CommentsController.DocumentStore.Initialize();
            CommentsController.DocumentStore.DatabaseCommands.EnsureDatabaseExists(database);
            IndexCreation.CreateIndexes(typeof(CommentIndex).Assembly, CommentsController.DocumentStore);

            CommentsController.Bus =
                Configure.With()
                    .DefaultBuilder()
    //                .ForMvc()
                    .JsonSerializer()
                    .Log4Net()
                    .MsmqTransport()
                        .IsTransactional(false)
                        .PurgeOnStartup(true)
                    .UnicastBus()
                        .ImpersonateSender(false)
                    .CreateBus()
                    // install if you need to.
                    .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }
    }

    public static class ObjectExtensions
    {
        public static string ToJson(this object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.None,
                                               new JsonSerializerSettings()
                                               {
                                                   ContractResolver = new CamelCasePropertyNamesContractResolver(),
                                                   Converters = new List<JsonConverter>
                                                                {
                                                                    new IsoDateTimeConverter() //tODO: Blog - for knockout http://james.newtonking.com/archive/2009/02/20/good-date-times-with-json-net.aspx
                                                                }
                                               });
        }

        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        //http://stackoverflow.com/a/222761/214073
        //Really dirty - and delicious
        public static T JsonCopy<T>(this T target)
        {
            return target.ToJson().FromJson<T>();
        }

        public static TResult NullOr<T, TResult>(this T foo, Func<T, TResult> func) where T : class
        {
            if (foo == null) return default(TResult);
            return func(foo);
        }
    }
}