using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace SetYourTone.Services
{
    public struct Rule
    // Структура содержит всю информацию для
    // получения значения клетки на основе предыдущей строки
    {
        /* если смещение и длину подстроки задавать отдельно для каждого триггера, 
         потребуется решать конфликты/коллизии между ними*/
        public int offset; //смещение в предыдущей строке для поиска подстроки
        public int length; //длина подстроки триггеров
        public Dictionary<string, char> Triggers;// словарь с триггерами вида подстрока - значение клетки
        public char defaultCellMean;
        public string leftBorder;
        public string rightBorder;//значение определяемой клетки если триггеры не сработали
        public Rule(int offset, int length, Dictionary<string, char> Triggers, char defaultCellMean, string leftBorder, string rightBorder)
        {
            this.offset = offset;
            this.length = length;
            this.Triggers = Triggers;
            this.defaultCellMean = defaultCellMean;
            this.leftBorder = leftBorder;
            this.rightBorder = rightBorder;
        }
    }
}