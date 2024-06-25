using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class Intents
    {
        /// <summary>
        /// You should just use this for checking the intents and not modifying them .If no ID was found, it will return the default one.
        /// </summary>
        /// <returns></returns>
        static public IntentInfo GetInGame_IntentInfo(IntentType_GameIDs id)
        {
            return LoadedDBsHandler.IntentDB.TryGetIntentInfo(id.ToString());
        }
        /// <summary>
        /// You should just use this for checking the intents and not modifying them .If no ID was found, it will return the default one.
        /// </summary>
        /// <returns></returns>
        static public IntentInfo GetCustom_IntentInfo(string id)
        {
            return LoadedDBsHandler.IntentDB.TryGetIntentInfo(id);
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the IntentInfoBasic but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public IntentInfoBasic CreateAndAddCustom_Basic_IntentToPool(string id, Sprite sprite, Color color)
        {
            IntentInfoBasic intent = new IntentInfoBasic();
            intent.id = id;
            intent._sprite = sprite;
            intent._color = color;

            LoadedDBsHandler.IntentDB.AddNewBasicIntent(id, intent);
            return intent;
        }
        static public void AddCustom_Basic_IntentToPool(string id, IntentInfoBasic intent)
        {
            LoadedDBsHandler.IntentDB.AddNewBasicIntent(id, intent);
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the IntentInfoDamage but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public IntentInfoDamage CreateAndAddCustom_Damage_IntentToPool(string id, Sprite characterSprite, Color characterColor, Sprite enemySprite, Color enemyColor)
        {
            IntentInfoDamage intent = new IntentInfoDamage();
            intent.id = id;
            intent._sprite = characterSprite;
            intent._color = characterColor;
            intent._enemySprite = enemySprite;
            intent._enemyColor = enemyColor;

            LoadedDBsHandler.IntentDB.AddNewDamageIntent(id, intent);
            return intent;
        }
        static public void AddCustom_Damage_IntentToPool(string id, IntentInfoDamage intent)
        {
            LoadedDBsHandler.IntentDB.AddNewDamageIntent(id, intent);
        }
    }
}
