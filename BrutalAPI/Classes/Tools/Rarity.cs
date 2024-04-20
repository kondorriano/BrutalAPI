using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public class Rarity
    {
        static public RaritySO ExtremelyCommon => LoadedDBsHandler.MiscDB.GetRarity("ExtremelyCommon");
        static public RaritySO Common => LoadedDBsHandler.MiscDB.GetRarity("Common");
        static public RaritySO Uncommon => LoadedDBsHandler.MiscDB.GetRarity("Uncommon");
        static public RaritySO Rare => LoadedDBsHandler.MiscDB.GetRarity("Rare");
        static public RaritySO VeryRare => LoadedDBsHandler.MiscDB.GetRarity("VeryRare");
        static public RaritySO AbsurdlyRare => LoadedDBsHandler.MiscDB.GetRarity("AbsurdlyRare");
        static public RaritySO Impossible => LoadedDBsHandler.MiscDB.GetRarity("Impossible");
        static public RaritySO ImpossibleNoReroll => LoadedDBsHandler.MiscDB.GetRarity("ImpossibleNoReroll");
    }
}
