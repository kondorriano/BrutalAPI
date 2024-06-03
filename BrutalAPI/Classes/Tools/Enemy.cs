using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class EnemyUtils
    {
        public static void AddEnemyToSpawnPool(EnemySO enemy, PoolList_GameIDs poolListID)
        {
            if (!LoadedDBsHandler.EnemyDB.TryGetEnemyPoolEffect(poolListID.ToString(), out SpawnRandomEnemyAnywhereEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._enemies.Add(enemy);
        }
        public static void AddEnemyToHealthSpawnPool(EnemySO enemy, PoolList_GameIDs poolListID)
        {
            if (!LoadedDBsHandler.EnemyDB.TryGetEnemyPoolEffect(poolListID.ToString(), out SpawnMassivelyEverywhereUsingHealthEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._possibleEnemies.Add(enemy);
        }
        public static void AddEnemyToTransformationPool(EnemySO enemy, PoolList_GameIDs poolListID)
        {
            if (!LoadedDBsHandler.EnemyDB.TryGetEnemyPoolEffect(poolListID.ToString(), out CasterRandomTransformationEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._possibleTransformations.Add(new TransformOption(enemy));
        }


        public static void AddEnemyToCustomSpawnPool(EnemySO enemy, string poolListID)
        {
            if (!LoadedDBsHandler.EnemyDB.TryGetEnemyPoolEffect(poolListID, out SpawnRandomEnemyAnywhereEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._enemies.Add(enemy);
        }
        public static void AddEnemyToCustomHealthSpawnPool(EnemySO enemy, string poolListID)
        {
            if (!LoadedDBsHandler.EnemyDB.TryGetEnemyPoolEffect(poolListID, out SpawnMassivelyEverywhereUsingHealthEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._possibleEnemies.Add(enemy);
        }
        public static void AddEnemyToCustomTransformationPool(EnemySO enemy, string poolListID)
        {
            if (!LoadedDBsHandler.EnemyDB.TryGetEnemyPoolEffect(poolListID, out CasterRandomTransformationEffect effect))
            {
                Debug.LogError($"No Pool with ID {poolListID}");
                return;
            }

            effect._possibleTransformations.Add(new TransformOption(enemy));
        }
    }
    public class Enemy
    {
        public EnemySO enemy;

        #region ENEMY PROPERTIES

        #region MAIN
        public string ID_EN
        {
            set
            {
                enemy.name = value;
            }
        }
        public string Name
        {
            set
            {
                enemy._enemyName = value;
            }
        }
        public List<string> UnitTypes
        {
            set
            {
                enemy.unitTypes = value;
            }
        }
        public int Size
        {
            set
            {
                enemy.size = value;
            }
        }
        public PrioritySO Priority
        {
            set
            {
                enemy.priority = value;
            }
        }
        #endregion
        
        #region HEALTH
        public int Health
        {
            set
            {
                enemy.health = value;
            }
        }
        public ManaColorSO HealthColor
        {
            set
            {
                enemy.healthColor = value;
            }
        }
        #endregion

        #region ABILITIES
        public Ability[] Abilities
        {
            set
            {
                EnemyAbilityInfo[] eneAbs = new EnemyAbilityInfo[value.Length];
                for (int i = 0; i < eneAbs.Length; i++)
                    eneAbs[i] = value[i].GenerateEnemyAbility(true);
                enemy.abilities = eneAbs;
            }
        }
        public BaseAbilitySelectorSO AbilitySelector
        {
            set
            {
                enemy.abilitySelector = value;
            }
        }
        #endregion

        #region ASSETS
        public Sprite CombatSprite
        {
            set
            {
                enemy.enemySprite = value;
            }
        }
        public Sprite OverworldAliveSprite
        {
            set
            {
                enemy.enemyOverworldSprite = value;
            }
        }
        public Sprite OverworldDeadSprite
        {
            set
            {
                enemy.enemyOWCorpseSprite = value;
            }
        }
        #endregion

        #region SPECIAL EFFECTS
        public EffectInfo[] CombatEnterEffects
        {
            get
            {
                return enemy.enterEffects;
            }
            set
            {
                enemy.enterEffects = value;
            }
        }
        public EffectInfo[] CombatExitEffects
        {
            get
            {
                return enemy.exitEffects;
            }
            set
            {
                enemy.exitEffects = value;
            }
        }
        #endregion

        #region SOUNDS
        public string DamageSound
        {
            set
            {
                enemy.damageSound = value;
            }
        }
        public string DeathSound
        {
            set
            {
                enemy.deathSound = value;
            }
        }
        #endregion

        #endregion

        public Enemy(string displayName, string id_EN)
        {
            enemy = ScriptableObject.CreateInstance<EnemySO>();
            ID_EN = id_EN;
            Name = displayName;
            //Basic Priority
            enemy.priority = LoadedDBsHandler.MiscDB.DefaultPriority;
            //Basic Ability selector
            enemy.abilitySelector = LoadedDBsHandler.MiscDB.RarityAbilitySelector;

            //Initialize Lists here?
            enemy.passiveAbilities = new List<BasePassiveAbilitySO>();
            enemy.abilities = new EnemyAbilityInfo[0];
            enemy.enterEffects = new EffectInfo[0];
            enemy.exitEffects = new EffectInfo[0];
        }

        public void PrepareEnemyPrefab(string prefabBundlePath, AssetBundle fileBundle)
        {
            GameObject asset = fileBundle.LoadAsset<GameObject>(prefabBundlePath);
            EnemyInFieldLayout_Data data = asset.GetComponent<EnemyInFieldLayout_Data>();
            EnemyInFieldLayout layout = asset.AddComponent<EnemyInFieldLayout>();
            layout.m_Data = data;
            enemy.enemyTemplate = layout;
        }
        public void AddPassive(BasePassiveAbilitySO passive)
        {
            enemy.passiveAbilities.Add(passive);
        }
        public void AddPassives(BasePassiveAbilitySO[] passives)
        {
            enemy.passiveAbilities.AddRange(passives);
        }
        public void AddLootData(EnemyLootItemProbability[] data)
        {
            enemy.enemyLoot = new EnemyLoot(data);
        }
        public void AddEnemy(bool addToBronzoPool = false, bool addToSepulchrePool = false, bool addToSmallPool = false)
        {
            LoadedDBsHandler.EnemyDB.AddNewEnemy(enemy.name, enemy);

            if (addToBronzoPool)
                EnemyUtils.AddEnemyToSpawnPool(enemy, PoolList_GameIDs.Bronzo);
            if (addToSepulchrePool)
                EnemyUtils.AddEnemyToHealthSpawnPool(enemy, PoolList_GameIDs.Sepulchre);
            if (addToSmallPool)
                EnemyUtils.AddEnemyToSpawnPool(enemy, PoolList_GameIDs.SmallEnemy);
        }
    }
}
