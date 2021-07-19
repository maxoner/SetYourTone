using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace SetYourTone.Services
{      
    public class ArrWorkers
    {
        //"Большой" массив, из него вырезается кадр и отправляется в представление.
        private char[,] arr;

        //Конструктор инициализирует массив и вставляет в него первую строку и левую/правую границы по бокам.
        public ArrWorkers(string startLayer, string leftBorder , string rightBorder, int deep)
        {
            arr = new char[deep, leftBorder.Length + startLayer.Length + rightBorder.Length];
            //Выставление левой и правой границ на основе образцов string left/right border полученных от пользователя.
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < leftBorder.Length; j++)
                {
                    arr[i,j] = leftBorder[j];
                }
                for (int j = arr.GetLength(1) - rightBorder.Length; j < arr.GetLength(1); j++)
                {
                    arr[i,j] = rightBorder[j - (arr.GetLength(1) - rightBorder.Length)];
                }
            }

            // Занесение StartLayer в первою строку между границ.
            for (int i = leftBorder.Length; i < arr.GetLength(1) - rightBorder.Length; i++)
            {
                arr[0, i] = startLayer[i - leftBorder.Length];
            }
            return;
        }

        //Метод-заполнитель char массива на основе правила и массива с границами и первой строкой.
        public void Filler(Rule CurRule, int leftBorderLength, int rightBorderLength)
        {
            //Массив заполняется внутри отступов, которые больше максимального отхождения триггера от заданной точки влево или вправо соответственно.
            int leftInnerOffset = 0;
            int rightInnerOffset = 0;
            if (CurRule.offset < 0) leftInnerOffset  = Math.Abs(CurRule.offset);

            if ((CurRule.offset + (CurRule.length - 1)) > 0) rightInnerOffset = CurRule.offset + (CurRule.length - 1);

            if (leftBorderLength >= leftInnerOffset)
                leftInnerOffset = leftBorderLength;
            else
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    for (int j = leftBorderLength; j < leftInnerOffset; j++)
                    {
                        arr[i, j] = CurRule.defaultCellMean;
                    }
                }
            }
            if (rightBorderLength >= rightInnerOffset) rightInnerOffset = rightBorderLength;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = arr.GetLength(1) - rightInnerOffset; j < arr.GetLength(1) - rightBorderLength; j++)
                {
                    arr[i, j] = CurRule.defaultCellMean;
                }
            }
            //подстрока для проверки совпадения в триггерах.
            string subArr;
            //Builder для неё, очищается после присваивания к строке.
            StringBuilder builderSubArr = new StringBuilder(CurRule.length);
            
            //итерируемся по высоте со второй строки массива.
            for (int i = 1; i < arr.GetLength(0); i++)
            {
                //итерируюсь по горизонтали с левого отступа.
                for (int j = leftInnerOffset; j < arr.GetLength(1) - rightInnerOffset; j++)
                {
                    //вынимаю подстроку для триггеров.
                    for (int indSubstr = 0; indSubstr < CurRule.length; indSubstr++)
                    {
                        if (arr[i - 1, j + CurRule.offset + indSubstr] != '0')
                             builderSubArr.Append ('1');
                        else 
                             builderSubArr.Append ('0');
                    }
                    subArr = builderSubArr.ToString();
                    builderSubArr.Clear();                    
                    //Ищу в триггерах.
                    if (CurRule.Triggers.TryGetValue(subArr, out char value))
                    {
                        arr[i,j] = value;
                    }
                    else
                    {
                        arr[i, j] = CurRule.defaultCellMean;
                    }
                }
            }
        }       
        //Получение подмассива для пользователя - кадра.
        public char[,] Framer(int Xleft,int Ytop, int Xright, int Yfoot)
        {
            int Xlength = Xright - Xleft + 1;
            int Ylength = Yfoot - Ytop + 1;
            char[,] frame = new char[Xlength,Ylength];
            //Итерация по высоте.
            for (int i = 0; i < Ylength; i++)
            {
                //Итерация по строкам.
                for (int j = 0; j < Xlength; j++)
                {
                    frame[j,i] = arr[Ytop + i, Xleft + j];
                }
            }
            return frame;
        }
    }
}
