﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using BotOperate.Models.DatabaseContext;
using BotOperate.Services.Data;

namespace BotOperate
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var config = GlobalConfiguration.Configuration;

            ConfigureServices(config);
            Configure(config);

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        private static void ConfigureServices(HttpConfiguration config)
        {
            Conversation.UpdateContainer(
                builder =>
                {
                    // Register the Bot Builder module
                    builder.RegisterModule(new DialogModule());
                    // Register the alarm dependencies
                    builder.RegisterModule(new TeamsTalentMgmtAppModule());

                    // Bot Storage: Here we register the state storage for your bot. 
                    // Default store: volatile in-memory store - Only for prototyping!
                    // In production you need to use adapters for Azure Table, CosmosDb, SQL Azure, or you can implement your own!
                    // For samples and documentation, see: https://github.com/Microsoft/BotBuilder-Azure
                    var store = new InMemoryDataStore();

                    // Other storage options
                    // var store = new TableBotDataStore("...DataStorageConnectionString..."); // requires Microsoft.BotBuilder.Azure Nuget package 
                    // var store = new DocumentDbBotDataStore("cosmos db uri", "cosmos db key"); // requires Microsoft.BotBuilder.Azure Nuget package 

                    builder.Register(c => store)
                        .Keyed<IBotDataStore<BotData>>(typeof(ConnectorStore))
                        .AsSelf()
                        .SingleInstance();

                    // Register your Web API controllers.
                    builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
                    builder.RegisterWebApiFilterProvider(config);
                });

            // Set the dependency resolver to be Autofac.
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Conversation.Container);
        }

        private static void Configure(HttpConfiguration config)
        {
            // Json settings
            var jsonSerializerSettings = config.Formatters.JsonFormatter.SerializerSettings;
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            jsonSerializerSettings.Converters.Add(new StringEnumConverter
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            });
            jsonSerializerSettings.Formatting = Formatting.Indented;
            JsonConvert.DefaultSettings = () => jsonSerializerSettings;
            
            InitializeDatabase();
        }

        private static void InitializeDatabase()
        {
            var db = Conversation.Container.Resolve<DatabaseContext>();

            var dataSeedPath = HostingEnvironment.MapPath("~\\App_Data\\") ??
                               throw new DirectoryNotFoundException();

            var recruiters =
                JsonConvert.DeserializeObject<List<Recruiter>>(
                    File.ReadAllText(Path.Combine(dataSeedPath, "recruiters.json")));
            
            var candidates =
                JsonConvert.DeserializeObject<List<Candidate>>(
                    File.ReadAllText(Path.Combine(dataSeedPath, "candidates.json")));
            
            var positions =
                JsonConvert.DeserializeObject<List<Ticket>>(
                    File.ReadAllText(Path.Combine(dataSeedPath, "tickets.json")));
            
            var locations =
                JsonConvert.DeserializeObject<List<Location>>(
                    File.ReadAllText(Path.Combine(dataSeedPath, "locations.json")));
            
            var interviews =
                JsonConvert.DeserializeObject<List<Interview>>(
                    File.ReadAllText(Path.Combine(dataSeedPath, "interviews.json")));

            db.Database.EnsureDeleted();

            db.Recruiters.AddRange(recruiters);
            db.Candidates.AddRange(candidates);
            db.Tickets.AddRange(positions);
            db.Locations.AddRange(locations);
            db.Interviews.AddRange(interviews);

            db.SaveChanges();
        }
    }
}