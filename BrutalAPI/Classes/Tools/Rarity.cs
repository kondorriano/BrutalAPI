using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class Rarity
    {
        static public RaritySO ExtremelyCommon => LoadedDBsHandler.MiscDB.GetRarity("ExtremelyCommon");
        static public RaritySO Common => LoadedDBsHandler.MiscDB.GetRarity("Common");
        static public RaritySO Uncommon => LoadedDBsHandler.MiscDB.GetRarity("Uncommon");
        static public RaritySO Rare => LoadedDBsHandler.MiscDB.GetRarity("Rare");
        static public RaritySO VeryRare => LoadedDBsHandler.MiscDB.GetRarity("VeryRare");
        static public RaritySO AbsurdlyRare => LoadedDBsHandler.MiscDB.GetRarity("AbsurdlyRare");
        static public RaritySO Impossible => LoadedDBsHandler.MiscDB.GetRarity("Impossible");
        static public RaritySO ImpossibleNoReroll => LoadedDBsHandler.MiscDB.GetRarity("ImpossibleNoReroll");


        static public RaritySO GetCustomRarity(string id)
        {
            return LoadedDBsHandler.MiscDB.GetRarity(id);
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the Rarity but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public RaritySO CreateAndAddCustomRarityToPool(string id, int rarityValue, bool canBeRerolled = true)
        {
            RaritySO rarity = ScriptableObject.CreateInstance<RaritySO>();
            rarity.rarityValue = rarityValue;
            rarity.canBeRerolled = canBeRerolled;

            LoadedDBsHandler.MiscDB.AddNewRarity(id, rarity);
            return rarity;
        }

        static public void AddCustomRarityToPool(RaritySO rarity, string id)
        {
            LoadedDBsHandler.MiscDB.AddNewRarity(id, rarity);
        }
    }
}
