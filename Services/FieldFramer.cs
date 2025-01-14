﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SetYourTone.Models;
namespace SetYourTone.Services
{
    public class FieldFramer
    {
        //"Большой" массив, из него вырезается кадр и отправляется в представление.
        private char[,] bigArray;
        public char[,] frame;
        private string usersFirstString;
        private string topLayer;
        private string leftBorder;
        private string rightBorder;
        private int deep;
        private int triggerLength;
        private int triggerOffset;
        private char defaultCellMean;
        private Dictionary<string, char> Triggers;
        private int topLayerCenter;
        //Конструктор инициализирует массив и вставляет в него первую строку и левую/правую границы по бокам.
        public FieldFramer(RuleModel RuleParameters, Dictionary<string, char> inputTriggers, string currentUserFrame)
        {
            string[] splittedFrame = currentUserFrame.Split(';');
            int XLeft = Convert.ToInt32(splittedFrame[0]);
            int YTop = Convert.ToInt32(splittedFrame[1]);
            int XRight = Convert.ToInt32(splittedFrame[2]);
            int YFoot = Convert.ToInt32(splittedFrame[3]);

            if (RuleParameters.LeftBorder == null) RuleParameters.LeftBorder = "";
            if (RuleParameters.RightBorder == null) RuleParameters.RightBorder = "";
            usersFirstString = RuleParameters.StartLayerWorkPiece;
            leftBorder = RuleParameters.LeftBorder;
            rightBorder = RuleParameters.RightBorder;
            deep = YFoot + 1;
            triggerLength = RuleParameters.Length;
            triggerOffset = RuleParameters.Offset;
            leftBorder = RuleParameters.LeftBorder;
            rightBorder = RuleParameters.RightBorder;
            defaultCellMean = RuleParameters.DefaultCellMean;
            Triggers = inputTriggers;

            topLayer = expander(usersFirstString,
                                maxStepAnalyzer('l'), maxStepAnalyzer('r'),
                                XLeft, XRight,
                                leftBorder.Length, deep, out topLayerCenter);
            preFiller();
            Filler();
            frame = Framer(XLeft, YTop, XRight, YFoot);
        }

