using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SetYourTone.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SetYourTone.Services;


namespace SetYourTone.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult UserPage()
        {
            RuleModel userRule = new RuleModel();
            userRule.StartLayerWorkPiece = "1";
            userRule.Offset = -1;
            userRule.Length = 3;
            userRule.DefaultCellMean = '0';
            userRule.LeftBorder = "0";
            userRule.RightBorder = "0";

            StateSaving RowsState = new StateSaving();
            RowsState.Triggers = "100;R;011;G;010;B;001;1;";
            RowsState.Frame = "-10;0;10;10";
            //Верхняя строка при запуске
            ViewData["userRule.StartLayerWorkPiece"] = userRule.StartLayerWorkPiece;
            //Смещение относительно клетки
            ViewData["userRule.Offset"] = userRule.Offset;
            //Длина подстроки триггера
            ViewData["userRule.Length"] = userRule.Length;
            //Значение реакции по умолчанию
            ViewData["userRule.DefaultCellMean"] = userRule.DefaultCellMean;
            //Левая граница
            ViewData["userRule.LeftBorder"] = userRule.LeftBorder;
            //Правая граница
            ViewData["userRule.RightBorder"] = userRule.RightBorder;

            //Триггеры в виде подстрока; реакция - символ;
            ViewData["RowsState.Triggers"] = RowsState.Triggers;
            //Координаты для построения кадра
            //1;2 координаты левого верхнего угла - X относительно центра, а Y положительный сверху вниз
            //3;4 координаты правого нижнего угла - X относительно центра, а Y положительный сверху вниз
            ViewData["RowsState.Frame"] = RowsState.Frame;

            Dictionary<string, char> Triggers = Unwraper.TriggersUnwraper (RowsState.Triggers);

            ViewData["TriggersDictionary"] = Triggers;
            FramerLineByLine Base = new FramerLineByLine(userRule, Triggers, RowsState.Frame);
            ViewData["Message"] = Base.frame;
            return View("UserPage");
        }
        [HttpPost]
        public ViewResult UserPage(RuleModel userRule, StateSaving RowsState, Editing CurrentEdit)
        {
            //Верхняя строка от пользователя
            ViewData["userRule.StartLayerWorkPiece"] = userRule.StartLayerWorkPiece;
            //Смещение относительно клетки
            ViewData["userRule.Offset"] = userRule.Offset;
            //Длина подстроки триггера
            ViewData["userRule.Length"] = userRule.Length;
            //Значение реакции по умолчанию
            ViewData["userRule.DefaultCellMean"] = userRule.DefaultCellMean;
            //Левая граница
            ViewData["userRule.LeftBorder"] = userRule.LeftBorder;
            //Правая граница
            ViewData["userRule.RightBorder"] = userRule.RightBorder;

            //Координаты для построения кадра
            //1;2 координаты левого верхнего угла - X относительно центра, а Y положительный сверху вниз
            //3;4 координаты правого нижнего угла - X относительно центра, а Y положительный сверху вниз
            ViewData["RowsState.Frame"] = RowsState.Frame;

            //Триггеры в виде "подстрока 1; реакция 1; ... подстрока n; реакция n"
            RowsState.Triggers = RowsState.Triggers + $"{CurrentEdit.TriggerToAdd};{CurrentEdit.ReactionToAdd};";
            Dictionary<string, char> Triggers = Unwraper.TriggersUnwraper(RowsState.Triggers);

            if (CurrentEdit.TriggersKeysToDelete != null)
            {
                foreach (string keyLine in CurrentEdit.TriggersKeysToDelete)
                {
                    RowsState.Triggers = RowsState.Triggers.Replace($"{keyLine};{Triggers[keyLine]};", "");
                    Triggers.Remove(keyLine);
                }
            }
            ViewData["RowsState.Triggers"] = RowsState.Triggers;
            ViewData["TriggersDictionary"] = Triggers;
            FramerLineByLine BaseLineByLine = new FramerLineByLine(userRule, Triggers, RowsState.Frame);
            ViewData["Message"] = BaseLineByLine.frame;
            return View("UserPage");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}