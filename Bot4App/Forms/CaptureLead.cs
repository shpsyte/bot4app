using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Bot4App.Forms
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Hum, esta opção não é valida \"{0}\".")]
    public class CaptureLead
    {
        [Describe("Tipo do Bot")]
        [Template(TemplateUsage.EnumSelectOne, "Qual tipo de {&} que gostaria de criar ? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
        public TipoBot TipoBot { get; set; }
        
        [Prompt("Qual o seu Nome ?")]
        [Template(TemplateUsage.NotUnderstood, "What does \"{0}\" mean???")]
        [Describe("Nome")]
        public string Name { get; set; }

        [Prompt("Qual o seu E-Mail ?")]
        [Describe("Email")]
        [Pattern(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")]
        public string Email { get; set; }

        [Prompt("Qual o seu Telefone ?")]
        [Describe("Fone")]
        [Pattern(@"(<Undefined control sequence>\d)?\s*\d{3}(-|\s*)\d{4}")]
        public string Fone { get; set; }

        [Describe("Breve observação")]
        [Optional]
        public string Describe { get; set; }

        [Optional]
//        [Prompt("Você tem API ? {||}")]
        [Template(TemplateUsage.EnumSelectOne, "Você tem api {&} que gostaria de integrar ? {||}", ChoiceStyle = ChoiceStyleOptions.PerLine)]
  //      [Template(TemplateUsage.CurrentChoice, "Nenhuma opção")]
        public Api api { get; set; }

        public static IForm<CaptureLead> BuildForm()
        {

            var form = new FormBuilder<CaptureLead>();
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Configuration.Yes = new string[] { "sim", "yes", "s", "y", "yeap", "ok" };
            form.Configuration.No = new string[] { "nao", "não", "no", "not", "n" };
            //form.Confirm(async (email) =>
            // {
            //     return new PromptAttribute($"Ok, vou lhe pedir alguns dados, ok ?");
            // });
            

            form.OnCompletion(async (context, pedido) =>
            {
                //Salvra n base de dadoo gerar pedido, integrar com servicos PTO
                await context.PostAsync("Pronto, ok, seus dados serão enviados..");
            });
            return form.Build();

        }


        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendFormat("Tipo de Bot.: {0}, ", TipoBot);
            builder.AppendFormat("Nome: {0}: ", Name);
            builder.AppendFormat("Email: {0} ", Email);
            builder.AppendFormat("Fone: {0} ", Fone);
            builder.AppendFormat("Descrição: {0} ", Describe);
            builder.AppendFormat("Tem API: {0} ", api);
             
            return builder.ToString();
        }

    } 
     
    [Describe("Tipo do Bot")]
    public enum TipoBot
    {
        [Describe("Outro")]
        [Terms("Outro", "O", "outro")]
        Outro = 1,
        [Describe("Atendimento")]
        [Terms("Atend", "A", "atendimento")]
        Atendimento,
        [Describe("Prospect")]
        [Terms("Cap", "C", "Captura", "lead", "Prospect" , "Captura de Bot")]
        CapLead,
        [Describe("FaQ")]
        [Terms("faq", "F", "faq", "Faq")]
        Faq
    }

    [Describe("Você possui API? ")]
    public enum Api
    {
        [Terms("Não sei", "nao sei")]
        [Describe("Não sei")]
        NaoSei = 1,
        [Terms("Sim", "sim", "s", "yeap")]
        [Describe("Sim")]
        Sim,
        [Terms("Nao", "Não", "n")]
        [Describe("Nao")]
        Nao
    }





}