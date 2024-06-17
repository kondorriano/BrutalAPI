using BepInEx;
using BepInEx.Configuration;
using System.IO;
using System.Xml;
using UnityEngine;
using HarmonyLib;

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

            Logger.LogInfo("BrutalAPI loaded successfully!");
        }

        void LoadConfig()
        {
            _OpenDebugConsoleKey = Config.Bind("Debug Console",      // The section under which the option is shown
                                         "Open Console Key",  // The key of the configuration option in the configuration file
                                         "*", // The default value
                                         "They key that needs to be pressed to open the debug console."); // Description of the option to show in the config file
        }
    }
}