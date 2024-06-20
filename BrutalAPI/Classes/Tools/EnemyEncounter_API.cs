using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class EnemyEncounterUtils
    {
        /// <summary>
        /// Use this function to add encounters to in game zones.
        /// </summary>
        /// <param name="encounterID">Encounter ID you have set</param>
        /// <param name="priority">The higher this value the easier for this encounter to show</param>
        public static void AddEncounterToZoneSelector(string encounterID, int priority, ZoneType_GameIDs zoneID, BundleDifficulty difficulty)
        {
            LoadedDBsHandler.EnemyDB.AddBundleToSelector(encounterID, priority, zoneID.ToString(), difficulty);
        }
        /// <summary>
        /// Use this function to add SPECIAL encounters to in game zones.
        /// </summary>
        /// <param name="encounterID">Encounter ID you have set</param>
        public static void AddEncounterToZoneSpecialSelector(string encounterID, ZoneType_GameIDs zoneID, BundleDifficulty difficulty)
        {
            LoadedDBsHandler.EnemyDB.AddBundleToSpecialSelector(encounterID, zoneID.ToString(), difficulty);
        }
        /// <summary>
        /// Use this function to add encounters to custom zones.
        /// </summary>
        /// <param name="encounterID">Encounter ID you have set</param>
        /// <param name="priority">The higher this value the easier for this encounter to show</param>
        /// <param name="zoneID">Your Zone ID</param>
        public static void AddEncounterToCustomZoneSelector(string encounterID, int priority, string zoneID, BundleDifficulty difficulty)
        {
            if (!LoadedDBsHandler.EnemyDB.DoesEncounterPoolExist(zoneID))
                OverworldZone.Get_Or_CreateAndAdd_EncounterZonePool(zoneID);

            LoadedDBsHandler.EnemyDB.AddBundleToSelector(encounterID, priority, zoneID, difficulty);
        }
        /// <summary>
        /// Use this function to add SPECIAL encounters to custom zones.
        /// </summary>
        /// <param name="encounterID">Encounter ID you have set</param>
        /// <param name="zoneID">Your Zone ID</param>
        public static void AddEncounterToCustomZoneSpecialSelector(string encounterID, string zoneID, BundleDifficulty difficulty)
        {
            if(!LoadedDBsHandler.EnemyDB.DoesEncounterPoolExist(zoneID))
                OverworldZone.Get_Or_CreateAndAdd_EncounterZonePool(zoneID);

            LoadedDBsHandler.EnemyDB.AddBundleToSpecialSelector(encounterID, zoneID, difficulty);
        }
    }
    public class EnemyEncounter_API
    {
        public BaseBundleGeneratorSO encounter;
        #region ENCOUNTER PROPERTIES

        #region MAIN
        public string ID_EB
        {
            set
            {
                encounter.name = value;
            }
        }
        public string EncounterSignID
        {
            set
            {
                encounter.m_BundleSignID = value;
            }
        }
        #endregion

        #region SOUND
        public string MusicEvent
        {
            set
            {
                encounter._musicEventReference = value;
            }
        }
        public string RoarEvent
        {
            set
            {
                encounter._roarReference.roarEvent = value;
            }
        }
        #endregion

        #region EXTRA
        public string BossID
        {
            set
            {
                encounter._BossID = value;
            }
        }
        public bool UsesDialogueEvent
        {
            set
            {
                encounter._usesDialogueEvent = value;
            }
        }
        public string DialogueEvent
        {
            set
            {
                encounter._preCombatDialogueEventReference = value;
            }
        }
        public bool UsesSpecialEnvironment
        {
            set
            {
                encounter._usesSpecialEnvironment = value;
            }
        }
        public string SpecialEnvironmentID
        {
            set
            {
                encounter._specialCombatEnvironment = value;
            }
        }
        public bool UsesCustomOverworldRoom
        {
            set
            {
                encounter._usesCustomRoomPrefab = value;
            }
        }
        public string CustomOverworldRoomID
        {
            set
            {
                encounter._customRoomPrefab = value;
            }
        }
        #endregion
        
        #endregion

        public EnemyEncounter_API(EncounterType encounterType, string id_EB, string signID)
        {
            if(encounterType == EncounterType.Random)
            {
                RandomEnemyBundleSO bundle = ScriptableObject.CreateInstance<RandomEnemyBundleSO>();
                bundle._enemyBundles = new List<RandomEnemyGroup>();
                encounter = bundle;
            }
            else
            {
                SpecificEnemyBundleSO bundle = ScriptableObject.CreateInstance<SpecificEnemyBundleSO>();
                bundle._enemyBundles = new List<SpecificEnemyGroup>();
                encounter = bundle;
            }

            ID_EB = id_EB;
            EncounterSignID = signID;
            encounter._roarReference = new RoarData("");
        }

        public void AddDialogueEvent(string eventReference)
        {
            encounter._usesDialogueEvent = true;
            encounter._preCombatDialogueEventReference = eventReference;
        }
        public void AddSpecialEnvironment(string id)
        {
            encounter._usesSpecialEnvironment = true;
            encounter._specialCombatEnvironment = id;
        }
        public void AddCustomOverworldRoom(string id)
        {
            encounter._usesCustomRoomPrefab = true;
            encounter._customRoomPrefab = id;
        }
        public void CreateNewEnemyEncounterData(string[] enemyIDs, int[] slots = null)
        {
            encounter.AddEnemyData(enemyIDs, slots);
        }
        public void AddEncounterToDataBases()
        {
            LoadedDBsHandler.EnemyDB.AddNewEnemyBundle(encounter.name, encounter);
        }
    }
    public enum EncounterType
    {
        Random,
        Specific
    }
}
