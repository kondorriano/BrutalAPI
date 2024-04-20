using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    class Priority
    {
        static public PrioritySO ExtremelyFast => LoadedDBsHandler.MiscDB.GetPriority("ExtremelyFast");
        static public PrioritySO VeryFast => LoadedDBsHandler.MiscDB.GetPriority("VeryFast");
        static public PrioritySO Fast => LoadedDBsHandler.MiscDB.GetPriority("Fast");
        static public PrioritySO Normal => LoadedDBsHandler.MiscDB.GetPriority("Normal");
        static public PrioritySO Slow => LoadedDBsHandler.MiscDB.GetPriority("Slow");
        static public PrioritySO VerySlow => LoadedDBsHandler.MiscDB.GetPriority("VerySlow");
        static public PrioritySO ExtremelySlow => LoadedDBsHandler.MiscDB.GetPriority("ExtremelySlow");
    }
}
