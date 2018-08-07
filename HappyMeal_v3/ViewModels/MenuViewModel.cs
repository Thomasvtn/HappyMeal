using HappyMeal_v3.Services;
using Parsing;
using System.Collections.Generic;

namespace HappyMeal_v3.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public List<Cell> Starters { get; set; }
        public List<Cell> Dishes { get; set; }
        public List<Cell> Desserts { get; set; }
        public List<Cell> Sandwiches { get; set; }
        public List<Cell> Drinks { get; set; }
        public List<Extra> Extras { get; set; }

        public MenuViewModel()
        {
            Starters = new List<Cell>();
            Dishes = new List<Cell>();
            Desserts = new List<Cell>();
            Sandwiches = new List<Cell>();
            Drinks = new List<Cell>();
            Extras = new List<Extra>();
        }
    }
}