using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI 
{
    public static class ItemUtils
    {
        /*       
        Unlock ID Set?
         */

        #region Item Pools
        /// <summary>
        /// Just adds the item so it can be loaded in game. But does not add it into any item stats pool or the Shop or Treasure Pool.
        /// </summary>
        /// <param name="item"></param>
        public static void JustAddItemSoItCanBeLoaded(BaseWearableSO item)
        {
            LoadedDBsHandler.ItemUnlocksDB.AddNewItem(item.name, item);
        }
        /// <summary>
        /// Adds your item so it can be loaded in game. Also adds it into the Shop stats and Shop Pool.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemStats"></param>
        public static void AddItemToShopStatsCategoryAndGamePool(BaseWearableSO item, ItemModdedUnlockInfo itemStats = null)
        {
            item.isShopItem = true;
            LoadedDBsHandler.ItemUnlocksDB.AddNewItem(item.name, item, itemStats, true, false, "Shop", "Shop");
        }
        /// <summary>
        /// Adds your item so it can be loaded in game. Also adds it into the Treasure stats and Treasure pool.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemStats"></param>
        public static void AddItemToTreasureStatsCategoryAndGamePool(BaseWearableSO item, ItemModdedUnlockInfo itemStats = null)
        {
            item.isShopItem = false;
            LoadedDBsHandler.ItemUnlocksDB.AddNewItem(item.name, item, itemStats, false, true, "Treasure", "Treasure");
        }
        /// <summary>
        /// Adds your item so it can be loaded. Also adds it into a custom stats pool. Anad can also be added to Shop or Treasure pool.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="categoryID"></param>
        /// <param name="categoryDisplayName"></param>
        /// <param name="itemStats"></param>
        /// <param name="addToShopGamePool"></param>
        /// <param name="addToTreasureGamePool"></param>
        public static void AddItemToCustomStatsCategoryAndGamePool(BaseWearableSO item, string categoryID, string categoryDisplayName,
            ItemModdedUnlockInfo itemStats, bool addToShopGamePool = false, bool addToTreasureGamePool = false)
        {
            LoadedDBsHandler.ItemUnlocksDB.AddNewItem(item.name, item, itemStats, addToShopGamePool, addToTreasureGamePool, categoryID, categoryDisplayName);
        }
        /// <summary>
        /// You can add loot to cutom loot pools. But, if the pool is not created, you could also create it yourself.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="lootListID"></param>
        /// <param name="rarity"></param>
        /// <param name="addToLockedItem"></param>
        public static ExtraLootListEffect AddItemToCustomLootPool(BaseWearableSO item, string lootListID, int rarity, bool addToLockedItem = false, bool createIfDoesNotExists = false)
        {
            if (!LoadedDBsHandler.ItemPoolDB.TryGetItemLootListEffect(lootListID, out ExtraLootListEffect list))
            {
                if (!createIfDoesNotExists)
                {
                    Debug.LogError($"No Pool with ID {lootListID} you did not set the createIfDoesNotExists to true");
                    return null;
                }
                list = ScriptableObject.CreateInstance<ExtraLootListEffect>();
                list._lockedLootableItems = new List<LootItemProbability>();
                list._lootableItems = new List<LootItemProbability>();
                list._nothingPercentage = 40;
                list._shopPercentage = 2;
                list._treasurePercentage = 1;

                LoadedDBsHandler.ItemPoolDB.AddItemLootListEffect(lootListID, list);
            }

            LootItemProbability data = new LootItemProbability(item.name, rarity);
            if (addToLockedItem)
                list._lockedLootableItems.Add(data);
            else
                list._lootableItems.Add(data);

            return list;
        }
        public static void AddItemFishingRodPool(BaseWearableSO item, int rarity, bool addToLockedItem = false)
        {
            AddItemToCustomLootPool(item, PoolList_GameIDs.FishingRod.ToString(), rarity, addToLockedItem);
        }
        public static void AddItemCanOfWormsPool(BaseWearableSO item, int rarity, bool addToLockedItem = false)
        {
            AddItemToCustomLootPool(item, PoolList_GameIDs.CanOfWorms_WelsCatfish.ToString(), rarity, addToLockedItem);
        }

        #endregion
    }
}

namespace BrutalAPI.Items
{
    public abstract class BaseItem
    {
        #region ITEM PROPERTIES
        //Basic Info
        public abstract BaseWearableSO Item { get; }
        public string Item_ID
        {
            get
            {
                return Item.name;
            }
            set
            {
                Item.name = value;
            }
        }
        public string Name
        {
            set
            {
                Item._itemName = value;
            }
        }
        public string Flavour
        {
            set
            {
                Item._flavourText = value;
            }
        }
        public string Description
        {
            set
            {
                Item._description = value;
            }
        }
        public Sprite Icon
        {
            set
            {
                Item.wearableImage = value;
            }
        }
        public WearableStaticModifierSetterSO[] EquippedModifiers
        {
            get
            {
                return Item.staticModifiers;
            }
            set
            {
                Item.staticModifiers = value;
            }
        }

