using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(InfoVis.KnowledgeTrees.Startup))]

namespace InfoVis.KnowledgeTrees
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
