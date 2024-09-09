using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Articles.Infrastructure.Swagger
{
    public static class SwaggerConfiguration
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please Insert Jwt with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT"
                });

                x.SupportNonNullableReferenceTypes();

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer"}
                        },
                        Array.Empty<string>()
                    }
                });

                x.SwaggerDoc("v1", new OpenApiInfo() { Title = "Article Publisher Api", Version = "V1" });
                x.CustomSchemaIds(y => y.FullName);
                x.DocInclusionPredicate((version, ApiDescription) => true);
                x.TagActionsBy(y => new List<string>()
                {
                    y.GroupName ?? "Default"
                });
            });
        }
    }
}
