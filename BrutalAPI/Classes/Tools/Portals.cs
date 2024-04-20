using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class Portals
    {
        static public string NPCIDColor => "NPC";
        static public string EnemyIDColor => "Enemy";
        static public string BossIDColor => "Boss";
        static public string LootIDColor => "Loot";
        static public void AddPortalSign(string id, Sprite sprite, string colorID)
        {
            LoadedDBsHandler.PortalDB.AddNewPortalSign(id, sprite, colorID);
        }
        static public void AddPortalColor(string id, Color color)
        {
            LoadedDBsHandler.PortalDB.AddPortalColor(id, color);
        }
        static public void ModifyPortalColor(string id, Color color)
        {
            LoadedDBsHandler.PortalDB.ModifyPortalColor(id, color);
        }
    }
}
