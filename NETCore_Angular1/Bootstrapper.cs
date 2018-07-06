using Microsoft.AspNetCore.Hosting;
using Nancy;
using Nancy.Conventions;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using NETCore_Angular1.Models;

namespace NETCore_Angular1
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override IRootPathProvider RootPathProvider { get; }
        public Bootstrapper(IHostingEnvironment environment)
        {
            RootPathProvider = new AppRootPathProvider(environment);
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<JsonSerializer, CustomJsonSerializer>();
        }

        protected override void ConfigureConventions(NancyConventions conventions)
        {
            base.ConfigureConventions(conventions);

            conventions.StaticContentsConventions.AddDirectory("css");
            conventions.StaticContentsConventions.AddDirectory("js");
            conventions.StaticContentsConventions.AddDirectory("fonts");
        }

        public class AppRootPathProvider : IRootPathProvider
        {
            private readonly IHostingEnvironment _environment;

            public AppRootPathProvider(IHostingEnvironment environment)
            {
                _environment = environment;
            }
            public string GetRootPath()
            {
                return _environment.WebRootPath;
            }
        }
    }
}
