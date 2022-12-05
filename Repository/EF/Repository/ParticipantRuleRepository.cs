using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using Model.ViewModels.ParticipantRule;

namespace Repository.EF.Repository
{
    public class ParticipantRuleRepository : EFBaseRepository<ParticipantRule>
    {
        public IEnumerable<ParticipantRule> Select()
        {
            var ParticipantRuleList = from ParticipantRule in Context.ParticipantRules
                                      select ParticipantRule;

            return ParticipantRuleList.ToArray();
        }
        public void CreateParticipantRule(ParticipantRule newParticipantRule)
        {
            Add(newParticipantRule);
        }
        public void UpdateParticipantRule(ParticipantRule updateableParticipantRule)
        {
            var oldParticipantRule = (from s in Context.ParticipantRules where s.Id == updateableParticipantRule.Id select s).FirstOrDefault();

            oldParticipantRule.FirstTeamMaxMember = updateableParticipantRule.FirstTeamMaxMember;
            oldParticipantRule.EachExtraTeamMaxMember = updateableParticipantRule.EachExtraTeamMaxMember;
            oldParticipantRule.ExtraParticipantFee = updateableParticipantRule.ExtraParticipantFee;

            Update(oldParticipantRule);
        }
        public bool DeleteParticipantRule(int participantRuleId)
        {
            var oldParticipantRule = (from s in Context.ParticipantRules where s.Id == participantRuleId select s).FirstOrDefault();

            Delete(oldParticipantRule);

            return true;
        }

        public ParticipantRule GetParticipantRule(int paymentTypeId)
        {
            return Context.ParticipantRules.First(p=>p.PaymentTypeId == paymentTypeId);
        }
        public IEnumerable<VmParticipantRule> GetParticipantRuleList()
        {
            return (from participantRule in Context.ParticipantRules
                    join paymentType in Context.PaymentTypes
                     on participantRule.PaymentTypeId equals paymentType.Id
                    select new VmParticipantRule
                    {
                        Id = participantRule.Id,
                        FirstTeamMaxMember = participantRule.FirstTeamMaxMember,
                        EachExtraTeamMaxMember = participantRule.EachExtraTeamMaxMember,
                        ExtraParticipantFee = participantRule.ExtraParticipantFee,
                        UIExtraParticipantFee = participantRule.ExtraParticipantFee.ToString(),
                        PaymentTypeId = participantRule.PaymentTypeId,
                        PaymentType = paymentType.Name,
                        PaymentTypeDescription = paymentType.Description,
                    }).ToArray();
        }
    }
}
