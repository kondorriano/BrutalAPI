using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public static class Glossary
    {
        static public void CreateAndAddCustom_PassiveToGlossary(string passiveName, string passiveDescription, Sprite sprite)
        {
            GlossaryPassives data = new(passiveName, passiveDescription, sprite);
            LoadedDBsHandler.GlossaryDB.AddNewPassive(data);
        }

        static public void CreateAndAddCustom_KeywordToGlossary(string keyword, string description)
        {
            GlossaryKeywords data = new(keyword, description);
            LoadedDBsHandler.GlossaryDB.AddNewKeyword(data);
        }

    }
}
