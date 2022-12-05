using Model;
using Repository.EF.Base;
using System.Collections.Generic;
using System.Linq;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace Repository.EF.Repository
{
    public class ViewSurveyRepository : EFBaseRepository<ViewSurvey>
    {
        public IEnumerable<ViewSurvey> Select(int index, int count)
        {
            var ViewSurveyList = from ViewSurvey in Context.ViewSurveys
                                 select ViewSurvey;

            return ViewSurveyList.OrderBy(A => A.QuestionPriority).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<ViewSurvey> GetViewSurveys(QuestionType type)
        {

            var questionList = from q in Context.ViewSurveys
                               where q.QuestionType == (int)type
                               select q;

            return questionList.ToArray();
        }
        public IEnumerable<ViewSurveyResult> GetViewSurveyResults(string userId, int teamId)
        {

            var questionList = from q in Context.ViewSurveyResults
                               where
                               q.UserId == userId &&
                               q.TeamId == teamId
                               select q;

            return questionList.ToArray();
        }
        public IEnumerable<ViewSurveyResult> GetViewSurveyResults(string userId)
        {

            var questionList = from q in Context.ViewSurveyResults
                               where
                               q.UserId == userId
                               select q;

            return questionList.ToArray();
        }
        public IEnumerable<ViewSurvey> GetViewSurveyByQuestion(int questionId)
        {
            return Context.ViewSurveys.Where(t => t.Id == questionId);
        }
    }
}
