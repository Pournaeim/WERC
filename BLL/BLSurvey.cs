using BLL.Base;
using Model;
using Model.ViewModels.Survey;
using Model.ViewModels.Team;
using Repository.EF.Repository;
using System.Collections.Generic;
using System.Linq;
using static Model.ApplicationDomainModels.ConstantObjects;

namespace BLL
{
    public class BLSurvey : BLBase
    {
        public IEnumerable<VmSurvey> GetSurveyList(string userId, QuestionType type)
        {

            var blPerson = new BLPerson();
            var person = blPerson.GetPersonByUserId(userId);

            var blTeamMember = new BLTeamMember();

            VmTeamMember teamMember = null;

            var teamId = 0;

            if (person.RoleName.ToLower().Contains("judge") == false)
            {
                teamMember = blTeamMember.GetTeamMemberByUserId(userId);
                teamId = teamMember.TeamId;
                blTeamMember.UpdateTeamMemberSurvey(userId, true);
            }

            var viewViewSurveyRepository = UnitOfWork.GetRepository<ViewSurveyRepository>();

            var questionList = viewViewSurveyRepository.GetViewSurveys(type);

            var vmSurveyList = (from q in questionList
                                group new
                                {
                                    q.AnswerId,
                                    q.QuestionAnswerId,
                                    q.AnswerType,
                                    q.Weight,
                                    q.AnswerPriority,
                                    q.Answer,
                                    q.TitleVisible,
                                    q.Comment,
                                    q.ShowComment,
                                }
                                by new
                                {
                                    q.Id,
                                    q.Question,
                                    q.QuestionPriority,
                                    q.QuestionType,
                                    q.QuestionComment,
                                }
                               into g
                                select new VmSurvey
                                {
                                    Id = g.Key.Id,
                                    Question = g.Key.Question,
                                    QuestionPriority = g.Key.QuestionPriority,
                                    QuestionComment = g.Key.QuestionComment,
                                    QuestionType = g.Key.QuestionType,

                                    SurveyDetailList = (from gd in g.ToList()
                                                        select new VmSurveyDetail
                                                        {
                                                            AnswerId = gd.AnswerId,
                                                            QuestionAnswerId = gd.QuestionAnswerId,
                                                            AnswerType = gd.AnswerType,
                                                            Weight = gd.Weight,
                                                            AnswerPriority = gd.AnswerPriority,
                                                            Answer = gd.Answer,
                                                            TitleVisible = gd.TitleVisible,
                                                            Value = "",
                                                            Comment = gd.Comment,
                                                            ShowComment = gd.ShowComment,
                                                        }).ToList()

                                }).ToList();


            IEnumerable<ViewSurveyResult> surveyResultList = null;

            if (person.RoleName.ToLower().Contains("judge") == false)
            {
                surveyResultList = viewViewSurveyRepository.GetViewSurveyResults(userId, teamId);
            }
            else
            {
                surveyResultList = viewViewSurveyRepository.GetViewSurveyResults(userId);
            }
            if (surveyResultList.Count() > 0)
            {
                foreach (var question in vmSurveyList)
                {
                    foreach (var answer in question.SurveyDetailList)
                    {
                        answer.Value = surveyResultList.Where(a => a.UserId == userId && a.QuestionAnswerId == answer.QuestionAnswerId && a.Id == question.Id).First().Value ?? "";
                        answer.Comment = surveyResultList.Where(a => a.UserId == userId && a.QuestionAnswerId == answer.QuestionAnswerId && a.Id == question.Id).First().Comment ?? "";
                    }
                }
            }
            return vmSurveyList;
        }

        //public int CreateSurvey(VmSurvey vmSurvey)
        //{
        //    var result = -1;
        //    try
        //    {
        //        var questionRepository = UnitOfWork.GetRepository<ViewSurveyRepository>();

        //        var newSurvey = new Survey
        //        {
        //            Id = vmSurvey.Id,
        //            Name = vmSurvey.Name,
        //            Description = vmSurvey.Description,
        //        };

        //        questionRepository.CreateSurvey(newSurvey);

        //        UnitOfWork.Commit();

        //        result = newSurvey.Id;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = -1;
        //    }

        //    return result;
        //}

        //public bool UpdateSurvey(VmSurvey vmSurvey)
        //{
        //    try
        //    {
        //        var questionRepository = UnitOfWork.GetRepository<ViewSurveyRepository>();

        //        var updateableSurvey = new Survey
        //        {
        //            Id = vmSurvey.Id,
        //            Name = vmSurvey.Name,
        //            Description = vmSurvey.Description,
        //        };

        //        questionRepository.UpdateSurvey(updateableSurvey);

        //        return UnitOfWork.Commit();
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}
        public bool UpdateSurvey(string userId, List<VmClientSurveyResult> clientSurveyResult)
        {
            try
            {
                var blPerson = new BLPerson();
                var person = blPerson.GetPersonByUserId(userId);


                var blTeamMember = new BLTeamMember();

                VmTeamMember teamMember = null;

                var teamId = 0;

                if (person.RoleName.ToLower().Contains("judge") == false)
                {
                    teamMember = blTeamMember.GetTeamMemberByUserId(userId);
                    teamId = teamMember.TeamId;
                }

                var surveyResultRepository = UnitOfWork.GetRepository<SurveyResultRepository>();

                surveyResultRepository.DeleteSurveyResult(userId, clientSurveyResult.Select(q => q.QuestionId).ToArray());

                var surveyResultDetails = new List<SurveyResultDetail>();

                var surveyResultList = from survey in clientSurveyResult
                                       group new
                                       {
                                           survey.QuestionAnswerId,
                                           survey.Value,
                                           survey.Comment
                                       }
                                       by survey.QuestionId into g
                                       select new SurveyResult
                                       {
                                           QuestionId = g.Key,
                                           TeamId = teamId,
                                           UserId = userId,
                                           SurveyResultDetails = (from s in g.ToList()
                                                                  select new SurveyResultDetail
                                                                  {
                                                                      QuestionAnswerId = s.QuestionAnswerId,
                                                                      Value = s.Value,
                                                                      Comment = s.Comment,
                                                                  }).ToList()
                                       };

                


                if (person.RoleName.ToLower().Contains("judge") == false)
                {
                    blTeamMember.UpdateTeamMemberSurvey(userId, true);
                }

                surveyResultRepository.CreateSurveyResult(surveyResultList);

                return UnitOfWork.Commit();
            }
            catch
            {
                return false;
            }

        }
        //public bool DeleteSurvey(int questionId)
        //{
        //    try
        //    {
        //        var ViewSurveyRepository = UnitOfWork.GetRepository<ViewSurveyRepository>();


        //        if (ViewSurveyRepository.DeleteSurvey(questionId) == true)
        //        {
        //            return UnitOfWork.Commit();
        //        }
        //        return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //}
    }
}
