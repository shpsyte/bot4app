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

            if (activity.Type == ActivityTypes.Message)
            {
                await SendBotIsTyping(activity, connector);

                await Conversation.SendAsync(activity, () => new LuisBasicDialog());
            }
            else
            {
                await HandleSystemMessageAsync(activity);
                

            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        public static async Task SendBotIsTyping(Activity activity, ConnectorClient connector)
        {
            Activity reply = activity.CreateReply();
            reply.Type = ActivityTypes.Typing;
            reply.Text = null;
            await connector.Conversations.ReplyToActivityAsync(reply);
        }

        //internal static IDialog<CaptureLead> MakeRoot()
        //{
        //    return Chain.From(() => new LuisBasicDialog(BuildForm));
        //}



        private async Task<Activity> HandleSystemMessageAsync(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                IConversationUpdateActivity update = message;
                var client = new ConnectorClient(new Uri(message.ServiceUrl));
                if (update.MembersAdded != null && update.MembersAdded.Any())
                {
                    foreach (var newMember in update.MembersAdded)
                    {
                        if (newMember.Id != message.Recipient.Id)
                        {
                            var reply = message.CreateReply();
                            reply.Text = $"**(▀̿Ĺ̯▀̿ ̿)** { "Oi sou o Bot, estou aprendendo muitas coisas.. quero ajudar você a me contratar.."}";
                            await client.Conversations.ReplyToActivityAsync(reply);

                            await SendBotIsTyping(message, client);

                            await Task.Delay(2000); // 4 second delay
                            reply.Text = $"Então o que estou aprendendo é responder questões de **Bot**, **Solicitar Orçamentos de Bot**, **Traduzir Texto**, e até **Contar uma piada**";
                            await client.Conversations.ReplyToActivityAsync(reply);

                            await SendBotIsTyping(message, client);

                            await Task.Delay(2000); // 4 second delay
                            reply.Text = $"Vamos lá ? Sempre que precisar digite ajuda...";
                            await client.Conversations.ReplyToActivityAsync(reply);

                        }
                    }
                }
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                var client = new ConnectorClient(new Uri(message.ServiceUrl), new MicrosoftAppCredentials());

                ITypingActivity update = message;
                var reply = message.CreateReply();
                reply.Text = $"Estou aguardando você digitar...";
                await client.Conversations.ReplyToActivityAsync(reply);
               // return (Activity)reply;
                 
            }
            else if (message.Type == ActivityTypes.Ping)
            {
                
               
            }

            return null;
        }
    }
}