using BepInEx;
using UnityEngine;

namespace BrutalAPI
{
    [BepInPlugin("BrutalOrchestra.BrutalAPI", "BrutalAPI", "0.1.5")]
    public class BrutalAPI : BaseUnityPlugin
    {
        public void Awake()
        {
            //LoadedDBsHandler.UnlockablesDB.TryGetFinalBossUnlockCheck(BossTypeIDs.OsmanSinnoks.ToString(), out FinalBossCharUnlockCheck check);
            //check.entityIDUnlocks.a
        }
    }
}
