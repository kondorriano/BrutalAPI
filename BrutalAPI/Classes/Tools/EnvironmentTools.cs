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

        public static void PrepareOverworldEnvPrefab(string prefabBundlePath, string owEnvID, AssetBundle fileBundle)
        {
            GameObject asset = fileBundle.LoadAsset<GameObject>(prefabBundlePath);
            OverworldEnvironmentTransitionHandler handler = asset.AddComponent<OverworldEnvironmentTransitionHandler>();
            OverworldEnvironmentData data = asset.GetComponent<OverworldEnvironmentData>();
            handler.m_OWEnvData = data;
            LoadedAssetsHandler.AddExternalOWEnvironment(owEnvID, handler);
        }
    }
}
