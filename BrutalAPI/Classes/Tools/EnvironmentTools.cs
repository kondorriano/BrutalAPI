using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class EnvironmentTools
    {
        public static void PrepareCombatEnvPrefab(string prefabBundlePath, string combatEnvID, AssetBundle fileBundle)
        {
            GameObject asset = fileBundle.LoadAsset<GameObject>(prefabBundlePath);
            CombatEnvironmentHandler data = asset.GetComponent<CombatEnvironmentHandler>();
            LoadedAssetsHandler.AddExternalCombatEnvironment(combatEnvID, data);
        }
    }
}
