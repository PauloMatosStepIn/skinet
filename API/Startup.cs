using API.Extensions;
using API.Helpers;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace API
{

  public class Startup
  {
    private readonly IConfiguration _config;
    public Startup(IConfiguration config)
    {
      _config = config;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // implemenbted in ApplicationServicesExtensions
      // services.AddScoped<IProductRepository, ProductRepository>();
      // services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

      services.AddAutoMapper(typeof(MappingProfiles));
      services.AddControllers();

      services.AddDbContext<StoreContext>(x => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

      //must be after controllers:
      // implemenbted in ApplicationServicesExtensions
      services.AddApplicationServices();
      // services.Configure<ApiBehaviorOptions>(options =>
      // {
      //   options.InvalidModelStateResponseFactory = actionContext =>
      //   {
      //     var errors = actionContext.ModelState
      //     .Where(e => e.Value.Errors.Count > 0)
      //     .SelectMany(x => x.Value.Errors)
      //     .Select(x => x.ErrorMessage).ToArray();

      //     var errorResponse = new ApiValidationErrorResponse
      //     {
      //       Errors = errors
      //     };

      //     return new BadRequestObjectResult(errorResponse);
      //   };
      // });

      // implemenbted in SwaggerServiceExtensions
      services.AddSwaggerDocumentation();
      // services.AddSwaggerGen(c =>
      // {
      //   c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIv5", Version = "v1" });
      // });

      //Allow cross origin from javascript

      services.AddCors(opt =>
      {
        opt.AddPolicy("CorsPolicy", policy =>
        {
          policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
        });
      });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ExceptionMiddleware>();

      //available for dev and prod
      // implemenbted in SwaggerServiceExtensions
      app.UseSwaggerDocumentation();
      // app.UseSwagger();
      // app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));

      // if (env.IsDevelopment())
      // {
      //   // app.UseDeveloperExceptionPage();
      //   app.UseSwagger();
      //   app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
      // }

      app.UseStatusCodePagesWithReExecute("/errors/{0}");
      app.UseHttpsRedirection();
      app.UseRouting();

      //serve static content (from wwwwroot)
      app.UseStaticFiles();

      //cors must be before Authorization
      app.UseCors("CorsPolicy");

      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
