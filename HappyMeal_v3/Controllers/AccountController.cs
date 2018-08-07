using HappyMeal_v3.ViewModels;
using HappyMeal_v3.Services;
using HappyMeal_v3.Models;
using System.IO;
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
            string path = "C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/DataBase.txt";
            model.Password = model.Password.ToSha1();

            if (DataBase.Instanciate.Verifie(path, model.Password, model.Email))
                {
                    var user = new User
                    {
                        Pseudo = DataBase.Instanciate.PseudoRecup(path, model.Email),
                        Email = model.Email,
                        Password = model.Password
                    };

                    HttpContext.Session.SetObject("user", user);
                }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registration(AccountViewModel model)
        {
            if (DataBase.Instanciate.AccountWriting("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/DataBase.txt", model.Pseudo, model.Password, model.Email));
            {
                var user = new User
                {
                    Pseudo = model.Pseudo,
                    Email = model.Email,
                    Password = model.Password
                };

                HttpContext.Session.SetObject("user", user);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Account ()
        {
            return PartialView("_Account");
        }

        public IActionResult Registration2 ()
        {
            return PartialView("_Registration");
        }
    }
}
