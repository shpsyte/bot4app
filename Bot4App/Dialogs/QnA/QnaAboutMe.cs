using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Bot4App.QnA
{
    [Serializable]
    public class QnaAboutMe : QnAMakerDialog
    {
        private readonly static string _QnaKnowledgedId = "b80c3d4a-e80c-436f-800c-16e9cc73f402"; //ConfigurationManager.AppSettings["QnaSubscriptionKey"]
        private readonly static string _QnaSubscriptionKey = "ac1a50f16e0b400f93e81d87eea081fc"; //ConfigurationManager.AppSettings["QnaKnowledgedId"]
        private readonly static string _DefatulMsg = "Hum... Minha conciência não entende isso ainda, mas com certeza aprenderei mais sobre isso...";
        private readonly static double _Score = 0.3;
        private readonly static int _QtyAnswerReturn = 3;


        public QnaAboutMe() : base(new QnAMakerService(new QnAMakerAttribute(_QnaSubscriptionKey, _QnaKnowledgedId, _DefatulMsg, _Score, _QtyAnswerReturn)))
        {

        }




        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults result)
        {

            //await base.RespondFromQnAMakerResultAsync(context, message, result);
            if (result.Answers.Count > 0)
            {
                var response = result.Answers.First().Answer;
                await context.PostAsync(response);
            }else
            {
                var response = "EU sei que você queria saber algo sobre mim, " +
                    "mas não sei lhe responder isso ainda...";
                await context.PostAsync(response);

            }

        }


        ////// Override to log matched Q&A before ending the dialog
        protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            await base.DefaultWaitNextMessageAsync(context, message, results);
            // await context.PostAsync("It taht");
            
        }



   
        //// Qunado o ML está ativa este metodo pergunta: "Você quis dizer isso ?" para que o qna possa aprender.
        protected override async Task QnAFeedbackStepAsync(IDialogContext context, QnAMakerResults qnaMakerResults)
        {
            // responding with the top answer when score is above some threshold
            if (qnaMakerResults.Answers.Count > 0 && qnaMakerResults.Answers.FirstOrDefault().Score > 0.75)
            {
                await context.PostAsync(qnaMakerResults.Answers.FirstOrDefault().Answer);
            }
            else
            {
                await base.QnAFeedbackStepAsync(context, qnaMakerResults);
            }
        }

    }

    
}