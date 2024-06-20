using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class AbilityUtils
    {
        public static void AddSlapModifierToAbilityToPool(BasicAbilityChange_Wearable_SMS ability, PoolList_GameIDs poolListID)
        {
            if (!LoadedDBsHandler.AbilityDB.TryGetExtraAbilityPoolEffect(poolListID.ToString(), out CasterAddRandomExtraAbilityEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._slapData.Add(ability);
        }

        public static void AddExtraAbilityToAbilityToPool(ExtraAbility_Wearable_SMS ability, PoolList_GameIDs poolListID)
        {
            if (!LoadedDBsHandler.AbilityDB.TryGetExtraAbilityPoolEffect(poolListID.ToString(), out CasterAddRandomExtraAbilityEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._extraData.Add(ability);
        }


        public static void AddSlapModifierToCustomAbilityToPool(BasicAbilityChange_Wearable_SMS ability, string poolListID)
        {
            if (!LoadedDBsHandler.AbilityDB.TryGetExtraAbilityPoolEffect(poolListID, out CasterAddRandomExtraAbilityEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._slapData.Add(ability);
        }

        public static void AddExtraAbilityToCustomAbilityToPool(ExtraAbility_Wearable_SMS ability, string poolListID)
        {
            if (!LoadedDBsHandler.AbilityDB.TryGetExtraAbilityPoolEffect(poolListID, out CasterAddRandomExtraAbilityEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._extraData.Add(ability);
        }
    }
    public class Ability
    {
        public AbilitySO ability;

        #region ABILITY PROPERTIES
        public string ID
        {
            set
            {
                ability.name = $"{value}_AB";
            }
        }
        public string Name
        {
            set
            {
                ability._abilityName = value;
            }
        }
        public string Description
        {
            set
            {
                ability._description = value;
            }
        }
        public Sprite AbilitySprite
        {
            set
            {
                ability.abilitySprite = value;
            }
        }
        public PrioritySO Priority
        {
            set
            {
                ability.priority = value;
            }
        }
        public AttackVisualsSO Visuals
        {
            set
            {
                ability.visuals = value;
            }
        }
        public BaseCombatTargettingSO AnimationTarget
        {
            set
            {
                ability.animationTarget = value;
            }
        }
        public UnitStoreData_BasicSO UnitStoreData
        {
            set
            {
                ability.specialStoredData = value;
            }
        }
        public List<IntentTargetInfo> EffectIntents
        {
            get
            {
                return ability.intents;
            }
            set
            {
                ability.intents = value;
            }
        }
        public EffectInfo[] Effects
        {
            get
            {
                return ability.effects;
            }
            set
            {
                ability.effects = value;
            }
        }
        public ManaColorSO[] Cost { get; set; }
        public RaritySO Rarity { get; set; }
        #endregion

        #region ABILITY GENERATION PROPERTIES
        bool _AddedAbilityToCharDB = false;
        bool _CharAbilityGenerated = false;
        CharacterAbility _CharAbility = null;
        bool _AddedAbilityToEnemDB = false;
        bool _EnemAbilityGenerated = false;
        EnemyAbilityInfo _EnemAbility;
        #endregion

        public void AddIntentsToTarget(BaseCombatTargettingSO targets, string[] intents)
        {
            IntentTargetInfo info = new IntentTargetInfo();
            info.targets = targets;
            info.intents = intents;
            ability.intents.Add(info);
        }

        public Ability(string id)
        {
            ability = ScriptableObject.CreateInstance<AbilitySO>();
            ability.name = id;
            ability.effects = new EffectInfo[0];
            ability.intents = new List<IntentTargetInfo>();
            //Basic Priority
            ability.priority = LoadedDBsHandler.MiscDB.DefaultPriority;
            Rarity = LoadedDBsHandler.MiscDB.DefaultRarity;
            Cost = [];
        }
        public Ability(string name, string id)
        {
            ability = ScriptableObject.CreateInstance<AbilitySO>();
            ability.name = id;
            ability._abilityName = name;
            ability.effects = new EffectInfo[0];
            ability.intents = new List<IntentTargetInfo>();
            //Basic Priority
            ability.priority = LoadedDBsHandler.MiscDB.DefaultPriority;
            Rarity = LoadedDBsHandler.MiscDB.DefaultRarity;
            Cost = [];
        }
        /// <summary>
        /// Use this one to Clone an existing AbilitySO!
        /// </summary>
        /// <param name="abilityToClone"></param>
        /// <param name="abilityID"></param>
        public Ability(AbilitySO abilityToClone, string abilityID, ManaColorSO[] cost = null, RaritySO rarity = null)
        {
            ability = abilityToClone.Clone();
            ability.name = abilityID;
            Cost = (cost == null) ? [] :cost;
            Rarity = (rarity == null) ? LoadedDBsHandler.MiscDB.DefaultRarity : rarity;
        }

        public CharacterAbility GenerateCharacterAbility(bool addToDB = false)
        {
            if (addToDB && !_AddedAbilityToCharDB)
            {
                LoadedDBsHandler.AbilityDB.AddNewCharacterAbility(ability.name, ability);
                _AddedAbilityToCharDB = true;
            }

            if (_CharAbilityGenerated)
                return _CharAbility;

            _CharAbilityGenerated = true;
            _CharAbility = new CharacterAbility();
            _CharAbility.ability = ability;
            _CharAbility.cost = Cost;
            return _CharAbility;
        }
        public EnemyAbilityInfo GenerateEnemyAbility(bool addToDB = false)
        {
            if (addToDB && !_AddedAbilityToEnemDB)
            {
                LoadedDBsHandler.AbilityDB.AddNewEnemyAbility(ability.name, ability);
                _AddedAbilityToEnemDB = true;
            }
            
            if(_EnemAbilityGenerated)
                return _EnemAbility;

            _EnemAbilityGenerated = true;
            _EnemAbility = new EnemyAbilityInfo();
            _EnemAbility.ability = ability;
            _EnemAbility.rarity = Rarity;
            return _EnemAbility;
        }


    }
}
