using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class Priority
    {
        static public PrioritySO ExtremelyFast => LoadedDBsHandler.MiscDB.GetPriority("ExtremelyFast");
        static public PrioritySO VeryFast => LoadedDBsHandler.MiscDB.GetPriority("VeryFast");
        static public PrioritySO Fast => LoadedDBsHandler.MiscDB.GetPriority("Fast");
        static public PrioritySO Normal => LoadedDBsHandler.MiscDB.GetPriority("Normal");
        static public PrioritySO Slow => LoadedDBsHandler.MiscDB.GetPriority("Slow");
        static public PrioritySO VerySlow => LoadedDBsHandler.MiscDB.GetPriority("VerySlow");
        static public PrioritySO ExtremelySlow => LoadedDBsHandler.MiscDB.GetPriority("ExtremelySlow");

        static public PrioritySO GetCustomPriority(string id)
        {
            return LoadedDBsHandler.MiscDB.GetPriority(id);
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the Priority but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public PrioritySO CreateAndAddCustomPriorityToPool(string id, int priorityValue)
        {
            PrioritySO priority = ScriptableObject.CreateInstance<PrioritySO>();
            priority.priorityValue = priorityValue;

            LoadedDBsHandler.MiscDB.AddNewPriority(id, priority);
            return priority;
        }

        static public void AddCustomPriorityToPool(PrioritySO priority, string id)
        {
            LoadedDBsHandler.MiscDB.AddNewPriority(id, priority);
        }
    }
}
