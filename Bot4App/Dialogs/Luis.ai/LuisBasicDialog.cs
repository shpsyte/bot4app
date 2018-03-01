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
    public class LuisBasicDialog : LuisDialog<object>
    {

        private readonly static string _LuisModelId = "a2ca67c6-9f1c-43ee-90e1-c9c297d5f330"; //ConfigurationManager.AppSettings["QnaSubscriptionKey"]
        private readonly static string _LuiSubscriptionKey = "a8046f7776b7494db8f1ea873eac5c3e"; //ConfigurationManager.AppSettings["LuisId"]
        private readonly static string _DefatulMsg = "Hum... Minha conciência não entende isso ainda, " +
            "mas com certeza aprenderei mais sobre isso...  ";

        private readonly static string _msg = $"Estou aprendendo muitas coisas, mas veja o que já posso fazer:\n" +
                             "* **Pergunte sobre mim**, tipo: *O que você é?*, ou algo assim\n" +
                             "* **Solicitar uma proposta**, tipo: *Pode enviar uma proposta?*, ou algo assim\n" +
                             "* **Traduzir textos**, tipo: *Traduz pra mim ?*, ou algo assim\n" +
                             "* **Contar piadas**, tipo: *Me conte uma piada?*, ou algo assim\n";


        public LuisBasicDialog() : base(new LuisService(new LuisModelAttribute(_LuisModelId, _LuiSubscriptionKey, LuisApiVersion.V2)))
        {
             
        }
 


        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"{_DefatulMsg}\n{_msg}");

            //var days = (IEnumerable<Days>)Enum.GetValues(typeof(Days));
            //PromptDialog.Choice(context, StoreHoursResult, days, "Which day of the week?",
            //    descriptions: from day in days
            //                  select (day == Days.Saturday || day == Days.Sunday) ? day.ToString() + "(no holidays)" : day.ToString());

            //context.Done<string>(null);
        }


        
        [LuisIntent("sense-bot")]
        [LuisIntent("greeting-bot")]
        public async Task Conciencia(IDialogContext context, LuisResult result)
        {
            
            var userQuestion = (context.Activity as Activity).Text;
            await context.Forward(new QnaAboutMe(), ResumeAfterQnA, context.Activity, CancellationToken.None);
        }

        

        [LuisIntent("laugh-bot")]
        public async Task Laugh(IDialogContext context, LuisResult result) => await context.PostAsync($"{ FakeList.GetRandomLaugh()}  { FakeList.GetListRandomEmojiHappy(3) }");

        [LuisIntent("hate-bot")]
        public async Task Hat(IDialogContext context, LuisResult result) => await context.PostAsync($"{ FakeList.GetRandomHatPhrase()} { FakeList.GetListRandomEmojiAngry(6) }  ");


        [LuisIntent("joke-bot")]
        public async Task Joke(IDialogContext context, LuisResult result) => await context.PostAsync($"{ FakeList.GetRandomJoke()} { FakeList.GetListRandomEmojiHappy(6) } ");

        [LuisIntent("help-bot")]
        public async Task Help(IDialogContext context, LuisResult result)
        {
           

            await context.PostAsync(_msg);
            context.Done<string>(null);
        }
         

        [LuisIntent("translate-bot")]
        public async Task Translate(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("**(▀̿Ĺ̯▀̿ ̿)** - Ok, me fala o texto então...");
            context.Wait(TraduzirPtBr);
        }


        [LuisIntent("request-quote")]
        public async Task ForwardRequestQuote(IDialogContext context, LuisResult result ) => await context.Forward(new RequestQuoteDialog(), ResumeAfterQnA, context.Activity, CancellationToken.None);

             
         

        private async Task TraduzirPtBr(IDialogContext context, IAwaitable<IMessageActivity> value)
        {
             
               var message = await value;
               var text = message.Text;
             
            var response = await new Translate().TranslateText(text);

            await context.PostAsync(response);
            context.Wait(MessageReceived);
        }

        private async Task ResumeAfterQnA(IDialogContext context, IAwaitable<object> result)
        {
            //var activity = (context as Activity);
            //ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            //Activity reply = activity.CreateReply();
            //reply.Type = ActivityTypes.Typing;
            //reply.Text = null;
            //await connector.Conversations.ReplyToActivityAsync(reply);

            //context.Done<object>(null);
        }

        private async Task ResumeAfterTranslate(IDialogContext context, IAwaitable<object> result)
        {

            await context.PostAsync("**(▀̿Ĺ̯▀̿ ̿)** - Ok, me fala o texto então...");
        }

        







    }
}