﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SetYourTone.Models
{
    public static class Unwraper
    {
        public static Dictionary<string, char> TriggersUnwraper (string strCurRule)
        {
            string[] TriggersStrings = strCurRule.Split(';');
            Dictionary<string, char> TriggersDict= new Dictionary<string, char>();
            for (int i = 0; i < TriggersStrings.Length; i += 2)
            {
                TriggersDict.Add(TriggersStrings[i], Convert.ToChar(TriggersStrings[i + 1]));
            }
            return TriggersDict;
        }
    }
}