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
        public string[] Colors { get; set; }
    }
}
