using FluentMigrator.Runner;
using FluentMigratorExample.Migrations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FluentMigratorExample
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "FluentMigratorExample", Version = "v1" });
			});
			services
				.AddLogging(c => c.AddFluentMigratorConsole())
				.AddFluentMigratorCore()
				.ConfigureRunner(c => c
				.AddSqlServer2012()
				.WithGlobalConnectionString("Persist Security Info = False; Integrated Security = true; Initial Catalog = Demo; server = .\\SQLEXPRESS")
					.ScanIn(Assembly.GetExecutingAssembly()).For.All());
		}



		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FluentMigratorExample v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			Database.EnsureDatabase("Persist Security Info = False; Integrated Security = true; Initial Catalog = master; server = .\\SQLEXPRESS",
	   "Demo");

			app.Migrate();
		}
	}
}
