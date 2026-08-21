
using Hospital_Managment_System.DAL.Utiles;
using Hospital_Managment_System_.PL.Extensions;
using Hospital_Mangament_System.BLL.Mapping;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Hospital_Managment_System_.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter JWT token like: Bearer {token}"
                });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        Reference = new Microsoft.OpenApi.Models.OpenApiReference
                        {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            });
            //db
            builder.Services.AddDatabaseServices(builder.Configuration);
            //Identity
            builder.Services.AddIdentityServices();
            //Auth
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddLocalizationServices();
            builder.Services.AddAplicationServices(builder.Configuration);
            builder.Services.AddAuthorization();//jwt
            MapsterConfig.MapsterConfigRegister();
            var app = builder.Build();
            app.UseRequestLocalization(app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseCors(CorsPolicy.PolicyName);//Â‰« Ì „ ì ›⁄Ì·î CORS ›⁄·Ì« ⁄·Ï «·ÿ·»« 
            //                                   //  √Ì request ÌœŒ· «·”Ì—›— Ì „ ›Õ’Â Õ”» «·”Ì«”…
            app.UseHttpsRedirection();
            app.UseAuthentication();//jwt
            app.UseAuthorization();
            app.UseStaticFiles();//Â«Ì ⁄‘«‰ «·’Ê— „‰ «·(wwwroot)  ÊŒ– «·’Ê—…
            app.MapControllers();//Ì—»ÿ «·‹ endpoints »«·‹ Controllers


           

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var seeders = services.GetServices<ISeedData>();
                foreach (var seeder in seeders)
                {
                    await seeder.DataSeed();
                }
            }


            app.Run();
        }
    }
}
