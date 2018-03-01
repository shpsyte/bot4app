using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bot4App.Dialogs.Luis.ai;
using Bot4App.Forms;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;

namespace Bot4App
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var _msg = $"Estou aprendendo muitas coisas, mas veja o que já posso fazer:\n" +
                              "* **Pergunte sobre mim**, tipo: *O que você é?*, ou algo assim\n" +
                              "* **Solicitar uma proposta**, tipo: *Pode enviar uma proposta?*, ou algo assim\n" +
                              "* **Traduzir textos**, tipo: *Traduz pra mim ?*, ou algo assim\n" +
                              "* **Contar piadas**, tipo: *Me conte uma piada?*, ou algo assim\n";

            if (activity.Type == ActivityTypes.Message)
            {
                await SendBotIsTyping(activity);

                await Conversation.SendAsync(activity, () => new LuisBasicDialog());
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                if (activity.MembersAdded != null && activity.MembersAdded.Any())
                {
                    foreach (var member in activity.MembersAdded)
                    {
                        if (member.Id != activity.Recipient.Id)
                        {
                            //await this.SendConversation(activity);
                            var reply = activity.CreateReply();
                            reply.Text = $"**( ͡° ͜ʖ ͡°)**  Oi sou o Bot, estou aprendendo muitas coisas.. " +
                                $"quero lhe ajudar a me entender... vamos la ? ";
                            await connector.Conversations.ReplyToActivityAsync(reply);

                            await SendBotIsTyping(activity);
                            

                            await Task.Delay(2000); // 4 second delay
                            reply.Text = _msg;
                            await connector.Conversations.ReplyToActivityAsync(reply);

                            //await SendBotIsTyping(message, client);

                            //await Task.Delay(2000); // 4 second delay
                            //reply.Text = $"Vamos lá ? Sempre que precisar digite ajuda...";
                            //await client.Conversations.ReplyToActivityAsync(reply);
                        };
                    }

                }
            }
            else
            {
                await HandleSystemMessageAsync(activity);
                

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static async Task SendBotIsTyping(Activity activity)
        {
            Activity reply = activity.CreateReply();
            reply.Type = ActivityTypes.Typing;
            reply.Text = null;

            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            await connector.Conversations.ReplyToActivityAsync(reply);
        }

        //internal static IDialog<CaptureLead> MakeRoot()
        //{
        //    return Chain.From(() => new LuisBasicDialog(BuildForm));
        //}


        private async Task SendConversation(Activity activity)
        {
            //await Conversation.SendAsync(activity, () => new Dialogs.BootLuisDialog());
            await Conversation.SendAsync(activity, () => Chain.From(() => FormDialog.FromForm(() => Forms.CaptureLead.BuildForm(), FormOptions.PromptFieldsWithValues)));
        }

        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle add
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                //var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());

                //ITypingActivity update = message;
                //var reply = message.CreateReply();
                //reply.Text = $"Estou aguardando você digitar...";
                //await client.Conversations.ReplyToActivityAsync(reply);
               // return (Activity)reply;
                 
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                
               
            }

            return null;
        }
    }
}