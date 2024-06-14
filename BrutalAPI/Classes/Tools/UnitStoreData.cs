using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class UnitStoreData
    {
        static public UnitStoreData_BasicSO GetInGame_UnitStoreData(UnitStoredValueNames_GameIDs id)
        {
            return LoadedDBsHandler.MiscDB.GetUnitStoreData(id.ToString());
        }
        static public UnitStoreData_BasicSO GetCustom_UnitStoreData(string id)
        {
            return LoadedDBsHandler.MiscDB.GetUnitStoreData(id);
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the UnitStoreData but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public UnitStoreData_BasicSO CreateAndAddCustom_Basic_UnitStoreDataToPool(string id)
        {
            UnitStoreData_BasicSO usd = ScriptableObject.CreateInstance<UnitStoreData_BasicSO>();
            usd._UnitStoreDataID = id;

            LoadedDBsHandler.MiscDB.AddNewUnitStoreData(id, usd);
            return usd;
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the UnitStoreData but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public UnitStoreData_ModIntSO CreateAndAdd_IntTooltip_UnitStoreDataToPool(string id, string formattedText, Color color, bool showIfDataIsOverThreshold = true, int threshold = 0)
        {
            UnitStoreData_ModIntSO usd = ScriptableObject.CreateInstance<UnitStoreData_ModIntSO>();
            usd._UnitStoreDataID = id;
            usd.m_Text = formattedText;
            usd.m_TextColor = color;
            usd.m_ShowIfDataIsOver = showIfDataIsOverThreshold;
            usd.m_CompareDataToThis = threshold;

            LoadedDBsHandler.MiscDB.AddNewUnitStoreData(id, usd);
            return usd;
        }

        /// <summary>
        /// This one accepts any kind of Unit Store Data
        /// </summary>
        static public void AddCustom_Any_UnitStoreDataToPool(UnitStoreData_BasicSO data, string id)
        {
            LoadedDBsHandler.MiscDB.AddNewUnitStoreData(id, data);
        }
    }
}
