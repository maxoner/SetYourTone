using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SetYourTone.Models
{
    public class Editing
    {
        public string TriggerToAdd { get; set; }
        public string ReactionToAdd { get; set; }
        public string[] TriggersKeysToDelete { get; set; }
        public string[] ColorsKeysToDelete { get; set; }
        public string ColorsKeyToAdd { get; set; }
        public string ColorsRGBToAdd { get; set; }

        public void Deletion (Dictionary<string, char> TriggersDictionary, Dictionary<char, string> ColorsDictionary, StateSaving CurrentState)
        {
            if (TriggersKeysToDelete != null)
            {
                foreach (string keyLine in TriggersKeysToDelete)
                {
                    CurrentState.Triggers = CurrentState.Triggers.Replace($"{keyLine};{TriggersDictionary[keyLine]};", "");
                    TriggersDictionary.Remove(keyLine);
                }
            }
            if (ColorsKeysToDelete != null)
            {
                foreach (string key in ColorsKeysToDelete)
                {
                    CurrentState.Colors = CurrentState.Colors.Replace($"{Convert.ToChar(key)};{ColorsDictionary[Convert.ToChar(key)]};", "");
                    ColorsDictionary.Remove(Convert.ToChar(key));
                    if (TriggersDictionary.ContainsValue(Convert.ToChar(key)))
                    {
                        foreach (string Triggerkey in TriggersDictionary.Keys)
                        {
                            if (TriggersDictionary[Triggerkey] == Convert.ToChar(key))
                            {
                                CurrentState.Triggers = CurrentState.Triggers.Replace($"{Triggerkey};{TriggersDictionary[Triggerkey]};", "");
                                TriggersDictionary.Remove(Triggerkey);
                            }
                        }

                    }
                    
                }
            }
        }

        //    Dictionary<char, string> ColorsDictionary = Unwraper.ColorsUnwraper(RowsState.Colors);

    }
}
