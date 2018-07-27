using HappyMeal_v3.ViewModels;
using HappyMeal_v3.Services;
using HappyMeal_v3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HappyMeal_v3.Extensions;

namespace HappyMeal_v3.Controllers
{
    public class AccountController : BaseController
    {

        [HttpPost]
        public IActionResult Authentication(AccountViewModel model)
        {

            if (DataBase.Verifie("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/DataBase.txt", model.Pseudo, model.Password, model.Email))
                {
                    var user = new User
                    {
                        Pseudo = model.Pseudo,
                        Email = model.Email,
                        Password = model.Password
                    };

                    HttpContext.Session.SetObject("user", user);

                    var viewmodel = new ConnectedViewModel
                    {
                        Pseudo = model.Pseudo
                    };

                    //return View("_Connected", viewmodel);

                }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Account ()
        {
            return PartialView("_Account");
        }
    }
}
