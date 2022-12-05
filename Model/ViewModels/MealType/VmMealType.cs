using Model.Base;
using System;

namespace Model.ViewModels.MealType
{
    public class VmMealType
    {
        public int Id { get; set; }
        public int MealTypeId { get; set; }
        public int PersonId { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Checked { get; set; }

    }
}
