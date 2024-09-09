using Articles.Infrastructure;
using Articles.Infrastructure.CurrentUser;
using Articles.Infrastructure.Security;
using Articles.Infrastructure.Swagger;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Retrieve connection string and database provider from environment variables or use default values
        //var connectionString = builder.Configuration.GetValue<string>("ArticleReportConnection") ??
        //                       "ConnectionString"; // Replace with your default connection string
        //var databaseProvider = builder.Configuration.GetValue<string>("ArticleReportConnection") ??
        //                       "sqlserver"; // Default to SQL Server if no provider is specified
        //var databaseProvider = builder.Configuration.GetValue<string>("ArticleReportConnection");
        //if (string.IsNullOrWhiteSpace(databaseProvider))
        //{
        //    databaseProvider = ;
        //}

        //Add services to the container.
       builder.Services.AddDbContext<ReportDbContext>(
           options => options.UseSqlServer(builder.Configuration.GetConnectionString("ArticleReportConnection")));

        // Add services to the container and configure the database context based on the provider
        //builder.Services.AddDbContext<ReportDbContext>(options =>
        //{
        //    if (databaseProvider.ToLowerInvariant().Trim().Equals("sqlite", StringComparison.Ordinal))
        //    {
        //        options.UseSqlite(connectionString);
        //    }
        //    else if (databaseProvider.ToLowerInvariant().Trim().Equals("sqlserver", StringComparison.Ordinal))
        //    {
        //        options.UseSqlServer(connectionString);
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("Unknown database provider. Please check configuration.");
        //    }
        //});


        builder.Services.AddLocalization(x => x.ResourcesPath = "Resource");

        // Inject an implementation of ISwaggerProvider with defaulted settings applied
        //builder.Services.AddSwaggerGen(x =>
        //{
        //    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        //    {
        //        In = ParameterLocation.Header,
        //        Description = "Please insert JWT with Bearer into field",
        //        Name = "Authorization",
        //        Type = SecuritySchemeType.ApiKey,
        //        BearerFormat = "JWT"
        //    });

        //    x.SupportNonNullableReferenceTypes();

        //    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
        //                {
        //            {   new OpenApiSecurityScheme
        //            {
        //                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        //            },
        //            Array.Empty<string>()}
        //                });
        //    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Mass Report API", Version = "v1" });
        //    x.CustomSchemaIds(y => y.FullName);
        //    x.DocInclusionPredicate((version, apiDescription) => true);
        //    x.TagActionsBy(y => new List<string>()
        //                {
        //            y.GroupName ?? throw new InvalidOperationException()
        //                });
        //});

        //Cors
        builder.Services.AddCors();

        // Register IPasswordHasher and IJwtTokenGenerator
        builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
        builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        // MVC configuration with custom conventions, filters, and FluentValidation
        builder.Services.AddMvc(opt =>
        {
            opt.Conventions.Add(new GroupByApiRootConvention());  // Custom API root convention
            opt.Filters.Add(typeof(ValidatorActionFilter));        // Global action filter for validation
            opt.EnableEndpointRouting = false;                    // Disable endpoint routing
        })
        .AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;  // Ignore null values in JSON response
        });

        // Use the new FluentValidation extensions
        builder.Services.AddFluentValidationAutoValidation() // Enable FluentValidation's automatic validation
                        .AddFluentValidationClientsideAdapters(); // Enable client-side validation adapters


        // Add the updated FluentValidation service
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();


        builder.Services.AddControllers();

        // Add MediatR 
        builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DbContextTransactionPipelineBehavior<,>));

        // Register AutoMapper
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly()); // Register AutoMapper in the DI

        // Register ICurrentUserAccessor (assuming you have this class implemented)
        builder.Services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerConfiguration();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}