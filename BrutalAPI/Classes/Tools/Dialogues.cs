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
            LoadedDBsHandler.GameOverDialogueDB.AddBossLinesData(bossID, lines);
        }

        static public void AddCustom_GameOver_RandomLines(string[] lines)
        {
            LoadedDBsHandler.GameOverDialogueDB.AddRandomLinesData(lines);
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
            data.name = speakerID;
            data.speakerName = speakerID;
            data._defaultBundle = bundleData;
            data.portraitLooksLeft = portraitLooksLeft;
            data.portraitLooksCenter = portraitLooksCenter;
            data._emotionBundles = emotes;

            LoadedDBsHandler.DialogueDB.AddNewSpeakerData(speakerID, data);
            return data;
        }

        static public void AddCustom_SpeakerData(string speakerID, SpeakerData data)
        {
            LoadedDBsHandler.DialogueDB.AddNewSpeakerData(speakerID, data);
        }
        #endregion


    }
}
