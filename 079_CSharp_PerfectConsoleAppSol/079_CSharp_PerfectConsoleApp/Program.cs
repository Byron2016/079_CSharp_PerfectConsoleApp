using _079_CSharp_PerfectConsoleApp;
using HelloWorldLibrary.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = CreateHostBuilder(args).Build();

// Con este using acá para decirle a la aplicación qu cuando nosotros
// cerrermos la aplicación, estar seguros del dispose adecuado de esta variable host.
// Va a crear un scope para DI el cual es basicamente la aplicación entera
// eso determina por nosotros tiempo de vida y cosas como esas.
using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    services.GetRequiredService<App>().Run(args);
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}


static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, serices) =>
        {
            serices.AddSingleton<IMessages, Messages>();
            serices.AddSingleton<App>();

        });
}
