using HappyMeal_v3.ViewModels;
using HappyMeal_v3.Services;
using Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HappyMeal_v3.Models;
using Microsoft.AspNetCore.Mvc;

namespace HappyMeal_v3.Controllers
{
    public class HomeController : BaseController
    {
        enum Type
        {
            Starter, Dish, Dessert, Sandwich, Drink, Extra
        }

        private string file
        {
            get
            {
                var time = DateTime.Now;
                return $"Menu_{time.Day:00}{time.Month:00}{time.Year}";
            }
        }

        public ActionResult Index()
        {
            var error = false;
            var end = ConfigurationService.GetValue("endOrder");
            var match = Regex.IsMatch(end, @"\d{1,2}:\d{2}");
            var soon = Convert.ToDateTime(match ? end : "10:15") - DateTime.Now;
            if (soon.TotalMinutes >= 0)
                ViewBag.Soon = $"Il reste {(soon.Hours > 0 ? $"{soon.Hours}h et" : string.Empty)} {soon.Minutes}min pour passer la commande.";
            else
            {
                ViewBag.Late = "Trop tard ! Revenez demain.";
                error = true;
            } 

            if (TempData["Success"] != null)
                ViewBag.Success = TempData["Success"];
            if (TempData["Error"] != null)
                ViewBag.Error = TempData["Error"];

            if (!error)
            {
                var maker = new Maker(file);
                if (!System.IO.File.Exists(maker.PathDocx))
                {
                    var result = CheckMail();
                    if (!result)
                    {
                        ViewBag.Late = "Le menu du jour n'est pas encore disponible.";
                        return View(new MenuViewModel());
                    }
                }
                return View(Parsing(maker));
            }
            return View(new MenuViewModel());
        }

        [HttpPost]
        public ActionResult Send(string choices, string email)
        {
            var maker = new Maker(file);

            email = email.ToLower();
            var emailRegex = new Regex(@"^[a-z0-9-]+\.[a-z0-9-]+@trsb\.net$");
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(choices) || !emailRegex.IsMatch(email))
            {
                TempData["Error"] = "Vous n'avez pas complété tous les champs.";
                return Json(Url.Action("Index", "Home")/*, JsonRequestBehavior.AllowGet*/);
            }

            var model = Choices(choices);

            // Récupération des noms et prénoms.
            var firstName = FirstUpper(email.Split('@')[0].Split('.')[0].Replace("-", " ").ToLower());
            var lastName = FirstUpper(email.Split('@')[0].Split('.')[1].Replace("-", " ").ToLower());

            maker.Init();
            var path = maker.MakeDoc(model, $"{firstName} {lastName} - TRSb");
            if (string.IsNullOrEmpty(path))
            {
                TempData["Error"] = "Problème lors de la génération du Docx";
                return Json(Url.Action("Index", "Home")/*, JsonRequestBehavior.AllowGet*/);
            }

            var mail = new Mail();
            mail.SendMessage(email, firstName, lastName, path);

            ModelState.Clear();
            TempData["Success"] = "Commande passée avec succès.";

            return Json(Url.Action("Index", "Home")/*, JsonRequestBehavior.AllowGet*/);
        }

        [HttpGet]
        public ActionResult Download()
        {
            var path = $@"{ConfigurationService.GetValue("pathMenus")}\{file}.docx";
            if (!System.IO.File.Exists(path))
                RedirectToAction("Index");

            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/docx", $"{file}.docx");
        }

        private string FirstUpper(string str)
        {
            if (str.Length == 0)
                return string.Empty;
            return str[0].ToString().ToUpper() + str.Substring(1).ToLower();
        }

        private bool CheckMail()
        {
            var mail = new Mail();
            mail.ImapConnect();

            for (var i = mail.Inbox.Count - 1; i >= 0; i -= 1)
            {
                var file = mail.GetAttachment(i);
                if (!string.IsNullOrEmpty(file))
                {
                    var filename = file.Substring(0, file.LastIndexOf('.'));
                    var extension = file.Split('.').Last();
                    if (!extension.Equals("docx")) continue;

                    var maker = new Maker(filename, true);
                    maker.Init();
                    var date = new Date(maker.Text);
                    switch (date.IsToday())
                    {
                        case DateType.Today:
                            maker.Save();
                            mail.ImapClose();
                            return true;
                        case DateType.BeforeToday:
                            maker.DeleteFile();
                            mail.ImapClose();
                            return false;
                        default:
                            maker.DeleteFile();
                            break;
                    }
                }
            }
            mail.ImapClose();
            return false;
        }

