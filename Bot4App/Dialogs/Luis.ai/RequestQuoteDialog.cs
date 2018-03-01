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
    public class RequestQuoteDialog : LuisDialog<object>
    {
        private readonly static string _LuisModelId = "a2ca67c6-9f1c-43ee-90e1-c9c297d5f330"; //ConfigurationManager.AppSettings["QnaSubscriptionKey"]
        private readonly static string _LuiSubscriptionKey = "a8046f7776b7494db8f1ea873eac5c3e"; //ConfigurationManager.AppSettings["LuisId"]
        private readonly static string _DefatulMsg = "Hum... Minha conciência não entende isso ainda, mas com certeza aprenderei mais sobre isso...";

        public RequestQuoteDialog() : base(new LuisService(new LuisModelAttribute(_LuisModelId, _LuiSubscriptionKey, LuisApiVersion.V2)))
        {

        }

        

        [LuisIntent("request-quote")]
        public async Task RequestQuoteForm(IDialogContext context, LuisResult result)
        {


            var activity = (context.Activity as Activity);
            var capLeadForm = new CaptureLead();
            var entities = new List<EntityRecommendation>(result.Entities);

            var form = new FormDialog<CaptureLead>(capLeadForm, CaptureLead.BuildForm, FormOptions.PromptInStart, entities);
            //context.Call<CaptureLead>(form, CaptureLeadComplete);

            await Conversation.SendAsync(activity, () => Chain.From(() => FormDialog.FromForm(() => CaptureLead.BuildForm(), FormOptions.PromptFieldsWithValues)));


        }


        private async Task CaptureLeadComplete(IDialogContext context, IAwaitable<CaptureLead> result)
        {
            var activity = (context.Activity as Activity);
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            CaptureLead order = null;
            try
            {
                order = await result;
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("You canceled the form!");
                return;
            }

            if (order != null)
            {

                await MessagesController.SendBotIsTyping(activity, connector);
                await context.PostAsync("Ok... enviado");

                await MessagesController.SendBotIsTyping(activity, connector);
                await context.PostAsync($"Se eu ajudar em outra coisa, é só me avisar... questões de **Bot**, **Solicitar Orçamentos de Bot**, **Traduzir Texto**, e até **Contar uma piada**");
            }
            else
            {
                await context.PostAsync("Form returned empty response!");
            }

            context.Wait(MessageReceived);
        }


    }
}