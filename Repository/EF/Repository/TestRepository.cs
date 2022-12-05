using System.Collections.Generic;
using Repository.EF.Base;
using System.Linq;
using Model;
using System;

namespace Repository.EF.Repository
{
    public class TestRepository : EFBaseRepository<Test>
    {
        public IEnumerable<Test> Select(int index, int count)
        {
            var TestList = from Test in Context.Tests
                           select Test;

            return TestList.OrderBy(A => A.Name).Skip(index).Take(count).ToArray();
        }
        public IEnumerable<Test> GetTests(string testName = "")
        {

            var testList = from test in Context.Tests
                           select test;

            if (testName != "")
            {
                testList = testList.Where(t => t.Name.Contains(testName));
            }

            return testList.ToArray();
        }

        public void CreateTest(Test newTest)
        {
            Add(newTest);
        }
        public void UpdateTest(Test updateableTest)
        {
            var oldTest = (from s in Context.Tests where s.Id == updateableTest.Id select s).FirstOrDefault();

            oldTest.Name = updateableTest.Name;
            oldTest.Description = updateableTest.Description;

            Update(oldTest);
        }
        public bool DeleteTest(int testId)
        {
            var oldTest = (from s in Context.Tests where s.Id == testId select s).FirstOrDefault();
            
            Delete(oldTest);

            return true;
        }

        public Test GetTestById(int id)
        {
            return Context.Tests.Find(id);

        }
    }
}