        private MenuViewModel Parsing(Maker maker)
        {
            var model = new MenuViewModel();
            maker.Init(true);

            var regex1 = new Regex(@"Entr(é|e)e|Plat \([0-9\.]+€[^\)]*\)", RegexOptions.IgnoreCase);
            var regex2 = new Regex(@"(Dessert|Sandwich) \([0-9\.]+€[^\)]*\)", RegexOptions.IgnoreCase);
            var regex3 = new Regex(@"Boisson \([0-9\.]+€[^\)]*\)", RegexOptions.IgnoreCase);

            var part = 0;
            foreach (var table in maker.Choose.Parser.Tables)
            {
                foreach (var row in table.Rows)
                {
                    if (regex1.Match(row.Cells.First().Text).Success) { part = 1; continue; }
                    if (regex2.Match(row.Cells.First().Text).Success) { part = 2; continue; }
                    if (regex3.Match(row.Cells.First().Text).Success) { part = 3; continue; }

                    if (row.Cells.Count == 1)
                        continue;

                    if (part == 1)
                    {
                        for(var i = 0; i < row.Cells.Count; i += 2)
                        {
                            var cell = row.Cells[i];
                            var match = Regex.Match(cell.Text, @"(?<price>\d+((\.|,)\d+)?)\s*€", RegexOptions.IgnoreCase);
                            if (match.Success)
                            {
                                float.TryParse(match.Groups["price"].Value.Replace(".", ","), out var price);
                                Insert(model, price >= 4 ? Type.Dish : Type.Starter, cell);
                                continue;
                            }
                            Insert(model, i == 0 ? Type.Starter : Type.Dish, cell);
                        }
                    }
                    if (part == 2)
                    {
                        Insert(model, Type.Dessert, row.Cells[0]);
                        if (row.Count == 4)
                            Insert(model, Type.Sandwich, row.Cells[2]);
                    }
                    if (part == 3)
                    {
                        Insert(model, Type.Drink, row.Cells[0]);
                        if (row.Count >= 4) Insert(model, Type.Drink, row.Cells[2]);
                        if (row.Count == 6) Insert(model, Type.Drink, row.Cells[4]);
                    }
                }
            }
            Insert(model, Type.Extra);

            return model;
        }

        private void Insert(MenuViewModel model, Type type, Cell cell = null)
        {
            if (cell != null && !string.IsNullOrWhiteSpace(cell.Text))
            {
                if (type == Type.Starter)                   
                    model.Starters.Add(cell);
                if (type == Type.Dish)
                    model.Dishes.Add(cell);
                if (type == Type.Dessert)
                    model.Desserts.Add(cell);
                if (type == Type.Sandwich)
                    model.Sandwiches.Add(cell);
                if (type == Type.Drink)
                    model.Drinks.Add(cell);
            }
            else if (type == Type.Extra)
            {
                model.Extras.Add(Extra.Bread);
                model.Extras.Add(Extra.Cultery);
            }
        }

        private List<Choice> Choices(string str)
        {
            var result = new List<Choice>();
            var parts = str.Split(';');
            if (parts.Length == 0) return result;

            foreach (var part in parts)
            {
                var nbs = part.Split('-');
                var choice = new Choice();

                if (nbs.Length >= 2)
                {
                    choice.Id = int.Parse(nbs[0]);
                    choice.Quantity = int.Parse(nbs[1]);
                }
                if (nbs.Length > 2)
                {
                    choice.HasMulti = true;
                    choice.Multichoice = new List<int>();
                    for (var i = 3; i < nbs.Length; i += 1)
                    {
                        choice.Multichoice.Add(int.Parse(nbs[i]));
                    }
                }
                if (nbs.Length >= 2) result.Add(choice);
            }
            return result;
        }
    }
}