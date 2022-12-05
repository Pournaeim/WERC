using Model.ViewModels.ParticipantRule;
using System.Web.Mvc;

namespace WERC.Models.CustomModelBinding
{
    public class ParticipantRuleModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            string id = GetValue(bindingContext, "Id");
            string extraParticipantFee = GetValue(bindingContext, "ExtraParticipantFee") ?? "200";
            string firstTeamMaxMember = GetValue(bindingContext, "FirstTeamMaxMember");
            string eachExtraTeamMaxMember = GetValue(bindingContext, "EachExtraTeamMaxMember");


            var vmParticipantRule = new VmParticipantRule()
            {
                Id = int.Parse(id),
                ExtraParticipantFee = decimal.Parse(extraParticipantFee),
                FirstTeamMaxMember = int.Parse(firstTeamMaxMember),
                EachExtraTeamMaxMember = int.Parse(eachExtraTeamMaxMember)
            };

            return vmParticipantRule;
        }

        private string GetValue(ModelBindingContext bindingContext, string key)
        {
            var result = bindingContext.ValueProvider.GetValue(key);
            return result?.AttemptedValue;
        }
    }
}
