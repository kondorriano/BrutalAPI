using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static UnityEngine.TouchScreenKeyboard;

namespace BrutalAPI
{
    public class StatusField
    {
        #region Basegame Status Effect Getters
        public static StatusEffect_SO Focused => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Focused_ID.ToString()];
        public static StatusEffect_SO Ruptured => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Ruptured_ID.ToString()];
        public static StatusEffect_SO Frail => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Frail_ID.ToString()];
        public static StatusEffect_SO OilSlicked => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.OilSlicked_ID.ToString()];
        public static StatusEffect_SO Spotlight => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Spotlight_ID.ToString()];
        public static StatusEffect_SO Cursed => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Cursed_ID.ToString()];
        public static StatusEffect_SO Linked => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Linked_ID.ToString()];
        public static StatusEffect_SO DivineProtection => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.DivineProtection_ID.ToString()];
        public static StatusEffect_SO Scars => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Scars_ID.ToString()];
        public static StatusEffect_SO Gutted => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Gutted_ID.ToString()];
        public static StatusEffect_SO Stunned => LoadedDBsHandler.StatusFieldDB.StatusEffects[StatusField_GameIDs.Stunned_ID.ToString()];
        #endregion

        #region Basegame Field Effect Getters
        public static FieldEffect_SO Constricted => LoadedDBsHandler.StatusFieldDB.FieldEffects[StatusField_GameIDs.Constricted_ID.ToString()];
        public static FieldEffect_SO OnFire => LoadedDBsHandler.StatusFieldDB.FieldEffects[StatusField_GameIDs.OnFire_ID.ToString()];
        public static FieldEffect_SO Shield => LoadedDBsHandler.StatusFieldDB.FieldEffects[StatusField_GameIDs.Shield_ID.ToString()];
        #endregion

        #region Status Effect Tools
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
        #endregion

        #region Field Effect Tools
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
        #endregion
    }
}
