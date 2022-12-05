using BLL;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace WERC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        Telegram.Bot.TelegramBotClient Bot = new TelegramBotClient("863147267:AAHSO8VhLvHyEDXgo0qtwCqs5e8sRhdTIew");
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.All;
             
            //ModelBinders.Binders.Add(typeof(VmParticipantRule), new ParticipantRuleModelBinder());


            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();

        }
        private void Bot_OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            return;
            var message = messageEventArgs.Message;

            var count = Bot.GetChatMembersCountAsync("CyberneticCodeCom");

            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text.Contains("/start"))
            {
                string Str = "Start Recived...";
                Bot.SendTextMessageAsync(message.Chat.Id, Str);

            }
            else if (message.Text.Contains("/Stop"))
            {
                string Str = "Stop Recived...";
                Bot.SendTextMessageAsync(message.Chat.Id, Str);
            }
            else
            {
                Bot.SendTextMessageAsync(message.Chat.Id, message.Chat.Id.ToString());

            }

        }
    }
}
