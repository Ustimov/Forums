using System;
using ServiceStack.WebHost.Endpoints;
using Forum.Services;
using ServiceStack.Common.Web;
using ServiceStack.Text;

namespace Forum
{
    public class Global : System.Web.HttpApplication
    {
        public class ForumAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public ForumAppHost() : base("Forum Web Services", typeof(CommonService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
                SetConfig(new EndpointHostConfig
                {
                    DefaultContentType = MimeTypes.Json,
                    //EnableFeatures = Feature.All.Remove(Feature.Metadata),
                    //AppendUtf8CharsetOnContentTypes = new HashSet<string> { ContentType.Json }
                });
                JsConfig.IncludeNullValues = true;
                /*
                this.ResponseFilters.Add((req, res, dto) => {
                    if (req.ResponseContentType == ContentType.Json)
                    {
                        req.
                        res.AddHeader(HttpHeaders.ContentDisposition,
                            string.Format("attachment;filename={0}.csv", req.OperationName));
                    }
                });
                */
            }
        }

        //Initialize your application singleton
        protected void Application_Start(object sender, EventArgs e)
        {
            new ForumAppHost().Init();
            
        }
    }
}