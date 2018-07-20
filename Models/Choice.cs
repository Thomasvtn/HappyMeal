using System.Collections.Generic;

namespace HappyMeal_v3.Models
{
    public class Choice
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public bool HasMulti { get; set; }
        public List<int> Multichoice { get; set; }
    }
}