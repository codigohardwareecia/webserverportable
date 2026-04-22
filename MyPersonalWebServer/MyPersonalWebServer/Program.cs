using MyPersonalWebServer;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        var app = builder.Build();

        builder.WebHost.UseUrls($"http://0.0.0.0:5000");

        app.UseDefaultFiles();

        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        Task.Run(() => app.Run());

        ApplicationConfiguration.Initialize();

        Application.Run(new frmMain());
    }
}