        //Trigger Info
        public TriggerCalls TriggerOn
        {
            set
            {
                Item.triggerOn = value;
            }
        }
        public bool DoesPopUpInfo
        {
            set
            {
                Item.doesItemPopUp = value;
            }
        }
        public EffectorConditionSO[] Conditions
        {
            get
            {
                return Item.conditions;
            }
            set
            {
                Item.conditions = value;
            }
        }
        public bool DoesActionOnTriggerAttached
        {
            set
            {
                Item.doesTriggerAttachedActionOnInitialization = value;
            }
        }

        //Consume Info
        public TriggerCalls ConsumeOnTrigger
        {
            set
            {
                Item.consumeOnTrigger = value;
            }
        }
        public bool ConsumeOnUse
        {
            set
            {
                Item.getsConsumedOnUse = value;
            }
        }
        public EffectorConditionSO[] ConsumeConditions
        {
            get
            {
                return Item.consumeConditions;
            }
            set
            {
                Item.consumeConditions = value;
            }
        }

        //Shop, Treasure, Unlock Info
        public int ShopPrice
        {
            set
            {
                Item.shopPrice = value;
            }
        }
        public bool IsShopItem
        {
            set
            {
                Item.isShopItem = value;
            }
        }
        public bool StartsLocked
        {
            set
            {
                Item.startsLocked = value;
            }
        }
        public bool OnUnlockUsesTHE
        {
            set
            {
                Item.usesTheOnUnlockText = value;
            }
        }
        public bool UsesSpecialUnlockText
        {
            set
            {
                Item.hasSpecialUnlock = value;
            }
        }
        public UILocID SpecialUnlockID
        {
            set
            {
                Item.unlockTextID = value;
            }
        }

        #endregion

