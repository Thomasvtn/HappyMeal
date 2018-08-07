using Microsoft.AspNetCore.Mvc;
using HappyMeal_v3.Services;
using HappyMeal_v3.ViewModels;
using HappyMeal_v3.Models;

namespace HappyMeal_v3.Controllers
{
    public class MarkController : BaseController
    {
        [HttpPost]
        public void SaveMark(MarkViewModel model)
        {
            Marks_Datase.WriteMark(model.Mark, model.Food);

            var mark = new Mark
            {
                mark = model.Mark,
                food = model.Food
            };
        }

        public IActionResult MarkIt()
        {
            return PartialView("_Mark");
        }
    }
}
