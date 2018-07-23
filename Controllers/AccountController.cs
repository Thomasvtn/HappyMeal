using HappyMeal_v3.ViewModels;
using HappyMeal_v3.Services;
using System;
using Microsoft.AspNetCore.Mvc;

namespace HappyMeal_v3.Controllers
{
    public class AccountController : HomeController
    {
        [HttpPost]
        public void Authentication(AccountViewModel model)
        {
            if (DataBase.Verifie("C:/Users/adelamare/Downloads/Thomas aka le stagiaire/HappyMeal_v3/HappyMeal_v3/DataBase.txt", model.Pseudo, model.Password, model.Email))
                {
                
                }
        }
    }
}
