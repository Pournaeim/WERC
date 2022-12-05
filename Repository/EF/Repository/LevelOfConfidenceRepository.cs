using Repository.EF.Base;
using System.Linq;
using Model;
using System;
using System.Collections.Generic;

namespace Repository.EF.Repository
{
    public class LevelOfConfidenceRepository : EFBaseRepository<LevelOfConfidence>
    {

        public void CreateLevelOfConfidence(LevelOfConfidence newLevelOfConfidence)
        {
            Add(newLevelOfConfidence);
        }
        public void UpdateLevelOfConfidence(LevelOfConfidence updateableLevelOfConfidence)
        {
            var oldLevelOfConfidence = (from s in Context.LevelOfConfidences where s.Id == updateableLevelOfConfidence.Id select s).FirstOrDefault();

            oldLevelOfConfidence.Name = updateableLevelOfConfidence.Name;

            Update(oldLevelOfConfidence);
        }
        public void DeleteLevelOfConfidence(int LevelOfConfidenceId)
        {
            var oldLevelOfConfidence = (from s in Context.LevelOfConfidences where s.Id == LevelOfConfidenceId select s).FirstOrDefault();
            Delete(oldLevelOfConfidence);
        }



        public IEnumerable<LevelOfConfidence> LevelOfConfidenceList { get; set; }
        public int Count(Func<LevelOfConfidence, bool> predicate)
        {
            return LevelOfConfidenceList.Count();
        }
        public IEnumerable<LevelOfConfidence> Select(int index = 0, int count = int.MaxValue)
        {
            var levelOfConfidenceList = from levelOfConfidence in Context.LevelOfConfidences
                                  select levelOfConfidence;

            return levelOfConfidenceList.OrderBy(A => A.Id).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<LevelOfConfidence> Select(Func<LevelOfConfidence, bool> predicate, int index, int count)
        {
            var levelOfConfidenceList = (from levelOfConfidence in Context.LevelOfConfidences
                                   select levelOfConfidence).Where(predicate);

            return levelOfConfidenceList.Skip(index).Take(count).ToArray();
        }
        public LevelOfConfidence GetLevelOfConfidenceById(int id)
        {
            var levelOfConfidence = Context.LevelOfConfidences.SingleOrDefault(a => a.Id == id);

            return levelOfConfidence;
        }
        public IEnumerable<LevelOfConfidence> Select(LevelOfConfidence filterItem, int index, int count)
        {
            var levelOfConfidenceList = from levelOfConfidence in Context.LevelOfConfidences
                                  select levelOfConfidence;

           
            if (string.IsNullOrWhiteSpace(filterItem.Name) == false)
            {
                levelOfConfidenceList = levelOfConfidenceList.Where(t => t.Name.Contains(filterItem.Name));
            }

            return levelOfConfidenceList.OrderBy(t => t.Id).Skip(index).Take(count).ToArray();

        }
    }
}
