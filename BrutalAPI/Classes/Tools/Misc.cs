using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace BrutalAPI
{
    static public class Misc
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
        static public void AddCustom_VSAnimationData(string bossID, VsBossData data)
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

        static public void Prepare_LocalizedImages(GameObject asset)
        {
            LocalizedImage_ModData locData = asset.GetComponent<LocalizedImage_ModData>();
            if (locData != null)
            {
                ImageRendererLocalizationManager locManager = asset.AddComponent<ImageRendererLocalizationManager>();
                locManager.FillWithModData(locData);
            }
        }

        #region UI Text Colors
        public enum UITextColorIDs
        {
            Positive,
            Negative,
            Parasite
        }

        /// <summary>
        /// Has the colours used by the tooltips in combat for easy access.
        /// </summary>
        /// <returns></returns>
        static public Color GetInGame_UITextColor(UITextColorIDs id)
        {
            return LoadedDBsHandler.MiscDB.GetTextColor(id.ToString());
        }
        static public Color GetCustom_UITextColor(string id)
        {
            return LoadedDBsHandler.MiscDB.GetTextColor(id);
        }
        static public void AddCustom_UITextColor(string id, Color col)
        {
            LoadedDBsHandler.MiscDB.AddNewTextColor(id, col);
        }
        #endregion

        #region Materials
        public enum MaterialIDs
        {
            Outline,
            Character,
            Character_Obliteration,
            Portal
        }

        /// <summary>
        /// Has useful materials used by the game
        /// </summary>
        /// <returns></returns>
        static public Material GetInGame_Material(MaterialIDs id)
        {
            return LoadedDBsHandler.MiscDB.GetMaterial(id.ToString());
        }
        static public Material GetCustom_Material(string id)
        {
            return LoadedDBsHandler.MiscDB.GetMaterial(id);
        }
        static public void AddCustom_Material(string id, Material mat)
        {
            LoadedDBsHandler.MiscDB.AddMaterial(id, mat);
        }
        #endregion

        #region Modular Options
        /// <summary>
        /// Has the Modular Options from the game
        /// </summary>
        /// <returns></returns>
        static public BaseModularOptSO GetInGame_ModularOption(ModularOption_GameIDs id)
        {
            return LoadedDBsHandler.MiscDB.GetModularOption(id.ToString());
        }
        static public BaseModularOptSO GetCustom_ModularOption(string id)
        {
            return LoadedDBsHandler.MiscDB.GetModularOption(id);
        }
        /// <summary>
        /// Use this one if you are creating your own prefab. Use the AddInGame otherwise (for clarity)
        /// </summary>
        static public void AddCustom_ModularOption(string id, BaseModularOptSO mOpt)
        {
            LoadedDBsHandler.MiscDB.AddNewModularOption(id, mOpt);
        }
        static public void AddInGame_ToggleModularOption(string id, Toggle_MOpt mOpt)
        {
            LoadedDBsHandler.MiscDB.AddNewModularOption(id, mOpt);
        }
        static public void AddInGame_SliderModularOption(string id, Slider_MOpt mOpt)
        {
            LoadedDBsHandler.MiscDB.AddNewModularOption(id, mOpt);
        }
        static public void AddInGame_DropdownModularOption(string id, Dropdown_MOpt mOpt)
        {
            LoadedDBsHandler.MiscDB.AddNewModularOption(id, mOpt);
        }


        /// <summary>
        /// Has the Modular Categories from the game
        /// </summary>
        /// <returns></returns>
        static public ModularCategorySO GetInGame_ModularCategory(ModularCategory_GameIDs id)
        {
            if(LoadedDBsHandler.MiscDB.TryGetModularCategory(id.ToString(), out ModularCategorySO modularCategory))
                return modularCategory;

            return null;
        }
        static public ModularCategorySO GetCustom_ModularCategory(string id)
        {
            if (LoadedDBsHandler.MiscDB.TryGetModularCategory(id, out ModularCategorySO modularCategory))
                return modularCategory;

            return null;
        }
        static public void AddInGame_ModularCategory(string id, ModularCategorySO mCat)
        {
            LoadedDBsHandler.MiscDB.AddNewModularCategory(id, mCat);
        }
        #endregion
    }
}
