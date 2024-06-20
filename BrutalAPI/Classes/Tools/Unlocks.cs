using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Utility.SerializableCollection;

namespace BrutalAPI
{
    public static class BackwardsUnlockCompatibility
    {
        public static void TryUnlockAchievementIfItemIsUnlocked(string itemID, UnlockableModData unlockData)
        {
            LoadedDBsHandler.ModdingDB.AddReverseItemAchCheck(itemID, unlockData);
        }

        public static void TryUnlockAchievementIfCharacterIsUnlocked(string characterID, UnlockableModData unlockData)
        {
            LoadedDBsHandler.ModdingDB.AddReverseCharacterAchCheck(characterID, unlockData);
        }

        public static void TryLockItemBehindAchievement(string achievementID, string itemID)
        {
            LoadedDBsHandler.ModdingDB.AddAchievementItemLockCheck(achievementID, itemID);
        }
    }

    public class Unlocks
    {
        public static UnlockableModData GenerateUnlockData(string id, string achievementUnlockID = "", string questUnlockID = "", string characterUnlockID = "", string[] itemUnlockIDs = null)
        {
            UnlockableModData data = new UnlockableModData(id);
            data.hasModdedAchievementUnlock = achievementUnlockID != "";
            data.moddedAchievementID = achievementUnlockID;

            data.hasQuestCompletion = questUnlockID != "";
            data.questID = questUnlockID;

            data.hasCharacterUnlock = characterUnlockID != "";
            data.character = characterUnlockID;

            data.hasItemUnlock = itemUnlockIDs != null;
            data.items = itemUnlockIDs;

            return data;
        }

        //Combat Call and Dialogue Unlocks
        /// <summary>
        /// This unlocks are mostly used to Unlock from combat or from dialogue. There is an effect you can use on combat and a call you can use on dialogue.
        /// </summary>
        /// <param name="data"></param>
        public static void AddUnlock_ByID(UnlockableModData data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddIDUnlock(data);
        }

        //Hook required Unlocks
        /// <summary>
        /// You will possibly never use this one. It would required for you to add a hook to do some checks
        /// </summary>
        /// <param name="data">This is a Scriptable Object. SingleUnlockCheck is the base but you could create your own to only unlock on certain conditions.</param>
        public static void AddUnlock_Misc(SingleUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddMiscUnlockCheck(data);
        }

        //EnemyDeathUnlockCheck
        /// <summary>
        /// You will use this one when you want to add unlocks to some enemies. You will need to create an EnemyDeathUnlockCheck
        /// </summary>
        /// <param name="data">You can set it up to have a kill unlock and multiple death type unlocks. The Id required is the enemyID.</param>
        public static void AddUnlock_EnemyDeath(EnemyDeathUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddEnemyUnlockCheck(data);
        }

        //ZoneUnlockCheck
        /// <summary>
        /// Use this one to add unlocks to your custom Zone.
        /// </summary>
        /// <param name="data">The id for ZoneUnlockCheck needs to be from ZoneNames_GameIDs</param>
        public static void AddUnlock_Zone(ZoneUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddZoneUnlockCheck(data);
        }

        //ListedUnlockCheck
        /// <summary>
        /// Use this when you want to unlock something when killing a Boss. I recommend to just use the base class ListedUnlockCheck. You could add conditions to the unlock by inheriting from ListedUnlockCheck.
        /// </summary>
        /// <param name="data">The id for the ListedUnlockCheck needs to be the BossID</param>
        public static void AddUnlock_BeatBoss(ListedUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddBossEncounterUnlockCheck(data);
        }
        /// <summary>
        /// You will possibly nver use this one, as it is just for unlocks when a character dies.
        /// </summary>
        /// <param name="data">The ID for ListedUnlockCheck needs to be a DeathID </param>
        public static void AddUnlock_CharacterDeath(ListedUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddCharacterDeathUnlockCheck(data);
        }
        /// <summary>
        /// Adds an unlock to check every time combat ends. Does not need an ID
        /// </summary>
        /// <param name="data"></param>
        /// 
        public static void AddUnlock_PostCombat(ListedUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddPostCombatUnlockCheck(data);
        }
        /// <summary>
        /// Adds an unlock to check every time the player loses combat. Does not need an ID
        /// </summary>
        /// <param name="data"></param>
        public static void AddUnlock_GameOver(ListedUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddCombatGameOverUnlockCheck(data);
        }
        /// <summary>
        /// Adds an unlock to check every time the player beats the game. Does not need an ID
        /// </summary>
        /// <param name="data"></param>
        public static void AddUnlock_HardEnding(ListedUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddHardEndingUnlockCheck(data);
        }
        /// <summary>
        /// Adds an unlock to check every time the player beats a zone. Does not need an ID
        /// </summary>
        /// <param name="data"></param>
        public static void AddUnlock_BeatingZone(ListedUnlockCheck data)
        {
            LoadedDBsHandler.UnlockablesDB.TryAddZoneExtraUnlockCheck(data);
        }

        //Final Boss Unlocks
        /// <summary>
        /// As Final boss unlocks have so much data, first get this FinalBossCharUnlockCheck class, then call AddUnlockData or AddMultipleUnlockData to add your data.
        /// If the data does not exist, this function will create new data using the bossID as Id and bossPearl as the Icon.
        /// </summary>
        /// <param name="bossID"></param>
        /// <param name="bossPearl"></param>
        /// <returns></returns>
        public static FinalBossCharUnlockCheck GetOrCreateUnlock_CustomFinalBoss(string bossID, Sprite bossPearl = null)
        {
            if(!LoadedDBsHandler.UnlockablesDB.TryGetFinalBossUnlockCheck(bossID, out FinalBossCharUnlockCheck data))
            {
                data = ScriptableObject.CreateInstance<FinalBossCharUnlockCheck>();
                data.bossID = bossID;
                data.bossDefeatedIcon = bossPearl;
                data.entityIDUnlocks = new SerializableDictionary<string, UnlockableModData>();
                LoadedDBsHandler.UnlockablesDB.TryAddFinalBossUnlockCheck(data);
            }

            return data;
        }
        /// <summary>
        /// Call AddUnlockData or AddMultipleUnlockData to add your data.
        /// </summary>
        /// <returns></returns>
        public static FinalBossCharUnlockCheck GetUnlock_HeavenFinalBoss()
        {
            LoadedDBsHandler.UnlockablesDB.TryGetFinalBossUnlockCheck(BossType_GameIDs.Heaven.ToString(), out FinalBossCharUnlockCheck data);
            return data;
        }
        /// <summary>
        /// Call AddUnlockData o AddMultipleUnlockData to add your data.
        /// </summary>
        /// <returns></returns>
        public static FinalBossCharUnlockCheck GetUnlock_OsmanFinalBoss()
        {
            LoadedDBsHandler.UnlockablesDB.TryGetFinalBossUnlockCheck(BossType_GameIDs.OsmanSinnoks.ToString(), out FinalBossCharUnlockCheck data);
            return data;
        }
    }
}
