using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class ModConfiguration
    {
        static public IModInformation PrepareAndAddMyModInformation(string modID)
        {
            bool isDisabled = LoadedDBsHandler.ModdingDB.IsModDisabled(modID);
            ModInfoData modData = new ModInfoData(modID, !isDisabled);
            LoadedDBsHandler.ModdingDB.AddNewModInfoData(modData);
            return modData;
        }
        static public IModInformation PrepareAndAddMyModInformation(string modID, string name, string description, string credits, Sprite icon = null, bool showIconOnMainMenu = false)
        {
            bool isDisabled = LoadedDBsHandler.ModdingDB.IsModDisabled(modID);
            ModInfoData modData = new ModInfoData(modID, !isDisabled);
            modData.name = name;
            modData.description = description;
            modData.credits = credits;
            modData.icon = icon;
            modData.showIconOnMainMenu = showIconOnMainMenu;

            LoadedDBsHandler.ModdingDB.AddNewModInfoData(modData);
            return modData;
        }

        static public bool IsModDisabled(string modID) 
        { 
            return LoadedDBsHandler.ModdingDB.IsModDisabled(modID);
        }
    }
}
