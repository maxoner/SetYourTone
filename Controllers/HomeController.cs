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
            //Занесение значений по умолчанию
            //Верхняя строка
            userRule.StartLayerWorkPiece = "1";
            //Смещение относительно клетки
            userRule.Offset = -1;
            //Длина подстроки триггера
            userRule.Length = 3;
            //Значение реакции по умолчанию
            userRule.DefaultCellMean = '0';
            //Левая граница
            userRule.LeftBorder = "0";
            //Правая граница
            userRule.RightBorder = "0";

            StateSaving RowsState = new StateSaving();
            //Триггеры в виде "подстрока 1; реакция 1; ... подстрока n; реакция n" (hidden поле формы)
            RowsState.Triggers = "100;R;011;G;010;B;001;1;";
            //Координаты для построения кадра
            //1;2 координаты левого верхнего угла - X относительно центра, а Y положительный сверху вниз
            //3;4 координаты правого нижнего угла - X относительно центра, а Y положительный сверху вниз
            RowsState.Frame = "-10;0;10;10";
            //Соотнесение символа и цвета в виде "символ 1; цвет RGB 1; ... символ n; цвет RGB n" (hidden поле формы)
            RowsState.Colors = "R;255, 0, 0;G;0, 128, 0;B;0, 0, 255;Y;255, 255, 0;0;255, 255, 255;1;0, 0, 0;P;128, 0, 128;";
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
            ViewData["RowsState.Colors"] = RowsState.Colors;
            Dictionary<string, char> Triggers = Unwraper.TriggersUnwraper (RowsState.Triggers);
            Dictionary<char, string> Colors = Unwraper.ColorsUnwraper(RowsState.Colors);

            ViewData["ColorsDictionary"] = Colors;
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

            //Триггеры в виде "подстрока 1; реакция 1; ... подстрока n; реакция n" (hidden поле формы)
            RowsState.Triggers = RowsState.Triggers + $"{CurrentEdit.TriggerToAdd};{CurrentEdit.ReactionToAdd};";
            //Соотнесение символа и цвета в виде "символ 1; цвет RGB 1; ... символ n; цвет RGB n" (hidden поле формы)
            RowsState.Colors = RowsState.Colors + $"{CurrentEdit.ColorsKeyToAdd};{CurrentEdit.ColorsRGBToAdd};";
            Dictionary<string, char> Triggers = Unwraper.TriggersUnwraper(RowsState.Triggers);
            Dictionary<char, string> ColorsDictionary = Unwraper.ColorsUnwraper(RowsState.Colors);

            CurrentEdit.Deletion(Triggers, ColorsDictionary, RowsState);

            ViewData["RowsState.Colors"] = RowsState.Colors;
            ViewData["ColorsDictionary"] = ColorsDictionary;
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