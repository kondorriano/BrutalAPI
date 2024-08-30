using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class ModdedNPCs
    {

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the BasicEncounterSO but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public BasicEncounterSO CreateAndAddCustom_BasicEncounter(string id, string[] entityIDs, string signID, string dialogueID, string roomPrefabID)
        {
            BasicEncounterSO encounter = ScriptableObject.CreateInstance<BasicEncounterSO>();
            encounter.name = id; ;
            encounter.encounterEntityIDs = entityIDs;
            encounter.signID = signID;
            encounter._dialogue = dialogueID;
            encounter.encounterRoom = roomPrefabID;

            if (!LoadedAssetsHandler.TryAddExternalBasicEncounter(id, encounter))
                Debug.Log($"The Basic Encounter ID {id} is already in use!");

            return encounter;
        }
        static public void AddCustom_BasicEncounter(string id, BasicEncounterSO data)
        {
            if (!LoadedAssetsHandler.TryAddExternalBasicEncounter(id, data))
                Debug.Log($"The Basic Encounter ID {id} is already in use!");
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the ConditionEncounterSO but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public ConditionEncounterSO CreateAndAddCustom_ConditionEncounter(string id, string[] entityIDs, string signID,
            string dialogueID, string roomPrefabID, string questNameID, string[] requiredQuestsIDCompleted = null)
        {
            ConditionEncounterSO encounter = ScriptableObject.CreateInstance<ConditionEncounterSO>();
            encounter.name = id; ;
            encounter.encounterEntityIDs = entityIDs;
            encounter.signID = signID;
            encounter._dialogue = dialogueID;
            encounter.encounterRoom = roomPrefabID;
            encounter.m_QuestName = questNameID;
            encounter.m_QuestsCompletedNeeded = (requiredQuestsIDCompleted == null) ? new string[0] : requiredQuestsIDCompleted;

            if (!LoadedAssetsHandler.TryAddExternalConditionEncounter(id, encounter))
                Debug.Log($"The Condition Encounter ID {id} is already in use!");

            return encounter;
        }
        static public void AddCustom_ConditionEncounter(string id, ConditionEncounterSO data)
        {
            if (!LoadedAssetsHandler.TryAddExternalConditionEncounter(id, data))
                Debug.Log($"The Condition Encounter ID {id} is already in use!");
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the FreeFoolEncounterSO but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public FreeFoolEncounterSO CreateAndAddCustom_FreeFoolEncounter(string id, string[] entityIDs, string signID,
            string dialogueID, string roomPrefabID, string freeFoolCharacterID)
        {
            FreeFoolEncounterSO encounter = ScriptableObject.CreateInstance<FreeFoolEncounterSO>();
            encounter.name = id; ;
            encounter.encounterEntityIDs = entityIDs;
            encounter.signID = signID;
            encounter._dialogue = dialogueID;
            encounter.encounterRoom = roomPrefabID;
            encounter._freeFool = freeFoolCharacterID;

            if (!LoadedAssetsHandler.TryAddExternalFreeFoolEncounter(id, encounter))
                Debug.Log($"The Free Fool Encounter ID {id} is already in use!");

            return encounter;
        }
        static public void AddCustom_FreeFoolEncounter(string id, FreeFoolEncounterSO data)
        {
            if (!LoadedAssetsHandler.TryAddExternalFreeFoolEncounter(id, data))
                Debug.Log($"The Free Fool Encounter ID {id} is already in use!");
        }
    }
}
