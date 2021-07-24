using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetYourTone.Models
{
    //Хранение всех параметров правила кроме словаря триггеров/реакций.
    public class RuleModel
    {
        //стартовая строка, расширяется чтобы правило
        //было рассчитано будто на основе бесконечной начальной строки
        //до нижней границы кадра и входили левая - правая границы кадра 
        //и помещается в строку 0 большого массива между границами
        public string StartLayerWorkPiece { get; set; }
        public int Offset { get; set; } //смещение в предыдущей строке для поиска подстроки на сопоставление
        public int Length { get; set; } //длина подстроки триггеров
                                        //левая и правая границы, заносятся в бока большого массива,
                                        //в каждую строку.
        public char DefaultCellMean { get; set; } //значение определяемой клетки если триггеры не сработали
        public string LeftBorder { get; set; }
        public string RightBorder { get; set; }
    }
}
