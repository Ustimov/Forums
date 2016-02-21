using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ServiceStack.WebHost.Endpoints;
using Forum.Services;

namespace Forum
{
    public class Global : System.Web.HttpApplication
    {
        public class ForumAppHost : AppHostBase
        {
            //Tell Service Stack the name of your application and where to find your web services
            public ForumAppHost() : base("Forum Web Services", typeof(HelloService).Assembly) { }

            public override void Configure(Funq.Container container)
            {
                //register any dependencies your services use, e.g:
                //container.Register<ICacheClient>(new MemoryCacheClient());
            }
        }

        //Initialize your application singleton
        protected void Application_Start(object sender, EventArgs e)
        {
            new ForumAppHost().Init();
        }
    }
}