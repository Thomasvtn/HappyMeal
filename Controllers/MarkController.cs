using Microsoft.AspNetCore.Mvc;
using HappyMeal_v3.Services;
using HappyMeal_v3.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace HappyMeal_v3.Controllers
{
    public class MarkController : BaseController
    {
        [HttpPost]
        public void SaveMark(MarkViewModel model)
        {
            Marks_Datase.WriteMark(model.Mark, model.Food);
        }

        public IActionResult MarkIt()
        {
            return PartialView("_Mark");
        }
    }
}
