using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public class Dialogues
    {
        public static void AddCustom_GameOver_BossLines(string bossID, string[] lines)
        {
            LoadedDBsHandler.GameOverDialogueDB.AddBossLinesData(bossID, lines);
        }
    }
}