        protected void InitializeItemData(string id)
        {
            Item_ID = id;
            Item.conditions = new EffectorConditionSO[0];
            Item.consumeConditions = new EffectorConditionSO[0];
            Item.staticModifiers = new WearableStaticModifierSetterSO[0];
        }

    }
    /// <summary>
    ///BarelyUsedGauze_SW, BrigadeOfDis_TW, Bronzos2Cents_SW, DivineMud_TW, FlyPaper_SW, GentlemensGlove_SW, GlueTrap_SW, Ichthys_TW, IdeaOfEvil_TW, ImmolatedFairy_TW, LadyGloves_SW, ManMadeOvum_TW, Prosthetics_SW, RibOfEve_TW, SkinnedSkate_TW, SoggyBandages_SW, SomeoneElsesFace_SW, SpikedCollar_TW, SpringTrap_SW, StrangeFruit_TW, TheBrand_TW, TheHumanSoul_TW, TheMastersSickle_SW, TondalsVision_TW, UnfortunateProphecy_TW
    /// </summary>
    public class CustomModdedClass_Item : BaseItem
    {
        public BaseWearableSO item;
        public override BaseWearableSO Item { get => item; }
        public CustomModdedClass_Item(BaseWearableSO moddedItem, string itemID = "DefaultID_Item")
        {
            item = moddedItem;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///BarelyUsedGauze_SW, BrigadeOfDis_TW, Bronzos2Cents_SW, DivineMud_TW, FlyPaper_SW, GentlemensGlove_SW, GlueTrap_SW, Ichthys_TW, IdeaOfEvil_TW, ImmolatedFairy_TW, LadyGloves_SW, ManMadeOvum_TW, Prosthetics_SW, RibOfEve_TW, SkinnedSkate_TW, SoggyBandages_SW, SomeoneElsesFace_SW, SpikedCollar_TW, SpringTrap_SW, StrangeFruit_TW, TheBrand_TW, TheHumanSoul_TW, TheMastersSickle_SW, TondalsVision_TW, UnfortunateProphecy_TW
    /// </summary>
    public class Basic_Item : BaseItem
    {
        public BasicWearable item;
        public override BaseWearableSO Item { get => item; }
        public Basic_Item(string itemID = "DefaultID_Item")
        {
            item = ScriptableObject.CreateInstance<BasicWearable>();
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///AGift_TW, Ampoule_SW, AnotherDud_SW, AsceticEgg_TW, Atabrine_SW, BalticBrine_SW, BananasCat_TW, BlackenedToad_TW, BlindFaith_TW, BloodBottle_SW, BloodBreathingBomb_TW, BoxOfMedals_SW, BrokenDoll_TW, BrokenHammer_TW, BronzosStupidHat_TW, CanOfWorms_SW, CaretakersCudgel_TW, ColonCoins_TW, ConscriptionNotice_SW, ConsolationPrize_SW, CounterfeitCoin_SW, CounterfeitMedal_SW, Cremation_TW, CrookedDagger_TW, CzechHedgehog_SW, DefacedScripture_TW, DefectiveRounds_SW, DemonCore_SW, DriedPaint_SW, DumDum_SW, EdsTags_SW, EggOfFirmament_TW, EsotericArtifact_SW, ExpiredMedicine_SW, FaultyLandMine_SW, FirstAid_SW, FishingRod_TW, ForgottenPump_SW, FunnelHelmet_TW, FunnyMushrooms_TW, GamifiedSquid_TW, GildedMirror_TW, GospelsSeveredHead_TW, GreatWhite_ExtraW, GrumpyGump_TW, GumpMingGoa_TW, Guppy_ExtraW, HarvestAndPlenty_TW, HeadOfScribe_TW, HolyChalice_TW, HomelessHotline_SW, HowlingLong_TW, IronHelmet_TW, LadyPills_SW, LilSmiley_SW, LitteringLeaflets_SW, LumpOfLamb_TW, LustPudding_TW, LycanthropesCore_TW, MeatreWorm_TW, MeatreWormEaten_ExtraW, MeatreWormSkeleton_ExtraW, MedicalLeeches_SW, MiniMordrake_TW, MommaNooty_TW, MysteryRation_SW, NorrisBad_ExtraW, OpulentEgg_TW, PainKillers_SW, PegLeg_TW, PepPowder_SW, PrussianBlue_SW, PurpleHeart_SW, RAM_TW, RecalledGasMask_SW, RoentgenRays_SW, RorschachTest_SW, RuntyRotter_TW, RusskiVampire_SW, SafeAndSound_SW, Salmon_ExtraW, SeedsOfTheConsumed_TW, SerpentsHead_TW, ShardOfNowak_TW, Skates_TW, Soap_SW, SomeonesWeddingRing_TW, StarvingApples_TW, Stigmata_TW, StillbornEgg_TW, StolenGold_SW, SulfaPowder_SW, Surrender_SW, SweetHeart_SW, TheFirstBorn_TW, TheIdealFormOfTrash_TW, TrenchFoot_SW, Trepanation_TW, UsedBandAids_SW, UsedNeedle_SW, Vowbreaker_SW, VyacheslavsLastSip_SW, WarBond_SW, WickerChild_TW, YouCanDoIt_SW
    /// </summary>
    public class PerformEffect_Item : BaseItem
    {
        public PerformEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] Effects
        {
            get
            {
                return item.effects;
            }
            set
            {
                item.effects = value;
            }
        }
        public bool IsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }

        public PerformEffect_Item(string itemID = "DefaultID_Item", EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<PerformEffectWearable>();
            item._immediateEffect = immediate;
            item.effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///ABananaPeel_ExtraW, CursedSword_TW, Enigma_TW, ExquisiteCorpse_TW, ExtremelyUnfinishedHeir_ExtraW, Norris_TW, ProfessionalProcrastinator_TW, SacrificialSaint_TW, TakePennyLeavePenny_SW, TapeWormPills_SW, ViolatedPact_TW
    /// </summary>
    public class DoublePerformEffect_Item : BaseItem
    {
        public CustomDoublePerformEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] Effects
        {
            get
            {
                return item._firstEffects;
            }
            set
            {
                item._firstEffects = value;
            }
        }
        public bool IsEffectImmediate
        {
            set
            {
                item._firstImmediateEffect = value;
            }
        }

        public TriggerCalls[] SecondaryTriggerOn
        {
            get
            {
                return item._secondPerformTriggersOn;
            }
            set
            {
                item._secondPerformTriggersOn = value;
            }
        }
        public bool SecondaryDoesPopUpInfo
        {
            set
            {
                item._secondDoesPerformItemPopUp = value;
            }
        }
        public EffectorConditionSO[] SecondaryConditions
        {
            get
            {
                return item._secondPerformConditions;
            }
            set
            {
                item._secondPerformConditions = value;
            }
        }
        public bool SecondaryConsumeOnUse
        {
            set
            {
                item._GetsConsumedOnSecondaryUse = value;
            }
        }
        public EffectInfo[] SecondaryEffects
        {
            get
            {
                return item._secondEffects;
            }
            set
            {
                item._secondEffects = value;
            }
        }
        public bool SecondaryIsEffectImmediate
        {
            set
            {
                item._secondImmediateEffect = value;
            }
        }

        public DoublePerformEffect_Item(string itemID = "DefaultID_Item", EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<CustomDoublePerformEffectWearable>();
            item._firstImmediateEffect = immediate;
            item._firstEffects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///HealthInsurance_SW, Hereafter_TW, TaintedApple_TW, TheApple_TW
    /// </summary>
    public class PerformEffectWithFalseSetter_Item : BaseItem
    {
        public PerformEffectWithFalseSetterWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] Effects
        {
            get
            {
                return item.effects;
            }
            set
            {
                item.effects = value;
            }
        }

        public PerformEffectWithFalseSetter_Item(string itemID = "DefaultID_Item", EffectInfo[] effects = null)
        {
            item = ScriptableObject.CreateInstance<PerformEffectWithFalseSetterWearable>();
            item.effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///WheelOfFortune_TW
    /// </summary>
    public class PerformEffectWithBooleanSetter_Item : BaseItem
    {
        public CustomBooleanTriggerSetterWithPerformEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public bool DataSet
        {
            set
            {
                item._dataSet = value;
            }
        }
        public bool BooleanDoesPopUp
        {
            set
            {
                item._doesBooleanItemPopUp = value;
            }
        }

        public TriggerCalls SpecialTriggerOn
        {
            set
            {
                item._performTriggerOn = value;
            }
        }
        public EffectorConditionSO[] SpecialConditions
        {
            get
            {
                return item._performConditions;
            }
            set
            {
                item._performConditions = value;
            }
        }
        public bool SpecialDoesPopUpInfo
        {
            set
            {
                item._doesPerformItemPopUp = value;
            }
        }
        public EffectInfo[] SpecialEffects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }
        public bool SpecialIsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }

        public PerformEffectWithBooleanSetter_Item(string itemID = "DefaultID_Item", bool dataSet = false, EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<CustomBooleanTriggerSetterWithPerformEffectWearable>();
            item._dataSet = dataSet;
            item._immediateEffect = immediate;
            item._effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///LilHomunculus_TW, OlStumpy_SW
    /// </summary>
    public class PerformEffectWithStatusEffectApplyBlock_Item : BaseItem
    {
        public PerformEffectWithStatusEffectApplicationFalseSetterWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] Effects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }
        public bool IsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }

        public PerformEffectWithStatusEffectApplyBlock_Item(string itemID = "DefaultID_Item", EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<PerformEffectWithStatusEffectApplicationFalseSetterWearable>();
            item.triggerOn = TriggerCalls.CanApplyStatusEffect;
            item._immediateEffect = immediate;
            item._effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///Bananas_TW, WelsCatfish_ExtraW
    /// </summary>
    public class PerformWithConsumeEffect_Item : BaseItem
    {
        public PerformEffectWithConsumeEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] CustomEffects
        {
            get
            {
                return item._customEffects;
            }
            set
            {
                item._customEffects = value;
            }
        }
        public bool CustomIsEffectImmediate
        {
            set
            {
                item._customIsImmediateEffect = value;
            }
        }
        public TriggerCalls CustomTriggerOn
        {
            set
            {
                item._customPerformTriggerOn = value;
            }
        }

        public TriggerCalls SpecialConsumeTriggerOn
        {
            set
            {
                item._specialConsumptionTriggerOn = value;
            }
        }
        public bool SpecialConsumeDoesPopUpInfo
        {
            set
            {
                item._specialConsumptionDoesPerformItemPopUp = value;
            }
        }
        public EffectInfo[] SpecialConsumeEffects
        {
            get
            {
                return item._consumptionEffects;
            }
            set
            {
                item._consumptionEffects = value;
            }
        }
        public bool SpecialConsumeIsEffectImmediate
        {
            set
            {
                item._specialConsumptionImmediateEffect = value;
            }
        }

        public PerformWithConsumeEffect_Item(string itemID = "DefaultID_Item", TriggerCalls triggerOn = TriggerCalls.Count, EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<PerformEffectWithConsumeEffectWearable>();
            item._customPerformTriggerOn = triggerOn;
            item._customIsImmediateEffect = immediate;
            item._customEffects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///RotundAmphibian_TW
    /// </summary>
    public class PerformEffectWithDamageMultiplierModifier_Item : BaseItem
    {
        public PerformEffectWithMultiplierDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] Effects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }
        public bool IsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }
        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public bool UseSimpleIntegerInsteadOfDamage
        {
            set
            {
                item._useSimpleInt = value;
            }
        }
        public int Multiplier
        {
            set
            {
                item._toMultiply = value;
            }
        }

        public PerformEffectWithDamageMultiplierModifier_Item(string itemID = "DefaultID_Item",
            int multiplier = 1, bool useDealt = false, bool useInt = false,
            EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<PerformEffectWithMultiplierDamageModifierSetterWearable>();
            item._toMultiply = multiplier;
            item._useDealt = useDealt;
            item._useSimpleInt = useInt;
            item._immediateEffect = immediate;
            item._effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///SculpturesTools_SW
    /// </summary>
    public class PerformEffectWithDamagePercentageModifier_Item : BaseItem
    {
        public CustomPercDmgModAndPerformEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public bool UseSimpleIntegerInsteadOfDamage
        {
            set
            {
                item._useSimpleInt = value;
            }
        }
        public bool DoesIncreaseDamage
        {
            set
            {
                item._doesIncrease = value;
            }
        }
        public int PercentageToModify
        {
            set
            {
                item._percentageToModify = value;
            }
        }

        public TriggerCalls SpecialTriggerOn
        {
            set
            {
                item._performTriggerOn = value;
            }
        }
        public bool SpecialDoesPopUpInfo
        {
            set
            {
                item._doesPerformItemPopUp = value;
            }
        }
        public bool SpecialIsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }
        public EffectorConditionSO[] SpecialConditions
        {
            get
            {
                return item._performConditions;
            }
            set
            {
                item._performConditions = value;
            }
        }
        public EffectInfo[] SpecialEffects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }

        public PerformEffectWithDamagePercentageModifier_Item(string itemID = "DefaultID_Item", 
            int percentage = 1, bool useDealt = false, bool useInt = false, bool doesIncreaseDmg = false)
        {
            item = ScriptableObject.CreateInstance<CustomPercDmgModAndPerformEffectWearable>();
            item._percentageToModify = percentage;
            item._useDealt = useDealt;
            item._useSimpleInt = useInt;
            item._doesIncrease = doesIncreaseDmg;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///DDT_SW
    /// </summary>
    public class PerformEffectWithCanHealSetter_Item : BaseItem
    {
        public CustomCanHealSetterWithPerformEffectWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool DataSet
        {
            set
            {
                item._dataSet = value;
            }
        }
        public TriggerCalls CustomTriggerOn
        {
            set
            {
                item._performTriggerOn = value;
            }
        }
        public EffectorConditionSO[] CustomConditions
        {
            get
            {
                return item._performConditions;
            }
            set
            {
                item._performConditions = value;
            }
        }
        public bool CustomDoesPopUpInfo
        {
            set
            {
                item._doesPerformItemPopUp = value;
            }
        }
        public bool CustomIsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }
        public EffectInfo[] CustomEffects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }



        public PerformEffectWithCanHealSetter_Item(string itemID = "DefaultID_Item", EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<CustomCanHealSetterWithPerformEffectWearable>();
            item._immediateEffect = immediate;
            item._effects = effects;
            item.triggerOn = TriggerCalls.CanHeal;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///UsedDogTags_SW
    /// </summary>
    public class PerformEffectAndCanHealSetter_Item : BaseItem
    {
        public CustomCanHealSetterAndPerformEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public bool DataSet
        {
            set
            {
                item._dataSet = value;
            }
        }
        public bool IsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }
        public EffectInfo[] Effects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }

        public PerformEffectAndCanHealSetter_Item(string itemID = "DefaultID_Item", bool dataSet = false, EffectInfo[] effects = null, bool immediate = false)
        {
            item = ScriptableObject.CreateInstance<CustomCanHealSetterAndPerformEffectWearable>();
            item._dataSet = dataSet;
            item._immediateEffect = immediate;
            item._effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///TheRestOfNowak_TW
    /// </summary>
    public class PerformEffectAddingResult_Item : BaseItem
    {
        public PerformEffectAddingResultWearable item;
        public override BaseWearableSO Item { get => item; }
        public bool UseIntReferenceResult
        {
            set
            {
                item._useIntReferenceResult = value;
            }
        }
        public bool IsEffectImmediate
        {
            set
            {
                item._immediateEffect = value;
            }
        }
        public EffectInfo[] Effects
        {
            get
            {
                return item._effects;
            }
            set
            {
                item._effects = value;
            }
        }

        public PerformEffectAddingResult_Item(string itemID = "DefaultID_Item", EffectInfo[] effects = null, bool immediate = false, bool useIntRefResult = false)
        {
            item = ScriptableObject.CreateInstance<PerformEffectAddingResultWearable>();
            item._useIntReferenceResult = useIntRefResult;
            item._immediateEffect = immediate;
            item._effects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///InfernalEye_TW
    /// </summary>
    public class PerformEffectOnAttach_Item : BaseItem
    {
        public PerformEffectOnAttachWearable item;
        public override BaseWearableSO Item { get => item; }
        public EffectInfo[] AttachEffects
        {
            get
            {
                return item.attachEffects;
            }
            set
            {
                item.attachEffects = value;
            }
        }
        public EffectInfo[] DettachEffects
        {
            get
            {
                return item.dettachEffects;
            }
            set
            {
                item.dettachEffects = value;
            }
        }

        public PerformEffectOnAttach_Item(string itemID = "DefaultID_Item", EffectInfo[] attachEffects = null, EffectInfo[] dettachEffects = null)
        {
            item = ScriptableObject.CreateInstance<PerformEffectOnAttachWearable>();
            item.attachEffects = attachEffects;
            item.dettachEffects = dettachEffects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///BlueBible_TW, Indulgence_TW, PontiffsParade_TW
    /// </summary>
    public class BooleanSetter_Item : BaseItem
    {
        public BooleanSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool DataSet
        {
            set
            {
                item._dataSet = value;
            }
        }


        public BooleanSetter_Item(string itemID = "DefaultID_Item", bool dataSet = false)
        {
            item = ScriptableObject.CreateInstance<BooleanSetterWearable>();
            item._dataSet = dataSet;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///AllThatIsMortal_TW, CertificateOfExemption_SW, LuckyHelmet_SW, RightShoe_ExtraW
    /// </summary>
    public class DamageMultiplierModifier_Item : BaseItem
    {
        public MultiplierDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public bool UseSimpleIntegerInsteadOfDamage
        {
            set
            {
                item._useSimpleInt = value;
            }
        }
        public int Multiplier
        {
            set
            {
                item._toMultiply = value;
            }
        }

        public DamageMultiplierModifier_Item(string itemID = "DefaultID_Item", int multiplier = 1, bool useDealt = false, bool useInt = false)
        {
            item = ScriptableObject.CreateInstance<MultiplierDamageModifierSetterWearable>();
            item._toMultiply = multiplier;
            item._useDealt = useDealt;
            item._useSimpleInt = useInt;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///GamblersRightHand_TW
    /// </summary>
    public class DamageMultiplierByPercentageModifier_Item : BaseItem
    {
        public PercentageMultiplierDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public int FirstMultiplier
        {
            set
            {
                item._firstToMultiply = value;
            }
        }
        public int SecondMultiplier
        {
            set
            {
                item._secondToMultiply = value;
            }
        }
        public int PercentageToMultiplyFirstOrSecond
        {
            set
            {
                item._percentageToMultiply = value;
            }
        }

        public DamageMultiplierByPercentageModifier_Item(string itemID = "DefaultID_Item", int firstMult = 1, int secondMult = 1, int percentage = 1, bool useDealt = false)
        {
            item = ScriptableObject.CreateInstance<PercentageMultiplierDamageModifierSetterWearable>();
            item._firstToMultiply = firstMult;
            item._secondToMultiply = secondMult;
            item._percentageToMultiply = percentage;
            item._useDealt = useDealt;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///ModernMedicine_SW
    /// </summary>
    public class DamageAdditionModifier_Item : BaseItem
    {
        public AdditionDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public int ToAdd
        {
            set
            {
                item._toAdd = value;
            }
        }

        public DamageAdditionModifier_Item(string itemID = "DefaultID_Item", int toAdd = 1, bool useDealt = false)
        {
            item = ScriptableObject.CreateInstance<AdditionDamageModifierSetterWearable>();
            item._useDealt = useDealt;
            item._toAdd = toAdd;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///OlReliable_SW
    /// </summary>
    public class DamageMaximizationModifier_Item : BaseItem
    {
        public MaximizationDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public int ToMaximize
        {
            set
            {
                item._toMaximize = value;
            }
        }

        public DamageMaximizationModifier_Item(string itemID = "DefaultID_Item", int toMaximize = 1, bool useDealt = false)
        {
            item = ScriptableObject.CreateInstance<MaximizationDamageModifierSetterWearable>();
            item._useDealt = useDealt;
            item._toMaximize = toMaximize;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///ChainofCommand_SW, DewormingPills_SW, ExtraStitching_SW, LeftShoe_ExtraW, LittleKnife_TW
    /// </summary>
    public class DamagePercentageModifier_Item : BaseItem
    {
        public PercentageDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public bool UseSimpleIntegerInsteadOfDamage
        {
            set
            {
                item._useSimpleInt = value;
            }
        }
        public bool DoesIncreaseDamage
        {
            set
            {
                item._doesIncrease = value;
            }
        }
        public int PercentageToModify
        {
            set
            {
                item._percentageToModify = value;
            }
        }

        public DamagePercentageModifier_Item(string itemID = "DefaultID_Item", int percentage = 1, bool useDealt = false, bool useInt = false, bool doesIncreaseDmg = false)
        {
            item = ScriptableObject.CreateInstance<PercentageDamageModifierSetterWearable>();
            item._percentageToModify = percentage;
            item._useDealt = useDealt;
            item._useSimpleInt = useInt;
            item._doesIncrease = doesIncreaseDmg;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///PharmaceuticalRollerCoaster_SW
    /// </summary>
    public class DamageDealtMultiPercentageModifier_Item : BaseItem
    {
        public PercentageDamageModifierVariousOptionsTypeSetterWearable item;
        public override BaseWearableSO Item { get => item; }
        public PercOption[] PercentageData
        {
            get
            {
                return item._percData;
            }
            set
            {
                item._percData = value;
            }
        }

        public DamageDealtMultiPercentageModifier_Item(string itemID = "DefaultID_Item", PercOption[] percData = null)
        {
            item = ScriptableObject.CreateInstance<PercentageDamageModifierVariousOptionsTypeSetterWearable>();
            item._percData = percData;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///LilOrro_TW
    /// </summary>
    public class DamageDealtPercentageModifierByUnitType_Item : BaseItem
    {
        public PercentageDamageModifierByUnitTypeSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool DefaultDoesIncreaseDamage
        {
            set
            {
                item._defaultDoesIncrease = value;
            }
        }
        public int DefaultPercentageToModify
        {
            set
            {
                item._defaultPercentageToModify = value;
            }
        }
        public UnitTypePercMod[] UnitTypeData
        {
            get
            {
                return item._unitTypeData;
            }
            set
            {
                item._unitTypeData = value;
            }
        }

        public DamageDealtPercentageModifierByUnitType_Item(string itemID = "DefaultID_Item", int percentage = 50, bool doesIncreaseDmg = true, UnitTypePercMod[] unitData = null)
        {
            item = ScriptableObject.CreateInstance<PercentageDamageModifierByUnitTypeSetterWearable>();
            item._defaultDoesIncrease = doesIncreaseDmg;
            item._defaultPercentageToModify = percentage;
            item._unitTypeData = (unitData == null) ? new UnitTypePercMod[0] : unitData;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///FistFullOfAsh_TW
    /// </summary>
    public class DamageReceivedPercentageModifier_Item : BaseItem
    {
        public PercDmgModifierSetterByReceivedDamageTypeWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool DoesIncreaseDirectDamage
        {
            set
            {
                item._doesIncreaseDirect = value;
            }
        }
        public int DirectDmgPercentageToModify
        {
            set
            {
                item._percentageToModifyDirect = value;
            }
        }
        public bool DoesIncreaseIndirectDamage
        {
            set
            {
                item._doesIncreaseIndirect = value;
            }
        }
        public int IndirectDmgPercentageToModify
        {
            set
            {
                item._percentageToModifyIndirect = value;
            }
        }

        public DamageReceivedPercentageModifier_Item(string itemID = "DefaultID_Item", int directPerc = 50, bool doesIncreaseDirectDmg = false, int indirectPerc = 50, bool doesIncreaseIndirectDmg = false)
        {
            item = ScriptableObject.CreateInstance<PercDmgModifierSetterByReceivedDamageTypeWearable>();
            item._percentageToModifyDirect = directPerc;
            item._doesIncreaseDirect = doesIncreaseDirectDmg;
            item._percentageToModifyIndirect = indirectPerc;
            item._doesIncreaseIndirect = doesIncreaseIndirectDmg;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///EggOfIncubus_TW, EggOfIncubusCracked_ExtraW
    /// </summary>
    public class DamageReceivedPercentageModifierWithConsumeEffect_Item : BaseItem
    {
        public PercDmgModByReceivedWithConsEffWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool DoesIncreaseDirectDamage
        {
            set
            {
                item._doesIncreaseDirect = value;
            }
        }
        public int DirectDmgPercentageToModify
        {
            set
            {
                item._percentageToModifyDirect = value;
            }
        }
        public bool DoesIncreaseIndirectDamage
        {
            set
            {
                item._doesIncreaseIndirect = value;
            }
        }
        public int IndirectDmgPercentageToModify
        {
            set
            {
                item._percentageToModifyIndirect = value;
            }
        }

        public TriggerCalls SpecialConsumeTriggerOn
        {
            set
            {
                item._specialConsumptionTriggerOn = value;
            }
        }
        public bool SpecialConsumeDoesPopUpInfo
        {
            set
            {
                item._specialConsumptionDoesPerformItemPopUp = value;
            }
        }
        public bool SpecialConsumeIsEffectImmediate
        {
            set
            {
                item._specialConsumptionImmediateEffect = value;
            }
        }
        public EffectorConditionSO[] SpecialConsumeConditions
        {
            get
            {
                return item._specialConsumeConditions;
            }
            set
            {
                item._specialConsumeConditions = value;
            }
        }
        public EffectInfo[] SpecialConsumeEffects
        {
            get
            {
                return item._consumptionEffects;
            }
            set
            {
                item._consumptionEffects = value;
            }
        }


        public DamageReceivedPercentageModifierWithConsumeEffect_Item(string itemID = "DefaultID_Item", int directPerc = 50, bool doesIncreaseDirectDmg = false, int indirectPerc = 50, bool doesIncreaseIndirectDmg = false)
        {
            item = ScriptableObject.CreateInstance<PercDmgModByReceivedWithConsEffWearable>();
            item._percentageToModifyDirect = directPerc;
            item._doesIncreaseDirect = doesIncreaseDirectDmg;
            item._percentageToModifyIndirect = indirectPerc;
            item._doesIncreaseIndirect = doesIncreaseIndirectDmg;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///ConvergentRage_TW
    /// </summary>
    public class DamageMaxHealthModifier_Item : BaseItem
    {
        public MaxHealthDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }
        public DamageMaxHealthModifier_Item(string itemID = "DefaultID_Item")
        {
            item = ScriptableObject.CreateInstance<MaxHealthDamageModifierSetterWearable>();
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///CrookedDie_TW
    /// </summary>
    public class DamageSubstractionModifier_Item : BaseItem
    {
        public SubstractionDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public int ToDecrease
        {
            set
            {
                item._toDecrease = value;
            }
        }
        public bool UseRandomFromList
        {
            set
            {
                item._useRandomFromList = value;
            }
        }
        public int[] RansomListToDecrease
        {
            get
            {
                return item._possiblesToDecrease;
            }
            set
            {
                item._possiblesToDecrease = value;
            }
        }

        public DamageSubstractionModifier_Item(string itemID = "DefaultID_Item", int toDecrease = 1, bool useDealt = false)
        {
            item = ScriptableObject.CreateInstance<SubstractionDamageModifierSetterWearable>();
            item._toDecrease = toDecrease;
            item._useDealt = useDealt;
            item._useRandomFromList = false;
            item._possiblesToDecrease = new int[0];
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///BloatingCoffers_TW
    /// </summary>
    public class MoneyShieldModifier_Item : BaseItem
    {
        public CurrencyShieldDamageModifierSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public EffectInfo[] AttachEffects
        {
            get
            {
                return item.m_AttachEffects;
            }
            set
            {
                item.m_AttachEffects = value;
            }
        }


        public MoneyShieldModifier_Item(string itemID = "DefaultID_Item", bool useDealt = false, EffectInfo[] effects = null)
        {
            item = ScriptableObject.CreateInstance<CurrencyShieldDamageModifierSetterWearable>();
            item._useDealt = useDealt;
            item.m_AttachEffects = effects;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///EffigyOfTheMettleMother_TW
    /// </summary>
    public class StatusEffectsReductionBlock_Item : BaseItem
    {
        public StatusEffectsReductionBlockWearable item;
        public override BaseWearableSO Item { get => item; }

        public StatusEffectsReductionBlock_Item(string itemID = "DefaultID_Item")
        {
            item = ScriptableObject.CreateInstance<StatusEffectsReductionBlockWearable>();
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///IronNecklace_SW, PlasterCast_SW, SplatterMask_SW
    /// </summary>
    public class StatusEffectApplyBlock_Item : BaseItem
    {
        public StatusEffectApplicationFalseSetterWearable item;
        public override BaseWearableSO Item { get => item; }

        public StatusEffectApplyBlock_Item(string itemID = "DefaultID_Item")
        {
            item = ScriptableObject.CreateInstance<StatusEffectApplicationFalseSetterWearable>();
            item.triggerOn = TriggerCalls.CanApplyStatusEffect;
            InitializeItemData(itemID);
        }
    }
    /// <summary>
    ///TheAsymptote_ExtraW, TheInfiniteJersey_ExtraW, TheJersey_TW
    /// </summary>
    public class TheJersey_Item : BaseItem
    {
        public JerseyEffectWearable item;
        public override BaseWearableSO Item { get => item; }
        public bool AffectDamageDealtInsteadOfReceived
        {
            set
            {
                item._useDealt = value;
            }
        }
        public bool UseSimpleInt
        {
            set
            {
                item._useSimpleInt = value;
            }
        }
        public int ToMaximize
        {
            set
            {
                item._toMaximize = value;
            }
        }

        public TriggerCalls SecondaryTriggerOn
        {
            set
            {
                item._secondPerformTriggerOn = value;
            }
        }
        public bool SecondaryDoesPopUpInfo
        {
            set
            {
                item._secondDoesPerformItemPopUp = value;
            }
        }
        public EffectInfo[] SecondaryEffects
        {
            get
            {
                return item._secondEffects;
            }
            set
            {
                item._secondEffects = value;
            }
        }
        public bool SecondaryIsEffectImmediate
        {
            set
            {
                item._secondImmediateEffect = value;
            }
        }

        public TriggerCalls ThirdTriggerOn
        {
            set
            {
                item._thirdPerformTriggerOn = value;
            }
        }
        public bool ThirdDoesPopUpInfo
        {
            set
            {
                item._thirdDoesPerformItemPopUp = value;
            }
        }
        public bool ThirdConsumedDoesPopUpInfo
        {
            set
            {
                item._thirdConsumedDoesPerformItemPopUp = value;
            }
        }
        public bool ThirdConsumeOnTrigger
        {
            set
            {
                item._thirdConsumeOnTrigger = value;
            }
        }
        public EffectorConditionSO[] ThirdConsumeConditions
        {
            get
            {
                return item._thirdConsumeConditions;
            }
            set
            {
                item._thirdConsumeConditions = value;
            }
        }

        public bool ThirdIsEffectImmediate
        {
            set
            {
                item._thirdImmediateEffect = value;
            }
        }
        public EffectInfo[] ThirdEffects
        {
            get
            {
                return item._thirdEffects;
            }
            set
            {
                item._thirdEffects = value;
            }
        }
        public EffectInfo[] ThirdConsumeEffects
        {
            get
            {
                return item._thirdConsumedEffects;
            }
            set
            {
                item._thirdConsumedEffects = value;
            }
        }

        public TheJersey_Item(string itemID = "DefaultID_Item")
        {
            item = ScriptableObject.CreateInstance<JerseyEffectWearable>();
            InitializeItemData(itemID);
        }
    }
}
