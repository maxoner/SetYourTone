using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetYourTone.Models
{
    //Хранение всех параметров правила кроме словаря триггеров/реакций.
    public class RuleModel
    {
        //стартовая строка, расширяется для точного расчёта на нужной глубине и помещается в строку 0 большого массива между границами
        public string startWorkPiece { get; set; } 
        //левая и правая границы, заносятся в бока большого массива, в каждую строку.
        public string leftBorder { get; set; }
        public string rightBorder { get; set; }
        public int Offset { get; set; } //смещение в предыдущей строке для поиска подстроки на сопоставление
        public int Length { get; set; } //длина подстроки триггеров
        public char DefaultCellMean { get; set; } //значение определяемой клетки если триггеры не сработали
    }
}
