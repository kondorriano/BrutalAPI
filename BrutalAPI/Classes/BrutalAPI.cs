using BepInEx;
using BepInEx.Configuration;
using System.IO;
using System.Xml;
using UnityEngine;
using HarmonyLib;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace BrutalAPI
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BrutalAPI : BaseUnityPlugin
    {
        public const string GUID = "BrutalOrchestra.BrutalAPI";
        public const string NAME = "BrutalAPI";
        public const string VERSION = "0.3";

        private static Harmony HarmonyInstance;

        private static ConfigEntry<string> _OpenDebugConsoleKey;
        public static string OpenDebugConsoleKey { get => _OpenDebugConsoleKey.Value; }

        public void Awake()
        {
            LoadConfig();

            HarmonyInstance = new Harmony(GUID);
            HarmonyInstance.PatchAll();

            _ = DebugController.Instance; // Ensure DebugController's existence.

            //Load Mod Config Data
            PrepareModInfoData();
            
            Logger.LogInfo("BrutalAPI loaded successfully!");
        }

        void LoadConfig()
        {
            _OpenDebugConsoleKey = Config.Bind("Debug Console",      // The section under which the option is shown
                                         "Open Console Key",  // The key of the configuration option in the configuration file
                                         "*", // The default value
                                         "They key that needs to be pressed to open the debug console."); // Description of the option to show in the config file
        }

        void PrepareModInfoData()
        {
            LoadedDBsHandler.ModdingDB.LoadDisabledModInfo();
            ModInfoData modData = new ModInfoData(GUID, true);
            modData.name = NAME;
            modData.description = "To Do";
            modData.credits = "A bunch of cool people";
            modData.canBeDisabled = false;
            LoadedDBsHandler.ModdingDB.AddNewModInfoData(modData);
            /*
            IModInformation modTest = ModConfiguration.PrepareAndAddMyModInformation("test");
            modTest.DisplayName = "Cool Mod";
            modTest.Description = "This mod does cool things";
            modTest.Credits = "Made by CoolModder_52";
            modTest.ShowIconOnMainMenu = true;
            modTest.Icon = Pigments.Green.manaSprite;
            Debug.Log("This mod " + ((modTest.CanILoadTheMod) ? "can" : "cannot") + " be loaded");
            */
        }
    }
}