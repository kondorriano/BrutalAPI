using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BrutalAPI
{
    public class Misc
    {
        /// <summary>
        /// Be careful, if the ID is already in use, it will create the VSData but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public VsBossData CreateAndAddCustom_VSAnimationData(string bossID, AnimationClip clip, Sprite bossSprite, Sprite signatureSprite, Sprite arenaSprite, Sprite extraSignature = null, Sprite extraArena = null)
        {
            VsBossData data = new VsBossData();
            data.animation = clip;
            data.bossSprite = bossSprite;
            data.signatureSprite = signatureSprite;
            data.arenaSprite = arenaSprite;
            data.extraSignatureSprite = extraSignature;
            data.extraArenaSprite = extraArena;

            LoadedDBsHandler.VSAnimDB.AddVSAnimData(bossID, data);
            return data;
        }
        public static void AddCustom_VSAnimationData(string bossID, VsBossData data)
        {
            LoadedDBsHandler.VSAnimDB.AddVSAnimData(bossID, data);
        }



        /// <summary>
        /// Has some sounds used in combat for easy access.
        /// </summary>
        /// <returns></returns>
        static public string GetInGame_CombatSoundReference(CombatType_GameIDs id)
        {
            return LoadedDBsHandler.CombatDB.TryGetSoundEventName(id.ToString());
        }
        static public void AddCustom_CombatSoundReference(string id, string soundReference)
        {
            LoadedDBsHandler.CombatDB.AddNewSound(id, soundReference);
        }
        /// <summary>
        /// Has the colours used by the texts in combat for easy access.
        /// </summary>
        /// <returns></returns>
        static public TMP_ColorGradient GetInGame_TextColorGradient(CombatType_GameIDs id)
        {
            return LoadedDBsHandler.CombatDB.TryGetTextColor(id.ToString());
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the Color Gradient but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public TMP_ColorGradient CreateAndAddCustom_Simple_TextColorGradient(string id, Color color)
        {
            TMP_ColorGradient gradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
            gradient.colorMode = ColorMode.Single;
            gradient.topLeft = color;
            gradient.topRight = color;
            gradient.bottomLeft = color;
            gradient.bottomRight = color;

            LoadedDBsHandler.CombatDB.AddNewTextColor(id, gradient);
            return gradient;
        }
        /// <summary>
        /// Be careful, if the ID is already in use, it will create the Color Gradient but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public TMP_ColorGradient CreateAndAddCustom_Complex_TextColorGradient(string id, ColorMode colorMode, Color topLeft, Color topRight, Color bottomLeft, Color bottomRight)
        {
            TMP_ColorGradient gradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
            gradient.colorMode = colorMode;
            gradient.topLeft = topLeft;
            gradient.topRight = topRight;
            gradient.bottomLeft = bottomLeft;
            gradient.bottomRight = bottomRight;

            LoadedDBsHandler.CombatDB.AddNewTextColor(id, gradient);
            return gradient;
        }

        static public void AddCustom_Basic_TextColorGradient(string id, TMP_ColorGradient gradient)
        {
            LoadedDBsHandler.CombatDB.AddNewTextColor(id, gradient);
        }
    }
}
