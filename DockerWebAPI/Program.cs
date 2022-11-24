using System.Net;
using Amazon;
using Amazon.S3;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using HealthChecks.Aws.S3;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://localhost:5341")
    .Enrich.FromLogContext().Enrich.WithProperty("appName", "sampleapp")
    .CreateLogger();

builder.Logging.AddSerilog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecksUI(x =>
{
    x.AddHealthCheckEndpoint("default api", "/healthz"); //map health check api
}).AddInMemoryStorage();
builder.Services.AddHealthChecks()
    // .AddMySql(
    //     "Server=asdasd-cluster.cluster-cylctavxxdxn.eu-central-1.rds.amazonaws.com;Port=3306;Uid=admin_master;Pwd=ssdasdasd;")
    // .AddS3(options =>
    // {
    //     options.BucketName = "sdsdgsd-files";
    //     options.S3Config = new AmazonS3Config
    //     {
    //         RegionEndpoint = RegionEndpoint.EUCentral1,
    //     };
    //     options.AccessKey = "sdgdfgdfg";
    //     options.SecretKey = "dfgdfhfdhd";
    // })
    ;

var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHealthChecksPrometheusExporter("/my-health-metrics",
        options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);
    app.UseAuthorization();

    app.MapControllers();
    app.UseRouting().UseEndpoints(x =>
    {
        x.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        x.MapHealthChecksUI();
    });
    app.Run();