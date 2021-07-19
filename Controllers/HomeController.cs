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
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ViewResult Index()
        {
            return View();
        }
        //public ViewResult DefUsersPage()
        [HttpGet]
        public ViewResult UserPage()
        {
            RuleModel userRule = new RuleModel();
            userRule.startWorkPiece = "1";
            userRule.Offset = -1;
            userRule.Length = 3;
            userRule.DefaultCellMean = '0';
            userRule.leftBorder = "0";
            userRule.rightBorder = "0";

            ViewData["userRule.startWorkPiece"] = userRule.startWorkPiece;
            ViewData["userRule.Offset"] = userRule.Offset;
            ViewData["userRule.Length"] = userRule.Length;
            ViewData["userRule.DefaultCellMean"] = userRule.DefaultCellMean;
            ViewData["userRule.leftBorder"] = userRule.leftBorder;
            ViewData["userRule.rightBorder"] = userRule.rightBorder;
            //Правило
            //1.Кол-во триггеров.
            //2.Смещение относительно клетки
            //3.Длина триггера
            //4.Значение клетки по умолчанию
            //5.Левая граница
            //6.Правая граница
            //7-end. Триггеры в виде подстрока; реакция - символ;
            string strCurRule = "100;R;011;G;010;B;001;1";
            ViewData["Rule"] = strCurRule;
            //Информация для построения кадра
            //1.Позиция стартовой строки в базовом массиве (на будущее)
            //2.Стартовая строка
            //3;4 координаты левого верхнего угла X относительно центра, а Y положительный сверху вниз
            //5;6 координаты правого нижнего угла X относительно центра, а Y положительный сверху вниз
            string strCurFrame = "-10;0;10;10";
            ViewData["Frame"] = strCurFrame;

            Coordinator DefaultFrame = new Coordinator(userRule, strCurRule, strCurFrame);
            ViewData["Message"] = DefaultFrame.frame;
            return View("UserPage");
        }
        [HttpPost]
        public ViewResult UserPage(RuleModel userRule, StateSaving RowsState)
        {
            ViewData["userRule.startWorkPiece"] = userRule.startWorkPiece;
            ViewData["userRule.Offset"] = userRule.Offset;
            ViewData["userRule.Length"] = userRule.Length;
            ViewData["userRule.DefaultCellMean"] = userRule.DefaultCellMean;
            ViewData["userRule.leftBorder"] = userRule.leftBorder;
            ViewData["userRule.rightBorder"] = userRule.rightBorder;
            //Правило
            //1.Смещение относительно клетки
            //2.Длина триггера
            //3.Значение клетки по умолчанию
            //4.Левая граница
            //5.Правая граница
            //6-end. Триггеры в виде подстрока; реакция - символ;
            string strCurRule = RowsState.Rule;
            ViewData["Rule"] = strCurRule;
            //Информация для построения кадра
            //1.Позиция стартовой строки в базовом массиве (на будущее)
            //2.Стартовая строка
            //3;4 координаты левого верхнего угла X относительно центра, а Y положительный сверху вниз
            //5;6 координаты правого нижнего угла X относительно центра, а Y положительный сверху вниз
            string strCurFrame = RowsState.Frame;
            ViewData["Frame"] = strCurFrame;

            Coordinator UserFrame = new Coordinator(userRule, strCurRule, strCurFrame);
            ViewData["Message"] = UserFrame.frame;
            return View("UserPage");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}