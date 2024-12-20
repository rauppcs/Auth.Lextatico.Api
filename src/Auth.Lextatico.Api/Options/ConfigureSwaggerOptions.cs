using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Auth.Lextatico.Api.Options;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, new OpenApiInfo
            {
                Title = $"Auth Lextatico Api - {description.ApiVersion}",
                Version = description.ApiVersion.ToString(),
                Contact = new OpenApiContact
                {
                    Name = "Cassiano dos Santos Raupp",
                    Email = "cassiano.raupp@outlook.com",
                    Url = new Uri("https://cassiano3795.github.io/cassianoraupp/")
                },
                Description = description.IsDeprecated
                    ? "Esta versão foi depreciada."
                    : "API ativa."
            });
        }
        
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "Entre com o Token JWT",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        });
    }
}