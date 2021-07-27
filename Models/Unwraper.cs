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
            string[] TriggersStrings = stringTriggers.Split(';', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, char> TriggersDict= new Dictionary<string, char>();
            for (int i = 0; i < TriggersStrings.Length; i += 2)
            {
                TriggersDict.Add(TriggersStrings[i], Convert.ToChar(TriggersStrings[i + 1]));
            }
            return TriggersDict;
        }
    }
}
