using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    static public class Dialogues
    {
        static public void AddCustom_GameOver_BossLines(string bossID, string[] lines)
        {
            LoadedDBsHandler.ExtraDialogueDB.AddBossLinesData(bossID, lines);
        }

        static public void AddCustom_GameOver_RandomLines(string[] lines)
        {
            LoadedDBsHandler.ExtraDialogueDB.AddGameOverLinesData(lines);
        }

        static public void AddCustom_ExtraDialogue_Lines(string extraID, string[] lines)
        {
            LoadedDBsHandler.ExtraDialogueDB.AddExtraLinesData(extraID, lines);
        }

        #region DialogueDB
        static public void AddCustom_DialogueProgram(string programID, YarnProgram data)
        {

            LoadedDBsHandler.DialogueDB.AddNewDialogueProgram(programID, data);
        }
        

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the DialogueSO but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public DialogueSO CreateAndAddCustom_DialogueSO(string dialogueID, YarnProgram defaultProgram, string programID, string startingNode)
        {
            DialogueSO data = ScriptableObject.CreateInstance<DialogueSO>();
            data.name = dialogueID;
            data.dialog = defaultProgram;
            data.m_DialogID = programID;
            data.startNode = startingNode;

            LoadedDBsHandler.DialogueDB.AddNewDialogueSO(dialogueID, data);
            return data;
        }

        static public void AddCustom_DialogueSO(string dialogueID, DialogueSO data)
        {
            LoadedDBsHandler.DialogueDB.AddNewDialogueSO(dialogueID, data);
        }

        /// <summary>
        /// Be careful, if the ID is already in use, it will create the SpeakerData but not add it to the Pool!
        /// </summary>
        /// <returns></returns>
        static public SpeakerData CreateAndAddCustom_SpeakerData(string speakerID, SpeakerBundle bundleData, bool portraitLooksLeft, bool portraitLooksCenter = false, SpeakerEmote[] emotes = null)
        {
            SpeakerData data = ScriptableObject.CreateInstance<SpeakerData>();
            data.speakerName = speakerID;

            if (!speakerID.EndsWith(Tools.PathUtils.speakerDataSuffix))
                speakerID = speakerID + Tools.PathUtils.speakerDataSuffix;
            data.name = speakerID;

            data._defaultBundle = bundleData;
            data.portraitLooksLeft = portraitLooksLeft;
            data.portraitLooksCenter = portraitLooksCenter;
            data._emotionBundles = (emotes == null) ? new SpeakerEmote[0] : emotes;

            LoadedDBsHandler.DialogueDB.AddNewSpeakerData(data.name, data);
            return data;
        }

        static public void AddCustom_SpeakerData(string speakerID, SpeakerData data)
        {
            if(!speakerID.EndsWith(Tools.PathUtils.speakerDataSuffix))
                speakerID = speakerID + Tools.PathUtils.speakerDataSuffix;

            data.name = speakerID;
            LoadedDBsHandler.DialogueDB.AddNewSpeakerData(data.name, data);
        }

        static public void AddCustom_OWDialogueFunction(string functionName, int parameterCount, Yarn.ReturningFunction implementation)
        {
            DialogueFunctionData data = new DialogueFunctionData(functionName, parameterCount, implementation);
            LoadedDBsHandler.DialogueDB.AddNewOWDialogueFunction(functionName, data);
        }
        static public void AddCustom_CombatDialogueFunction(string functionName, int parameterCount, Yarn.ReturningFunction implementation)
        {
            DialogueFunctionData data = new DialogueFunctionData(functionName, parameterCount, implementation);
            LoadedDBsHandler.DialogueDB.AddNewCombatDialogueFunction(functionName, data);
        }

        static public void AddCustom_OWDialogueCommand(string functionName, DialogueRunner_BO.CommandHandler handler)
        {
            DialogueFunctionData data = new DialogueFunctionData(functionName, handler);
            LoadedDBsHandler.DialogueDB.AddNewOWDialogueCommand(functionName, data);
        }
        static public void AddCustom_CombatDialogueCommand(string functionName, DialogueRunner_BO.CommandHandler handler)
        {
            DialogueFunctionData data = new DialogueFunctionData(functionName, handler);
            LoadedDBsHandler.DialogueDB.AddNewCombatDialogueCommand(functionName, data);
        }
        #endregion


    }
}
