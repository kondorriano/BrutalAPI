using BepInEx;
using BepInEx.Configuration;
using System.IO;
using System.Xml;
using UnityEngine;

namespace BrutalAPI
{
    [BepInPlugin("BrutalOrchestra.BrutalAPI", "BrutalAPI", "0.1.5")]
    public class BrutalAPI : BaseUnityPlugin
    {
        public DebugController m_DebugController;
        private static ConfigEntry<string> _OpenDebugConsoleKey;
        public static string OpenDebugConsoleKey { get => _OpenDebugConsoleKey.Value; }

        public void Awake()
        {
            /*
            LoadedDBsHandler.UnlockablesDB.TryGetFinalBossUnlockCheck(BossTypeIDs.OsmanSinnoks.ToString(), out FinalBossCharUnlockCheck check);
            check.entityIDUnlocks.Add
                */

            LoadConfig();
            Pigments.ReplaceBasePigmentHealthSprites();


            m_DebugController = new GameObject("DebugController").AddComponent<DebugController>();

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