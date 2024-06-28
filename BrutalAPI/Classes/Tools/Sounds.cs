using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class Sounds
    {
        static public string SoundsFolder => Application.dataPath + "/../BepInEx/plugins/Sounds";
        static public string BepInExPluginsFolder => Application.dataPath + "/../BepInEx/plugins";

        /// <summary>
        /// Adds into the Banks Database the path of your sound bank.
        /// </summary>
        /// <param name="folderLocation">I recommend to set all your Banks into the Sounds folder inside plugins</param>
        /// <param name="bankName">The name of your Bank, without the .bank or .strings.bank termination.</param>
        static public void AddSoundBankFromCustomFolderPath(string folderLocation, string bankName)
        {
            LoadedDBsHandler.ModdingDB.AddNewSoundBanks(bankName, folderLocation);
            //LoadedAssetsHandler.GetEnemyBundle("TUTORIAL_Encounter_01_EnemyBundle")._musicEventReference = "event:/Music/LorenzoTheme";
        }
        static public void AddSoundBankFromSoundsFolder(string bankName)
        {
            LoadedDBsHandler.ModdingDB.AddNewSoundBanks(bankName, SoundsFolder);
            //LoadedAssetsHandler.GetEnemyBundle("TUTORIAL_Encounter_01_EnemyBundle")._musicEventReference = "event:/Music/LorenzoTheme";
        }
    }
}
