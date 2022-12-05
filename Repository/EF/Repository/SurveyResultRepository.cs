using Model;
using Model.ViewModels.Survey;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repository.EF.Repository
{
    public class SurveyResultRepository : EFBaseRepository<SurveyResult>
    {
        public void CreateSurveyResult(SurveyResult surveyResult)
        {
            Add(surveyResult);
        }
        public void CreateSurveyResult(VmSurveyResult surveyResult)
        {
            var surveyResultDetails = new List<SurveyResultDetail>();

            foreach (var item in surveyResult.SurveyResultDetailList)
            {
                surveyResultDetails.Add(new SurveyResultDetail
                {
                    QuestionAnswerId = item.QuestionAnswerId,
                    SurveyResultId = item.SurveyResultId,
                    Value = item.Value
                });
            }

            Add(new SurveyResult
            {
                TeamId = surveyResult.TeamId.Value,
                UserId = surveyResult.UserId,
                QuestionId = surveyResult.QuestionId,
                Description = surveyResult.Description,
                SurveyResultDetails = surveyResultDetails
            });

        }
         public void CreateSurveyResult(IEnumerable<SurveyResult> surveyResultList)
        {

            foreach (var surveyResult in surveyResultList)
            {
                Add(surveyResult);
            }

        }
        public bool DeleteSurveyResult(string userId, int[] questionIds)
        {
            var oldSurveyResultList = Context.SurveyResults.Where(s => s.UserId == userId && questionIds.Contains(s.QuestionId));
            foreach (var surveyResult in oldSurveyResultList)
            {
                Delete(surveyResult);
            }

            return true;
        }
    }
}
