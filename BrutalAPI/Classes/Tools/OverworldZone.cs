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
}
