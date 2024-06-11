using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class Achievement
    {
        public ModdedAchievement_t achievement;

        #region PROPERTIES
        public string ID_ACH
        {
            get
            {
                return achievement.m_eAchievementID;
            }
            set
            {
                achievement.m_eAchievementID = value;
            }
        }
        public string Name
        {
            set
            {
                achievement.m_strName = value;
            }
        }
        public string Description
        {
            set
            {
                achievement.m_strDescription = value;
            }
        }
        public Sprite UnlockedSprite
        {
            set
            {
                achievement.m_unlockedSprite = value;
            }
        }
        public Sprite SpecialLockedSprite
        {
            set
            {
                achievement.m_specialLockedSprite = value;
            }
        }
        public bool IsSecret
        {
            set
            {
                achievement.m_isSecret = value;
            }
        }
        public string SecretDescription
        {
            set
            {
                achievement.m_strSecretDesctription = value;
            }
        }
        #endregion

        public Achievement(string displayName, string description, Sprite unlockSprite, string id_ACH)
        {
            achievement = new ModdedAchievement_t(id_ACH, displayName, description);
            achievement.m_unlockedSprite = unlockSprite;
            achievement.m_specialLockedSprite = null;
            achievement.m_isSecret = false;
        }

        public void SetAsSecretAchievemet(string secretDescription)
        {
            achievement.m_isSecret = true;
            achievement.m_strSecretDesctription = secretDescription;
        }

        public void AddNewAchievementToInGameCategory(AchievementCategoryIDs category)
        {
            LoadedDBsHandler.AchievementDB.AddNewAchievement(achievement, category.ToString());
        }

        /// <param name="categoryID">This is the ID your custom category has. It is also the Localization ID, if anybody wants to translate it.</param>
        /// <param name="displayName">This will be the text that will display on Screen.</param>
        public void AddNewAchievementToCUSTOMCategory(string categoryID, string displayName)
        {
            LoadedDBsHandler.AchievementDB.AddNewAchievement(achievement, categoryID, displayName);
        }
    }
}

public enum AchievementCategoryIDs
{
    BrutalOrchestraTitleLabel,
    NarrativeTitleLabel,
    PartyMembersTitleLabel,
    BossesTitleLabel,
    WitnessTitleLabel,
    DivineTitleLabel,
    StrangersTitleLabel,
    AreasTitleLabel,
    ComediesTitleLabel,
    TragediesTitleLabel
}
