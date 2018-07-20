using System;
using System.Linq;
using System.Net.Http;
using HappyMeal_v3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ParserHtml;
using HappyMeal_v3.Services;

namespace HappyMeal_v3.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            var controller = context.Controller as Controller;
            var model = controller.ViewData.Model as BaseViewModel;

            if (model != null)
            {
                var parserFacade = new ParserFacade();
                var str = string.Empty;

                bool.TryParse(ConfigurationService.GetValue("fete"), out var showFete);

                if (!showFete)
                {
                    model.HeaderData = new HeaderViewModel
                    {
                        TodayFete = "..."
                    };
                    return;
                }

                var url = new Uri("http://fetedujour.fr/");
                using (var client = new HttpClient())
                {
                    try
                    {
                        var result = client.GetAsync(url).Result;
                        str = result.Content.ReadAsStringAsync().Result;
                    }
                    catch //(Exception ex)
                    {
                        model.HeaderData = new HeaderViewModel
                        {
                            TodayFete = "..."
                        };
                        return;
                    }
                }

                var fete = parserFacade.GetBlocksByClass(str, Tag.div, "fdj").First();
                var h1 = parserFacade.GetBlock(fete.Text, Tag.h1);
                var index = h1.Text.IndexOf("</span>");

                model.HeaderData = new HeaderViewModel
                {
                    TodayFete = h1.Text.Substring(index + 7).Trim()
                };
            }
        }
    }
}