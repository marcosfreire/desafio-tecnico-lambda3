using AutoMapper;
using DesafioTecnico.Lambda3.Api.Models;
using DesafioTecnico.Lamda3.Domain;
using DesafioTecnico.Lamda3.Repository;
using DesafioTecnico.Lamda3.Repository.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace DesafioTecnico.Lambda3.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
               .AddCustomMvc(Configuration)
               .AddSwagger()
               .AddAutoMapper()
               .AddCustomFluentValidations()
               .AddCustomDbContext(Configuration)
               .ConfigurarDependencyInjection();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Desafio Tecnico Lambda3 v1");
            });

            app.UseCors();
        }
    }

    public static class StartupExtensions
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc()
            .AddFluentValidation()
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddControllersAsServices();

            services.AddCors(options =>
            {
                options.AddPolicy("*",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDataContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });
            });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Desafio Tecnico Lambda3",
                    Version = "v1",
                    Description = "API - Desafio Tecnico Lambda3"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });

            return services;
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProfessorModel, Professor>()
                    .ConstructUsing(a => new Professor(a.Id, a.Nome, a.Sobrenome, a.DataNascimento));

                cfg.CreateMap<Professor, ProfessorModel>();

                cfg.CreateMap<DisciplinaModel, Disciplina>()
                     .ConstructUsing(a => new Disciplina(a.Id, a.Nome));

                cfg.CreateMap<Disciplina, DisciplinaModel>();

            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

        public static IServiceCollection AddCustomFluentValidations(this IServiceCollection services)
        {
            services.AddTransient<ValidadorProfessor>();
            services.AddTransient<ValidadorDisciplina>();

            return services;
        }

        public static IServiceCollection ConfigurarDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IProfessorRepository, ProfessorRepository>();
            services.AddScoped<IDisciplinaRepository, DisciplinaRepository>();

            return services;
        }

    }
}
