using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class Tutorials
    {
        static public void CreateAndAddCustom_StatusTutorial(StatusEffect_SO status, string dialogueID, string characterStartNode, string enemyStartNode = "")
        {
            StatusEffectCheckTTA data = ScriptableObject.CreateInstance<StatusEffectCheckTTA>();
            data.m_IsCombatInsteadOfOW = true;
            data.m_TutorialID = status.StatusID;
            data._triggerCall = Tutorial_TriggerCalls.OnStatusEffectApplied;
            data._tutorialDialog = null;
            data._DialogID = dialogueID;
            data._startNode = characterStartNode;
            data.m_EnemyStartNode = (enemyStartNode == "") ? characterStartNode : enemyStartNode;
            data.m_StatusCheck = status;

            LoadedDBsHandler.TutorialsDB.AddNewCombatTutorial(data);
        }

        static public void CreateAndAddCustom_FieldTutorial(FieldEffect_SO field, string dialogueID, string characterStartNode, string enemyStartNode = "")
        {
            FieldEffectCheckTTA data = ScriptableObject.CreateInstance<FieldEffectCheckTTA>();
            data.m_IsCombatInsteadOfOW = true;
            data.m_TutorialID = field.FieldID;
            data._triggerCall = Tutorial_TriggerCalls.OnFieldEffectApplied;
            data._tutorialDialog = null;
            data._DialogID = dialogueID;
            data._startNode = characterStartNode;
            data.m_EnemyStartNode = (enemyStartNode == "") ? characterStartNode : enemyStartNode;
            data.m_FieldCheck = field;

            LoadedDBsHandler.TutorialsDB.AddNewCombatTutorial(data);
        }

        static public void AddCustom_Tutorial(TutorialTriggerAction tutorial)
        {
            LoadedDBsHandler.TutorialsDB.AddNewCombatTutorial(tutorial);
        }
    }
}
