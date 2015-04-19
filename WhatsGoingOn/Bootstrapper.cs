namespace WhatsGoingOn
{
    using BetterConfig;
    using Nancy;
    using Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            Configuration.ValueStrategies.Add(LocalDebugging.Strategy());

            var config = Configuration.For<IConfiguration>();
            container.Register(config);
        }
    }
}