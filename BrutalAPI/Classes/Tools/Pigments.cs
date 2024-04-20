using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public static class Pigments
    {
        static public ManaColorSO Blue => LoadedDBsHandler.PigmentDB.GetPigment("Blue");
        static public ManaColorSO Purple => LoadedDBsHandler.PigmentDB.GetPigment("Purple");
        static public ManaColorSO Red => LoadedDBsHandler.PigmentDB.GetPigment("Red");
        static public ManaColorSO Yellow => LoadedDBsHandler.PigmentDB.GetPigment("Yellow");
        static public ManaColorSO Grey => LoadedDBsHandler.PigmentDB.GetPigment("Grey");
        static public ManaColorSO Green => LoadedDBsHandler.PigmentDB.GetPigment("Green");
        static public ManaColorSO BluePurple => LoadedDBsHandler.PigmentDB.GetPigment("BP");
        static public ManaColorSO BlueRed => LoadedDBsHandler.PigmentDB.GetPigment("BR");
        static public ManaColorSO BlueYellow => LoadedDBsHandler.PigmentDB.GetPigment("BY");
        static public ManaColorSO PurpleBlue => LoadedDBsHandler.PigmentDB.GetPigment("PB");
        static public ManaColorSO PurpleRed => LoadedDBsHandler.PigmentDB.GetPigment("PR");
        static public ManaColorSO PurpleYellow => LoadedDBsHandler.PigmentDB.GetPigment("PY");
        static public ManaColorSO RedBlue => LoadedDBsHandler.PigmentDB.GetPigment("RB");
        static public ManaColorSO RedPurple => LoadedDBsHandler.PigmentDB.GetPigment("RP");
        static public ManaColorSO RedYellow => LoadedDBsHandler.PigmentDB.GetPigment("RY");
        static public ManaColorSO YellowBlue => LoadedDBsHandler.PigmentDB.GetPigment("YB");
        static public ManaColorSO YellowRed => LoadedDBsHandler.PigmentDB.GetPigment("YR");
        static public ManaColorSO YellowPurple => LoadedDBsHandler.PigmentDB.GetPigment("YP");
        static public ManaColorSO GetPigmentWithID(string id)
        {
            return LoadedDBsHandler.PigmentDB.GetPigment(id);
        }
        static public void AddNewPigment(string id, ManaColorSO pigment)
        {
            LoadedDBsHandler.PigmentDB.AddNewPigment(id, pigment);
        }
    }
}
