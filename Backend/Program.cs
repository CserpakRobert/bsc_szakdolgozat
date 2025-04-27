using Backend;

internal class Program
{
    private static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args).
            ConfigureWebHostDefaults(webBuilder => { 
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://*:5103");
            }).Build().Run();
    }
}