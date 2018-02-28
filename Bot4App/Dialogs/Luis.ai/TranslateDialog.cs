using Bot4App.Forms;
using Bot4App.Models;
using Bot4App.QnA;
using Bot4App.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static Bot4App.Models.Domain;

namespace Bot4App.Dialogs.Luis.ai
{
    [Serializable]
    public class TranslateDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(TraduzirPtBr);

            return Task.CompletedTask;
        }

        private async Task TraduzirPtBr(IDialogContext context, IAwaitable<IMessageActivity> value)
        {

            //await context.PostAsync("**(▀̿Ĺ̯▀̿ ̿)** - Ok, me fala o texto então...");
            var activity = await value as Activity;



            var message = activity;
           


            var response = await new Translate().TranslateText(message.Text);

            await context.PostAsync(response);
            context.Wait(TraduzirPtBr);
        }


    }
}