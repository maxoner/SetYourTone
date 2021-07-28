using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetYourTone.Models
{
    public static class Unwraper
    {
        public static Dictionary<string, char> TriggersUnwraper (string stringTriggers)
        {
            string[] splittedTriggers = stringTriggers.Split(';', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, char> TriggersDict= new Dictionary<string, char>();
            for (int i = 0; i < splittedTriggers.Length; i += 2)
            {
                TriggersDict.Add(splittedTriggers[i], Convert.ToChar(splittedTriggers[i + 1]));
            }
            return TriggersDict;
        }
        public static Dictionary<char, string> ColorsUnwraper(string stringColors)
        {
            string[] splittedColors = stringColors.Split(';', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<char, string> ColorsDict = new Dictionary<char, string>();
            for (int i = 0; i < splittedColors.Length; i += 2)
            {
                ColorsDict.Add(Convert.ToChar(splittedColors[i]), splittedColors[i + 1]);
            }
            return ColorsDict;
        }
    }
}
