using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SetYourTone.Models;
namespace SetYourTone.Services
{
    public class Coordinator
    {
        public char[,] frame;
        //Сюда попадают строки по правилу, триггерам и кадру.
        //Разбиваются, заполняется словарь триггеров и структура правила,
        public Coordinator (RuleModel userRule, Dictionary<string, char> Triggers, string curUserFrame)
        {

            //string[] stringsCurFrame = curUserFrame.Split(';');

            //задаю правило
            //Rule CurRule = new Rule(userRule.Offset, userRule.Length, Triggers, userRule.DefaultCellMean, userRule.LeftBorder, userRule.RightBorder);

            //int arrDeep = Convert.ToInt32(stringsCurFrame[3])+1;

            //Дополняю заданную пользователем строку, чтобы охватить ширину, "затронутую" правилом влево и вправо 
            //на максимальной глубине.
            //int leftStep = maxStepAnalyzer('l', CurRule);
            //int rightStep = maxStepAnalyzer('r', CurRule);

            //int userXLeft = Convert.ToInt32(stringsCurFrame[0]);
            //int userXRight = Convert.ToInt32(stringsCurFrame[2]);

            //центр строки, задаётся при расширении строки
            //int layerCenter = 0;
            //расширяю пользовательскую строку
            //string curLayerExpanded = expander(userRule.StartLayerWorkPiece, leftStep, rightStep, userXLeft, userXRight,
            //                                   CurRule.leftBorder.Length, arrDeep, ref layerCenter);
            //ArrWorkers Base = new ArrWorkers(userRule, Triggers, curUserFrame);

            //Base.Filler();

            //frame = Base.Framer(userXLeft, Convert.ToInt32(stringsCurFrame[1]), userXRight, Convert.ToInt32(stringsCurFrame[3]));
            //frame = Base.frame;
        }
        //private int centerFinder(int userLayerLength)
        //{           
        //    if (userLayerLength % 2 == 1)
        //        return userLayerLength / 2;
        //    else
        //        return userLayerLength / 2 - 1;
        //}

        ////На основе отступа и длины триггера задаёт максимальный сдвиг влево или вправо.
        //private int maxStepAnalyzer(char side, Rule Triggers)
        //{
        //    int maxOfst = 0;
        //    if (side=='l')
        //    {
        //        maxOfst = Triggers.offset + Triggers.length - 1;
        //    }
        //        else
        //    {
        //        maxOfst = Math.Abs(Triggers.offset);
        //    }
        //    return maxOfst;
        //}
        //private string expander (string workPiece,int leftMoving, int rightMoving, int frameLeft, int frameRight, int leftBorderLength, int howDeepWeGo, ref int center)
        //{
        //    //на каждой строке автомат может затронуть некоторое максимальное
        //    //количество клеток левее/правее относительно верхних, на основе широты захвата триггеров,
        //    //и так можно определить зоны влево и вправо, затронутые правилом через несколько рядов
        //    //если кадр находится левее или правее итоговой ширины распространения массива то 
        //    //итоговая ширина относительно центра влево =
        //    //длина левой границы + распространение влево (или заход кадра за него) + левая часть пользовательской строки
        //    string leftStencil = "";
        //    for (int i = 1; i < howDeepWeGo; i++)
        //    {
        //        for (int k = 0; k < leftMoving; k++)
        //        {
        //            leftStencil = leftStencil + "0";
        //        }
        //    }

        //    string rightStencil = "";
        //    for (int i = 1; i < howDeepWeGo; i++)
        //    {
        //        for (int k = 0; k < rightMoving; k++)
        //        {
        //            rightStencil = rightStencil + "0";
        //        }
        //        //rightStencil = rightStencil + "0";
        //    }

        //    center = centerFinder(workPiece.Length);
        //    //расстояние от центра пользовательской строки до края массива,
        //    //включает длину пользовательской строки до центра (=center), распространение, длину границы
        //    //требуется для определения центра
        //    int leftRangeToBorder = center + leftStencil.Length;
        //    //Дополнения слева/справа, чтобы пользовательский кадр вошёл.
        //    if (frameLeft < 0)
        //    {
        //            for (int i = leftRangeToBorder; i < Math.Abs(frameLeft); i++)
        //            leftStencil = leftStencil + "0";

        //        leftRangeToBorder = center + leftStencil.Length;
        //    }

        //    if (frameRight > 0)
        //    {
        //        int rightRangeToEdge = workPiece.Length - (center + 1) + rightStencil.Length;
        //            for (int i = rightRangeToEdge; i < Math.Abs(frameRight); i++)
        //                rightStencil = rightStencil + "0";
        //    }

        //    center = leftRangeToBorder + leftBorderLength;
        //    string layer = leftStencil + workPiece + rightStencil;
        //    return layer;
        //}

    }
}
