using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SetYourTone.Models;
using System.Text;

namespace SetYourTone.Services
{
    public class FramerLineByLine
    {
        //"Большой" массив, из него вырезается кадр и отправляется в представление.
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
        public FramerLineByLine(RuleModel RuleParameters, Dictionary<string, char> inputTriggers, string currentUserFrame)
        {
            string[] splittedFrame = currentUserFrame.Split(';');
            int XLeft = Convert.ToInt32(splittedFrame[0]);
            int YTop = Convert.ToInt32(splittedFrame[1]);
            int XRight = Convert.ToInt32(splittedFrame[2]);
            int YFoot = Convert.ToInt32(splittedFrame[3]);


            
            leftBorder = RuleParameters.LeftBorder;
            rightBorder = RuleParameters.RightBorder;
            deep = YFoot + 1;
            triggerLength = RuleParameters.Length;
            triggerOffset = RuleParameters.Offset;
            leftBorder = RuleParameters.LeftBorder;
            rightBorder = RuleParameters.RightBorder;
            defaultCellMean = RuleParameters.DefaultCellMean;
            Triggers = inputTriggers;

            if (leftBorder == null) leftBorder = "";
            if (rightBorder == null) rightBorder = "";

            //Если пользователь не дал строк они = "",
            //если они в итоге меньше расстояния, затрагиваемого триггером, вызывается borderSetter
            if (leftBorder.Length < maxStepAnalyzer('r'))
            {
                leftBorder = leftBorder + defaultSymbolLine(maxStepAnalyzer('r') - leftBorder.Length);
            }
            if (rightBorder.Length < maxStepAnalyzer('l'))
            {
                rightBorder = defaultSymbolLine(maxStepAnalyzer('l') - rightBorder.Length) + rightBorder;
            }
            usersFirstString = RuleParameters.StartLayerWorkPiece;

            topLayer = expander(usersFirstString,
                                maxStepAnalyzer('l'), maxStepAnalyzer('r'),
                                XLeft, XRight,
                                leftBorder.Length, deep, out topLayerCenter);

            frame = Framer(XLeft, YTop, XRight, YFoot);
        }
        
        private string defaultSymbolLine (int length)
        {
            StringBuilder line = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                line.Append(defaultCellMean);
            }
            return line.ToString();
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
                    leftStencil = leftStencil + defaultCellMean;
                }
            }

            string rightStencil = "";
            for (int i = 1; i < howDeepWeGo; i++)
            {
                for (int k = 0; k < rightMoving; k++)
                {
                    rightStencil = rightStencil + defaultCellMean;
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
        //Функция для получения внутренней строки на основе предыдущей и присоединения границ.
        void LineCalculator(in char[] inputLine, out char[] outputLine)
        {
            outputLine = new char[inputLine.Length];
            StringBuilder triggerLineSculpt = new StringBuilder(triggerLength);
            string currentTriggerLine;
            for (int i = leftBorder.Length; i < inputLine.Length - rightBorder.Length; i++)
            {
                for (int j = 0; j < triggerLength; j++)
                {
                    if (inputLine[i + triggerOffset + j] != '0')
                        triggerLineSculpt.Append('1');
                    else
                        triggerLineSculpt.Append('0');
                    //triggerLineSculpt.Append(inputLine[i + triggerOffset + j]);
                }
                currentTriggerLine = triggerLineSculpt.ToString();
                triggerLineSculpt.Clear();

                if (Triggers.TryGetValue(currentTriggerLine, out char value))
                {
                    outputLine[i] = value;
                }
                else
                {
                    outputLine[i] = defaultCellMean;
                }
                for (int k = 0; k < leftBorder.Length; k++)
                {
                    outputLine[k] = leftBorder[k];
                }
                for (int k = inputLine.GetLength(0) - rightBorder.Length; k < inputLine.GetLength(0); k++)
                {
                    outputLine[k] = leftBorder[k - inputLine.GetLength(0) + rightBorder.Length];
                }
            }
            //outputLine = inputLine;
        }
        //Получение подмассива для пользователя - кадра.
        public char[,] Framer(int XLeft, int YTop, int XRight, int YFoot)
        {
            XLeft = XLeft + topLayerCenter;
            XRight = XRight + topLayerCenter;

            int Xlength = XRight - XLeft + 1;
            int Ylength = YFoot - YTop + 1;
            int LineLength = leftBorder.Length + topLayer.Length + rightBorder.Length;
            //Строки для вычисления, second на основе first по правилу.
            string finalFirstLine = leftBorder + topLayer + rightBorder;
            char[] first = finalFirstLine.ToCharArray();
            char[] second;

            for (int i = 0; i < YTop; i++)
            {
                LineCalculator(in first, out second);
                first = second;
            }

            //кадр, который передаётся пользователю
            char[,] frame = new char[Ylength, Xlength];
            for (int i = 0; i < Ylength; i++)
            {
                for (int j = 0; j < Xlength; j++)
                {
                    frame[i, j] = first[XLeft + j];
                }
                LineCalculator(first, out second);
                first = second;
            }
            return frame;
        }
    }
}
