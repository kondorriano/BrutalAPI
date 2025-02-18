using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace BrutalAPI
{
    static public class OverworldZone
    {
        #region ZONE DBs
        /// <summary>
        /// Adds your custom zone to an in game run mode. 
        /// </summary>
        /// <returns></returns>
        static public void Add_ZoneToInGameRunMode(RunType_GameIDs runID, int zoneIndex, string zoneID, int zoneWeight = 100)
        {
            ZoneData data = new ZoneData(zoneID, zoneWeight);
            LoadedDBsHandler.MiscDB.AddZoneToRunInformation(runID.ToString(), zoneIndex, data);
        }
        /// <summary>
        /// Adds your custom zone to a custom run mode. 
        /// </summary>
        /// <returns></returns>
        static public void Add_ZoneToCustomRunMode(string runID, int zoneIndex, string zoneID, int zoneWeight = 100)
        {
            ZoneData data = new ZoneData(zoneID, zoneWeight);
            LoadedDBsHandler.MiscDB.AddZoneToRunInformation(runID, zoneIndex, data);
        }
        /// <summary>
        /// Be careful, if the ID is already in use, it will create the Zone but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public ZoneBGDataBaseSO CreateAndAdd_BasicZoneData(string zoneID, string overworld_EnvID, string combat_EnvID)
        {
            ZoneBGDataBaseSO zone = ScriptableObject.CreateInstance<ZoneBGDataBaseSO>();
            zone.name = zoneID;
            zone._zoneID = zoneID;
            zone._baseOWEnvironment = overworld_EnvID;
            zone._baseCombatEnvironment = combat_EnvID;


            LoadedDBsHandler.MiscDB.AddNewZone(zoneID, zone);
            return zone;
        }
        /// <summary>
        /// Be careful, if the ID is already in use, it will create the Zone but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public ManualZoneBGDataBaseSO CreateAndAdd_ManualZoneData(string zoneID, string overworld_EnvID, string combat_EnvID)
        {
            ManualZoneBGDataBaseSO zone = ScriptableObject.CreateInstance<ManualZoneBGDataBaseSO>();
            zone.name = zoneID;
            zone._zoneID = zoneID;
            zone._baseOWEnvironment = overworld_EnvID;
            zone._baseCombatEnvironment = combat_EnvID;


            LoadedDBsHandler.MiscDB.AddNewZone(zoneID, zone);
            return zone;
        }
        static public void AddCustom_VSAnimationData(string zoneID, ZoneDataBaseSO data)
        {
            LoadedDBsHandler.MiscDB.AddNewZone(zoneID, data);
        }
        #endregion

        #region ZONE Encounter Pools
        /// <summary>
        /// Looks for the Encounter pool, if it does not exists, creates and adds it
        /// </summary>
        /// <returns></returns>
        static public EnemyEncounterZoneData Get_Or_CreateAndAdd_EncounterZonePool(string zoneTypeID)
        {
            EnemyDataBase enemyDB = LoadedDBsHandler.EnemyDB;
            if (enemyDB.DoesEncounterPoolExist(zoneTypeID))
                return enemyDB.GetEncounterData(zoneTypeID);

            EnemyEncounterZoneData defaultData = enemyDB.m_DefaultEncounterData;
            EnemyEncounterZoneData zone = new EnemyEncounterZoneData();
            zone.m_EasySelector = defaultData.m_EasySelector.Clone();
            zone.m_MediumSelector = defaultData.m_MediumSelector.Clone();
            zone.m_HardSelector = defaultData.m_HardSelector.Clone();
            zone.m_SpecialSelector = defaultData.m_SpecialSelector.Clone();

            enemyDB.AddEnemyEncounterPool(zoneTypeID, zone);

            return zone;
        }
        #endregion

    }

    public class BasicOverworldZone
    {
        public ZoneBGDataBaseSO zone;

        #region IMPORTANT
        public string ID
        {
            set
            {
                zone.name = value;
                zone._zoneID = value;
            }
        }
        public string ZoneTypeID
        {
            set
            {
                zone.m_ZoneTypeID = value;
            }
        }
        public string OWEnvironment
        {
            set
            {
                zone._baseOWEnvironment = value;
            }
        }
        public string CombatEnvironment
        {
            set
            {
                zone._baseCombatEnvironment = value;
            }
        }

        public string StepSoundEvent
        {
            set
            {
                zone._stepSounds = value;
            }
        }
        public string OverworldMusicEvent
        {
            set
            {
                zone._overworldMusicEvent = value;
            }
        }

        public ZoneLootExperience ZoneLoot
        {
            set
            {
                zone._zoneLootCalculator = value;
            }
        }

        public DeckInfoSO Deck
        {
            set
            {
                zone._deckInfo = value;
            }
        }
        #endregion



        #region Rank
        public int MaxLevelUpRank
        {
            set
            {
                zone._maxLevelUpRank = value;
            }
        }
        public int EncounterLevelRank
        {
            set
            {
                zone._encounterLevelRank = value;
            }
        }

        public int FoolsLevelRank
        {
            set
            {
                zone._foolsRank = value;
            }
        }

        #endregion

        #region Pools Stuff
        public List<string> QuestPoolIDs
        {
            set
            {
                zone._QuestPool = value;
            }
        }

        public List<string> SpecialQuestPoolIDs
        {
            set
            {
                zone._SpecialQuestPool = value;
            }
        }

        public List<string> FreeFoolsIDs
        {
            set
            {
                zone._FreeFoolsPool = value;
            }
        }

        public List<string> FlavourPoolIDs
        {
            set
            {
                zone._FlavourPool = value;
            }
        }
        public List<string> OmittedCharacterIDs
        {
            set
            {
                zone._omittedCharacters = value;
            }
        }
        #endregion

        #region Money Data
        public int MinMoneyChestAmount
        {
            set
            {
                zone._minMoneyChestAmount = value;
            }
        }
        public int MaxMoneyChestAmount
        {
            set
            {
                zone._maxMoneyChestAmount = value;
            }
        }
        #endregion

        #region Signs
        public string ShopSignID
        {
            set
            {
                zone.m_ShopSignID = value;
            }
        }
        public string FoolsSignID
        {
            set
            {
                zone.m_FoolsSignID = value;
            }
        }
        public string PrizeSignID
        {
            set
            {
                zone.m_ItemSignID = value;
            }
        }
        public string MoneyChestSignID
        {
            set
            {
                zone.m_MoneyChestSignID = value;
            }
        }
        #endregion

        #region Rooms
        public string ShopRoomID
        {
            set
            {
                zone._shopRoom = value;
            }
        }
        public string FoolsRoomID
        {
            set
            {
                zone._foolsRoom = value;
            }
        }
        public string PrizeRoomID
        {
            set
            {
                zone._itemRoom = value;
            }
        }
        public string MoneyChestRoomID
        {
            set
            {
                zone._moneyChestRoom = value;
            }
        }
        #endregion

        public BasicOverworldZone(string zoneID, string overworld_EnvID, string combat_EnvID, string zoneTypeID = "")
        {
            zone = ScriptableObject.CreateInstance<ZoneBGDataBaseSO>();
            ID = zoneID;
            ZoneTypeID = (zoneTypeID == "") ? zoneID : zoneTypeID;

            OWEnvironment = overworld_EnvID;
            CombatEnvironment = combat_EnvID;

            ShopSignID = "Shop";
            FoolsSignID = "Fools";
            PrizeSignID = "Prize";
            MoneyChestSignID = "MoneyChest";

            ShopRoomID = "Shop_Zone01_Room";
            FoolsRoomID = "Fools_Zone01_Room";
            PrizeRoomID = "Prize_Room";
            MoneyChestRoomID = "MoneyChest_Room";
        }

        public void SetRankData(int maxLevelUpRank, int maxEncounterLevelRank, int foolsLevelRank)
        {
            MaxLevelUpRank = maxLevelUpRank;
            EncounterLevelRank = maxEncounterLevelRank;
            FoolsLevelRank = foolsLevelRank;
        }
        public void SetMoneyChestAmount(int minAmount, int maxAmount)
        {
            MinMoneyChestAmount = minAmount;
            MaxMoneyChestAmount = maxAmount;
        }

        public void AddZone()
        {
            LoadedDBsHandler.MiscDB.AddNewZone(zone._zoneID, zone);
        }
    }
}
