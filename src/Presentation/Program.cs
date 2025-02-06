using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args).ConfigureApplicationBuilder();

var app = builder.Build().ConfigureApplication();

try
{
    Console.WriteLine("Starting web host");
    app.Run();
    return 0;
}
catch (Exception ex)
{
    Console.WriteLine("Host terminated unexpectedly");
    Console.WriteLine(ex);
    return 1;
}
