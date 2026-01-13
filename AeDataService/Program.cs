// See https://aka.ms/new-console-template for more information
using AeDataService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder()
    .UseWindowsService()
    .ConfigureServices((t, tt) =>
    {
        tt.AddHostedService<MqttService>();
    })
    .Build().Run();
