
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Serilog;

var configuration=new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

//Log.Logger = new LoggerConfiguration() 
    //.ReadFrom.Configuration(configuration)
    //.CreateLogger();

try
{
    Log.Information("Application starting up");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, config) =>
    {
        config
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Elasticsearch("http://localhost:9200",
        indexFormat:"usage-{0:yyyy.MM.dd}",
        inlineFields: true);
        
    });
    
    //_ = builder.Host.UseSerilog((hostContext, loggerConfiguration) =>
    //        _ = loggerConfiguration.ReadFrom.Configuration(builder.Configuration));

    // Add services to the container.
    builder.Services.AddRazorPages();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    

    app.UseHttpLogging();
    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly.");
}
finally
{
    Log.CloseAndFlush();
}
