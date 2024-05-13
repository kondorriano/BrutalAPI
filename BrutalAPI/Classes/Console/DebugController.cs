using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class DebugController : MonoBehaviour
    {
        const BindingFlags AllFlags = (BindingFlags)(-1);
        public static DebugController _instance;

        string output;

        List<DebugCommand> commandList;
        public static DebugCommand HELP;
        public static DebugCommand GIVE;
        public static DebugCommand ADDFOOL;
        public static DebugCommand SPAWN;
        public static DebugCommand DAMAGE;
        public static DebugCommand HEAL;
        public static DebugCommand LEVELUP;
        public static DebugCommand MONEY;
        public static DebugCommand TP;
        public static DebugCommand ENDCOMBAT;
        public static DebugCommand PIGMENT;

        public static DebugController Instance
        {
            get
            {
                if (DebugController._instance == null)
                {
                    Debug.Log("WHAT");
                    DebugController._instance = new GameObject("DebugController").AddComponent<DebugController>();
                }
                return DebugController._instance;
            }
        }

        public void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            IDetour OpenConsoleHook = new Hook(
                    typeof(Keyboard).GetMethod("OnTextInput", AllFlags),
                    typeof(DebugController).GetMethod("KeyPressed", AllFlags));

            GameInformationHolder info = LoadedDBsHandler.InfoHolder;

            //Help command
            HELP = new DebugCommand("help", "Shows available commands and information about them.", "help (command)", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                {
                    string commands = "";
                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if (i == commandList.Count - 1)
                            commands += commandList[i].commandId;
                        else
                            commands += commandList[i].commandId + ", ";
                    }

                    WriteLine("Available commands: " + commands);
                    return;
                }
                else if (parameters != null && parameters.Length == 1)
                {
                    for (int i = 0; i < commandList.Count; i++)
                    {
                        if (commandList[i].commandId == parameters[0])
                        {
                            WriteLine(commandList[i].commandDescription + " Usage: " + commandList[i].commandFormat);
                            return;
                        }
                    }
                    WriteLine("Unknown command");
                    return;
                }
                else
                { WriteLine("Syntax error: " + HELP.commandFormat); return; }
            });

            //Give command
            GIVE = new DebugCommand("give", "Adds an item to your inventory.", "give <item_id>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                { WriteLine("Syntax error: " + GIVE.commandFormat); return; }

                if (!LoadedAssetsHandler.LoadedWearables.ContainsKey(parameters[0]))
                { WriteLine("Unknown Item"); return; }

                info.Run.playerData.AddNewItem(LoadedAssetsHandler.GetWearable(parameters[0]));
                { WriteLine("Added item " + parameters[0] + " to your inventory"); return; }
            });

            //AddFool command
            ADDFOOL = new DebugCommand("addfool", "Adds a fool to your party.", "addfool <fool_id> <rank> <abilityOne> <abilityTwo>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 4)
                { WriteLine("Syntax error: " + ADDFOOL.commandFormat); return; }

                if (!LoadedAssetsHandler.LoadedCharacters.ContainsKey(parameters[0]))
                { WriteLine("Unknown Character"); return; }

                int rank;
                int ability1;
                int ability2;

                if (!int.TryParse(parameters[1], out rank))
                { WriteLine("Rank is not a number"); return; }

                if (!int.TryParse(parameters[2], out ability1))
                { WriteLine("Ability 1 is not a number"); return; }

                if (!int.TryParse(parameters[3], out ability2))
                { WriteLine("Ability 2 is not a number"); return; }

                if (rank > 3)
                { WriteLine("A party member can't have a rank above 3"); return; }

                info.Run.playerData.AddNewCharacter(LoadedAssetsHandler.GetCharacter(parameters[0]), rank, ability1, ability2);
                { WriteLine("Added fool " + parameters[0] + " to your party"); return; }
            });

            //Spawn command
            SPAWN = new DebugCommand("spawn", "Spawns an enemy in battle.", "spawn <enemy_id> <slot>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 2)
                { WriteLine("Syntax error: " + SPAWN.commandFormat); return; }

                int slot;
                string enemyid = parameters[0];

                if (Resources.Load(Tools.PathUtils.enemiesResPath + "/" + enemyid) as EnemySO == null &&
                !LoadedAssetsHandler.LoadedEnemies.ContainsKey(enemyid))
                { WriteLine("Unknown Enemy"); return; }

                if (!int.TryParse(parameters[1], out slot))
                { WriteLine("Slot is not a number"); return; }

                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't spawn an enemy when out of combat"); return; }

                CombatManager.Instance.AddPriorityRootAction(new SpawnEnemyAction(LoadedAssetsHandler.GetEnemy(enemyid), slot, true, true, CombatTypeID.Spawn_Basic.ToString()));
                { WriteLine("Spawned enemy " + parameters[0] + " in slot " + slot.ToString()); return; }
            });

            //Damage command
            DAMAGE = new DebugCommand("damage", "Damages an enemy or fool.", "damage <damage> <slot> <targetsAllies>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 3)
                { WriteLine("Syntax error: " + DAMAGE.commandFormat); return; }

                int slot;
                int damage;
                bool targetAllies;

                if (!int.TryParse(parameters[0], out damage))
                { WriteLine("Damage is not a number"); return; }

                if (!int.TryParse(parameters[1], out slot))
                { WriteLine("Slot is not a number"); return; }

                if (!bool.TryParse(parameters[2], out targetAllies))
                { WriteLine("TargetsAllies is not true or false"); return; }

                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't damage when out of combat"); return; }

                GenericTargetting_BySlot_Index target = ScriptableObject.CreateInstance<GenericTargetting_BySlot_Index>();
                target.slotPointerDirections = new int[1] { slot };
                target.getAllies = targetAllies;

                EffectInfo killdmg = Effects.GenerateEffect(ScriptableObject.CreateInstance<DamageEffect>(),
                    damage, target);

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(new EffectInfo[1] { killdmg },
                    CombatManager.Instance._stats.CharactersOnField.First().Value));

                { WriteLine("Damaged entity in slot " + slot.ToString() + " for " + damage.ToString() + " damage"); return; }
            });

            //Heal command
            HEAL = new DebugCommand("heal", "Heals an enemy or fool.", "heal <amount> <slot> <targetsAllies>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 3)
                { WriteLine("Syntax error: " + HEAL.commandFormat); return; }

                int slot;
                int amount;
                bool targetAllies;

                if (!int.TryParse(parameters[0], out amount))
                { WriteLine("Amount is not a number"); return; }

                if (!int.TryParse(parameters[1], out slot))
                { WriteLine("Slot is not a number"); return; }

                if (!bool.TryParse(parameters[2], out targetAllies))
                { WriteLine("TargetsAllies is not true or false"); return; }

                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't heal when out of combat"); return; }

                GenericTargetting_BySlot_Index target = ScriptableObject.CreateInstance<GenericTargetting_BySlot_Index>();
                target.slotPointerDirections = new int[1] { slot };
                target.getAllies = targetAllies;

                EffectInfo heal = Effects.GenerateEffect(ScriptableObject.CreateInstance<HealEffect>(), amount, target);

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(new EffectInfo[1] { heal },
                    CombatManager.Instance._stats.CharactersOnField.First().Value));

                { WriteLine("Healed entity in slot " + slot.ToString() + " for " + amount.ToString() + " HP"); return; }
            });

            //Level up command
            LEVELUP = new DebugCommand("lvlup", "Changes the level of a fool.", "lvlup <partySlot>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                { WriteLine("Syntax error: " + LEVELUP.commandFormat); return; }

                int slot;

                if (!int.TryParse(parameters[0], out slot))
                { WriteLine("Slot is not a number"); return; }

                info.Run.playerData.RankUpCharacter(slot, 0);
                IMinimalCharacterInfo character = info.Run.playerData.GetCharacterFromPartySlot(slot);

                { WriteLine("Leveled up " + character.Character._characterName); return; }
            });

            //Money command
            MONEY = new DebugCommand("money", "Adds coins.", "money <amount>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 1)
                { WriteLine("Syntax error: " + MONEY.commandFormat); return; }

                int amount;

                if (!int.TryParse(parameters[0], out amount))
                { WriteLine("Money is not a number"); return; }

                info.Run.playerData.AddCurrency(amount);

                { WriteLine("Added " + amount.ToString() + " coins"); return; }
            });

            //Teleport command
            TP = new DebugCommand("tp", "Teleports you to an area.", "tp <areaID> <restartArea>", (string[] parameters) =>
            {
                if (parameters == null || parameters.Length != 2)
                { WriteLine("Syntax error: " + TP.commandFormat); return; }

                int ID;
                bool reroll;

                if (!int.TryParse(parameters[0], out ID))
                { WriteLine("Area ID is not a number"); return; }

                if (!bool.TryParse(parameters[1], out reroll))
                { WriteLine("Restart Area is not true or false"); return; }

                ChangeArea(ID, reroll);

                { WriteLine("Teleported to " + info.Run.CurrentZoneDB.ZoneName); return; }
            });

            //ENDCOMBAT
            ENDCOMBAT = new DebugCommand("endcombat", "Instantly ends the current combat.", "endcombat", (string[] parameters) =>
            {
                if (parameters.Length < 0)
                { WriteLine("Syntax error: " + ENDCOMBAT.commandFormat); return; }

                var firstChar = CombatManager.Instance._stats.CharactersOnField.First().Value;
                CombatEndEffect combatEndEffect = ScriptableObject.CreateInstance<CombatEndEffect>();
                combatEndEffect._ignoreLoot = false;

                EffectInfo effect = Effects.GenerateEffect(combatEndEffect);

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(new EffectInfo[1] { effect }, firstChar));
                WriteLine("Ended combat"); return;
            });

            //PIGMENT
            PIGMENT = new DebugCommand("genpigment", "Generates the input pigment.", "genpigment <pigmentID> <pigmentID> <pigmentID>...", (string[] parameters) =>
            {
                if (!CombatManager.Instance._combatInitialized)
                { WriteLine("Can't generate pigment when out of combat"); return; }

                if (parameters == null || parameters.Length == 0)
                { WriteLine("Syntax error: " + PIGMENT.commandFormat); return; }

                EffectInfo[] manaEffects = new EffectInfo[parameters.Length];
                
                var firstChar = CombatManager.Instance._stats.CharactersOnField.First().Value;

                for (int i = 0; i < parameters.Length; i++)
                {
                    GenerateColorManaEffect colorManaEffect = ScriptableObject.CreateInstance<GenerateColorManaEffect>();

                    ManaColorSO manaColor = LoadedDBsHandler.PigmentDB.GetPigment(parameters[i]);
                    if (manaColor == null || manaColor.Equals(null))
                    { WriteLine("ID is not a valid pigment ID"); return; }

                    colorManaEffect.mana = manaColor;
                    
                    manaEffects[i] = Effects.GenerateEffect(colorManaEffect, 1);
                }

                CombatManager.Instance.AddPriorityRootAction(new EffectAction(manaEffects, firstChar));

                WriteLine("Generated " + parameters.Length + " pigment"); return;
            });


            commandList = new List<DebugCommand>
            {
                HELP,
                GIVE,
                ADDFOOL,
                SPAWN,
                DAMAGE,
                HEAL,
                LEVELUP,
                MONEY,
                TP,
                ENDCOMBAT,
                PIGMENT
            };

            Debug.Log("Debug Console Ready");
        }

        public void WriteLine(string s)
        {
            output += "\n" + s;
        }

        public void ChangeArea(int areaID, bool rerollArea = false)
        {
            if (overworldManager._zoneBeingLoaded)
            {
                return;
            }
            overworldManager._zoneBeingLoaded = true;
            overworldManager._soundManager.ReleaseOverworldMusic();
            RunDataSO run = overworldManager._informationHolder.Run;
            ZoneDataBaseSO currentZoneDB = run.CurrentZoneDB;
            run._currentZoneID = areaID;
            if (rerollArea)
                run.ResetZoneData();
            string nextSceneName = overworldManager._mainMenuSceneName;
            if (run.DoesCurrentZoneExist)
            {
                overworldManager._informationHolder.Run.zoneLoadingType = ZoneLoadingType.ZoneStart;
                overworldManager._soundManager.TryStopAmbience();
                nextSceneName = SceneManager.GetActiveScene().name;
                overworldManager._soundManager.PlayOneshotSound(overworldManager._soundManager.changeZone);
            }
            overworldManager.StartCoroutine(overworldManager.LoadNextZone(nextSceneName));
        }
    }

}
