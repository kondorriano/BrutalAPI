using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class Effects
    {
        static public EffectInfo GenerateEffect(EffectSO effect, int entryVariable = 0, BaseCombatTargettingSO target = null, EffectConditionSO condition = null)
        {
            EffectInfo info = new EffectInfo();
            info.effect = effect;
            info.entryVariable = entryVariable;
            info.targets = target;
            info.condition = condition;
            return info;
        }

        static public EffectInfo CopyEffect(EffectInfo data)
        {
            EffectInfo info = new EffectInfo();
            info.effect = data.effect;
            info.entryVariable = data.entryVariable;
            info.targets = data.targets;
            info.condition = data.condition;
            return info;
        }

        #region CONDITIONS
        /// <summary>
        /// <paramref name="chance"/> Needs to be between 1 and 99. 
        /// 0 and 100 never and always trigger.
        /// </summary>
        static public PercentageEffectCondition ChanceCondition(int chance)
        {
            PercentageEffectCondition cond = ScriptableObject.CreateInstance<PercentageEffectCondition>();
            cond.percentage = chance;
            return cond;
        }
        /// <summary>
        /// This condition is dependant on the enemies in the field.
        /// If you set up a chance of 5 and there are 5 enemies, the total chance would be 25%
        /// <paramref name="chance"/> Needs to be between 1 and 99. 
        /// </summary>
        static public PercentageByEnemyAmountEffectCondition ChancePerEnemyCondition(int chance)
        {
            PercentageByEnemyAmountEffectCondition cond = ScriptableObject.CreateInstance<PercentageByEnemyAmountEffectCondition>();
            cond.percentagePerEnemy = chance;
            return cond;
        }

        /// <summary>
        /// Checks if an Effect that already happend was successful or Not and trigger depending on that.
        /// This condition will be true depending on the previous effect result. Use <paramref name="previousWasSuccessful"/> to decide if it triggers on Success or Fail. 
        /// <paramref name="previousLocationDistance"/> is where the effect is located on the EffectInfo[]. If you want to check an effect that happened right before this one, set this to 1. 
        /// An effect that happened 3 effects ago would be a 3.
        /// </summary>
        static public PreviousEffectCondition CheckPreviousEffectCondition(bool previousWasSuccessful, int previousLocationDistance)
        {
            PreviousEffectCondition cond = ScriptableObject.CreateInstance<PreviousEffectCondition>();
            cond.wasSuccessful = previousWasSuccessful;
            cond.previousAmount = previousLocationDistance;
            return cond;
        }
        /// <summary>
        /// Complex version of PreviousEffectCondition. 
        /// It requires multiple effects to meet the proper <paramref name="previousWasSuccessful"/> to trigger.
        /// </summary>
        static public MultiPreviousEffectCondition CheckMultiplePreviousEffectsCondition(bool[] previousWasSuccessful, int[] previousLocationDistance)
        {
            MultiPreviousEffectCondition cond = ScriptableObject.CreateInstance<MultiPreviousEffectCondition>();
            cond.wasSuccessful = previousWasSuccessful;
            cond.previousAmount = previousLocationDistance;
            return cond;
        }
        /// <summary>
        /// Checks if the caster has an item or not. 
        /// </summary>
        static public HasItemEffectCondition CheckCasterHasItemCondition(bool trueIfHasItem)
        {
            HasItemEffectCondition cond = ScriptableObject.CreateInstance<HasItemEffectCondition>();
            cond.trueIfHasItem = trueIfHasItem;
            return cond;
        }
        /// <summary>
        /// Checks if the Player has coins or not.
        /// </summary>
        static public HasCurrencyEffectCondition CheckPlayerHasCurrencyCondition(bool trueIfHasCurrency)
        {
            HasCurrencyEffectCondition cond = ScriptableObject.CreateInstance<HasCurrencyEffectCondition>();
            cond._passIfThereIsCurrency = trueIfHasCurrency;
            return cond;
        }
        /// <summary>
        /// Directly checks data stored in the Game File. 
        /// You can choose if the condition is met if it has the data as true or not
        /// </summary>
        static public GameBoolDataEffectCondition CheckGameBoolDataCondition(bool passIfTrue, string dataID)
        {
            GameBoolDataEffectCondition cond = ScriptableObject.CreateInstance<GameBoolDataEffectCondition>();
            cond._PassIfTrue = passIfTrue;
            cond._VariableName = dataID;
            return cond;
        }
        /// <summary>
        /// Condition is met dependant on the Computer's Time.
        /// You can make it trigger on Hour and Minute. Together or separate.
        /// </summary>
        static public DateCheckEffectCondition CheckDateCondition(bool usesHour, int hour, bool usesMinute, int minute)
        {
            DateCheckEffectCondition cond = ScriptableObject.CreateInstance<DateCheckEffectCondition>();
            cond._usesHourCheck = usesHour;
            cond._usesMinuteCheck = usesMinute;
            cond._hourCheck = hour;
            cond._minuteCheck = minute;
            return cond;
        }
        #endregion
    }
}
