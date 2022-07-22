using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.S3;
using Project5.Service;

namespace Project5;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container
    public void ConfigureServices(IServiceCollection services)
    {

        var credentials = new BasicAWSCredentials(Configuration["AWS:accessKeyID_DB"], Configuration["AWS:secretAccessKeyID_DB"]);
        var client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);

        services.AddRazorPages();
        services.AddControllers();
        services.AddSingleton<IAmazonDynamoDB>(client);
        services.AddTransient<IDynamoDBContext, DynamoDBContext>();
        services.AddSingleton<IProductionService, Project5.Service.Productionervice>();
        //var s3Config = new AmazonS3Config() { RegionEndpoint = RegionEndpoint.USEast1 };
        //var cli = new AmazonS3Client(
        //    Configuration["AWS:accessKeyID_S3"],
        //    Configuration["AWS:secretAccessKeyID_S3"],
        //    s3Config);

        //services.AddAWSService<IAmazonS3>();
        //services.AddSingleton<IAmazonS3>(cli);

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });
    }
}