        private int centerFinder(int userLayerLength)
        {
            if (userLayerLength % 2 == 1)
                return userLayerLength / 2;
            else
                return userLayerLength / 2 - 1;
        }
        private int maxStepAnalyzer(char side)
        {
            int maxOfst = 0;
            if (side == 'l')
            {
                maxOfst = triggerOffset + triggerLength - 1;
            }
            else
            {
                maxOfst = Math.Abs(triggerOffset);
            }
            return maxOfst;
        }
        private string expander(string workPiece, int leftMoving, int rightMoving, 
                                int frameLeft, int frameRight, 
                                int leftBorderLength, int howDeepWeGo, out int center)
        {
            //на каждой строке автомат может затронуть некоторое максимальное
            //количество клеток левее/правее относительно верхних, на основе широты захвата триггеров,
            //и так можно определить зоны влево и вправо, затронутые правилом через несколько рядов
            //если кадр находится левее или правее итоговой ширины распространения массива то 
            //итоговая ширина относительно центра влево =
            //длина левой границы + распространение влево (или заход кадра за него) + левая часть пользовательской строки
            string leftStencil = "";
            for (int i = 1; i < howDeepWeGo; i++)
            {
                for (int k = 0; k < leftMoving; k++)
                {
                    leftStencil = leftStencil + "0";
                }
            }

            string rightStencil = "";
            for (int i = 1; i < howDeepWeGo; i++)
            {
                for (int k = 0; k < rightMoving; k++)
                {
                    rightStencil = rightStencil + "0";
                }
            }
            //центр изначальной строки
            center = centerFinder(workPiece.Length);
            //Расстояние от центра пользовательской строки до края массива,
            //включает длину пользовательской строки до центра (=center), распространение, длину границы,
            //требуется для определения центра.
            int leftRangeToBorder = center + leftStencil.Length;
            //Дополнения слева/справа, чтобы пользовательский кадр вошёл.
            if (frameLeft < 0)
            {
                for (int i = leftRangeToBorder; i < Math.Abs(frameLeft); i++)
                    leftStencil = leftStencil + "0";

                leftRangeToBorder = center + leftStencil.Length;
            }

            if (frameRight > 0)
            {
                int rightRangeToEdge = workPiece.Length - (center + 1) + rightStencil.Length;
                for (int i = rightRangeToEdge; i < Math.Abs(frameRight); i++)
                    rightStencil = rightStencil + "0";
            }

            center = leftRangeToBorder + leftBorderLength;
            string layer = leftStencil + workPiece + rightStencil;
            return layer;
        }
        //Метод инициализирует большой массив, помещает в него первую строку и границы по бокам.
        private void preFiller ()
        {
            bigArray = new char[deep, leftBorder.Length + topLayer.Length + rightBorder.Length];
            //Выставление левой и правой границ на основе образцов string left/right border полученных от пользователя.
            for (int i = 0; i < bigArray.GetLength(0); i++)
            {
                for (int j = 0; j < leftBorder.Length; j++)
                {
                    bigArray[i, j] = leftBorder[j];
                }
                for (int j = bigArray.GetLength(1) - rightBorder.Length; j < bigArray.GetLength(1); j++)
                {
                    bigArray[i, j] = rightBorder[j - (bigArray.GetLength(1) - rightBorder.Length)];
                }
            }

            // Занесение topLayer в первою строку между границ.
            for (int i = leftBorder.Length; i < bigArray.GetLength(1) - rightBorder.Length; i++)
            {
                bigArray[0, i] = topLayer[i - leftBorder.Length];
            }
            return;
        }
        //Метод-заполнитель char массива на основе правила и массива с границами и первой строкой.
        private void Filler()
        {
            //Массив заполняется внутри отступов, которые >= максимального отхождения триггера от заданной точки влево или вправо соответственно.
            int leftInnerOffset = 0;
            int rightInnerOffset = 0;
            if (triggerOffset < 0) leftInnerOffset  = Math.Abs(triggerOffset);

            if ((triggerOffset + (triggerLength - 1)) > 0) rightInnerOffset = triggerOffset + (triggerLength - 1);

            if (leftBorder.Length >= leftInnerOffset)
                leftInnerOffset = leftBorder.Length;
            else
            {
                for (int i = 0; i < bigArray.GetLength(0); i++)
                {
                    for (int j = leftBorder.Length; j < leftInnerOffset; j++)
                    {
                        bigArray[i, j] = defaultCellMean;
                    }
                }
            }
            if (rightBorder.Length >= rightInnerOffset) rightInnerOffset = rightBorder.Length;
            else
            {
                for (int i = 0; i < bigArray.GetLength(0); i++)
                {
                    for (int j = bigArray.GetLength(1) - rightInnerOffset; j < bigArray.GetLength(1) - rightBorder.Length; j++)
                    {
                        bigArray[i, j] = defaultCellMean;
                    }
                }
            }

            //подстрока для проверки совпадения в триггерах.
            string substring;
            //Builder для неё, очищается после присваивания к строке.
            StringBuilder builderSubstring = new StringBuilder(triggerLength);
            
            //итерируемся по высоте со второй строки массива.
            for (int i = 1; i < bigArray.GetLength(0); i++)
            {
                //итерируюсь по горизонтали с левого отступа.
                for (int j = leftInnerOffset; j < bigArray.GetLength(1) - rightInnerOffset; j++)
                {
                    //вынимаю подстроку для триггеров.
                    for (int indSubstr = 0; indSubstr < triggerLength; indSubstr++)
                    {
                        if (bigArray[i - 1, j + triggerOffset + indSubstr] != '0')
                            builderSubstring.Append ('1');
                        else
                            builderSubstring.Append ('0');
                    }
                    substring = builderSubstring.ToString();
                    builderSubstring.Clear();             
                    //Ищу в триггерах.
                    if (Triggers.TryGetValue(substring, out char value))
                    {
                        bigArray[i,j] = value;
                    }
                    else
                    {
                        bigArray[i, j] = defaultCellMean;
                    }
                }
            }
        }       
        //Получение подмассива для пользователя - кадра.
        public char[,] Framer(int XLeft, int YTop, int XRight, int YFoot)
        {
            XLeft  = XLeft + topLayerCenter;
            XRight = XRight + topLayerCenter;

            int Xlength = XRight - XLeft + 1;
            int Ylength = YFoot - YTop + 1;
            char[,] frame = new char[Ylength, Xlength];
            //Итерация по высоте.
            for (int i = 0; i < Ylength; i++)
            {
                //Итерация по строкам.
                for (int j = 0; j < Xlength; j++)
                {
                    frame[i, j] = bigArray[YTop + i, XLeft + j];
                }
            }
            return frame;
        }
    }
}
