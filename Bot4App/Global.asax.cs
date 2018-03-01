using System;
using Autofac;
using System.Web.Http;
using System.Configuration;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Azure;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;

namespace Bot4App
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var uri = new Uri("https://bot4app.documents.azure.com:443/");
            var key = "kLvaPgHRgizcQ9l07GGn6szrRZMxjkoacmNsVtarP2QIhy1U1KXqPzKQxGx0OY3cxW46h3InRSkMjNsRx5nyYQ==";
            var store = new DocumentDbBotDataStore(uri, key);

            GlobalConfiguration.Configure(WebApiConfig.Register);


            Conversation.UpdateContainer(
               builder =>
               {
                   builder.Register(c => store)
                       .Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                       .AsSelf()
                       .SingleInstance();

                   builder.Register(c => new CachingBotDataStore(store, CachingBotDataStoreConsistencyPolicy.ETagBasedConsistency))
                       .As<IBotDataStore<BotData>>()
                       .AsSelf()
                       .InstancePerLifetimeScope();

               });
        }
    }

}
