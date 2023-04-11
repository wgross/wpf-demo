using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace DemoApp.WebService;

public record Data(string Text);

internal class HostedWebApp : IHostedService
{
    private WebApplication hostedWebApp;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var builder = WebApplication.CreateBuilder();

        // Add services to the container.
        this.hostedWebApp = builder.Build();
        this.hostedWebApp.Urls.Add("http://localhost:5000");

        // Configure the HTTP request pipeline.
        hostedWebApp.MapGet("/data", () => this.ReturnData());

        // run the app
        await hostedWebApp.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken) => await this.hostedWebApp.StopAsync(cancellationToken);

    private Data ReturnData() => new Data("text");
}