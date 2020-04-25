using System;
using HotChocolate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQLApi.Schema;
using HotChocolate.AspNetCore;
using GraphQLApi.DataAccess;
using GraphQLApi.DataAccess.Kafka;

namespace GraphQLApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQL(
                SchemaBuilder.New()
                    .AddQueryType<GetTodosQuery>()
                    .AddMutationType<TodoMutations>());
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddSingleton<ITodoByIdQuery, KafkaTodoRepository>();
            services.AddSingleton<IAllTodosQuery, KafkaTodoRepository>();
            services.AddSingleton<KafkaProducerFactory>();
            services.AddSingleton<KafkaTodoProducer>();
            services.AddSingleton<TodosStream>();
            services.AddTransient<Func<string, IAddTodoCommand>>(container => text => new KafkaAddTodoCommand(container.GetService<KafkaTodoProducer>(), text));
            services.AddTransient<Func<Guid, IToggleTodoCompletedCommand>>(container => id => new KafkaToggleTodoCompletedCommand(container.GetService<KafkaTodoProducer>(), container.GetService<ITodoByIdQuery>(), id));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TodosStream todosStream)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("MyPolicy");
            app.UseGraphQL();
            app.UsePlayground();

            // Start Kafka streams
            todosStream.Start();
        }
    }
}
