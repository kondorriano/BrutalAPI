using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.TouchScreenKeyboard;

namespace BrutalAPI
{
    public class StatusField
    {
        static public void AddNewStatusEffect(StatusEffect_SO status, bool addToGlossary = true)
        {
            LoadedDBsHandler.StatusFieldDB.AddNewStatusEffect(status, addToGlossary);
        }
        static public StatusEffect_SO GetInGameStatusEffect(StatusField_GameIDs statusID)
        {
            if(LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(statusID.ToString(), out StatusEffect_SO status))
                Debug.LogError($"No Status with ID {statusID}. Did you set a field ID instead?");
            return status;
        }
        static public StatusEffect_SO GetCustomStatusEffect(string statusID)
        {
            if(!LoadedDBsHandler.StatusFieldDB.TryGetStatusEffect(statusID, out StatusEffect_SO status))
                Debug.LogError($"No Status with ID {statusID}");

            return status;
        }


        static public void AddNewFieldEffect(FieldEffect_SO field, bool addToGlossary = true)
        {
            LoadedDBsHandler.StatusFieldDB.AddNewFieldEffect(field, addToGlossary);
        }
        static public FieldEffect_SO GetInGameFieldEffect(StatusField_GameIDs fieldID)
        {
            if (LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect(fieldID.ToString(), out FieldEffect_SO field))
                Debug.LogError($"No Status with ID {fieldID}. Did you set a status ID instead?");

            return field;
        }
        static public FieldEffect_SO GetCustomFieldEffect(string fieldID)
        {
            if (!LoadedDBsHandler.StatusFieldDB.TryGetFieldEffect(fieldID, out FieldEffect_SO field))
                Debug.LogError($"No Status with ID {fieldID}.");

            return field;
        }

        static public void PrepareFieldEffectPrefabs(string characterFieldPrefabBundlePath, string enemyFieldPrefabBundlePath, AssetBundle fileBundle, SlotStatusEffectInfoSO info)
        {
            GameObject charAsset = fileBundle.LoadAsset<GameObject>(characterFieldPrefabBundlePath);
            CharacterFieldEffectLayout charData = charAsset.GetComponent<CharacterFieldEffectLayout>();
            info.m_CharacterLayoutTemplate = charData;

            GameObject enemyAsset = fileBundle.LoadAsset<GameObject>(enemyFieldPrefabBundlePath);
            EnemyFieldEffectLayout enemData = enemyAsset.GetComponent<EnemyFieldEffectLayout>();
            info.m_EnemyLayoutTemplate = enemData;
        }
    }
}
