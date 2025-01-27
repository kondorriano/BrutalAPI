using MonoMod.RuntimeDetour;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using BepInEx.Logging;
using HarmonyLib;
using System.Text;

namespace BrutalAPI
{
    [HarmonyPatch]
    public class DebugController : MonoBehaviour
    {
        #region Static Data
        public static DebugController _instance;
        public static ManualLogSource ConsoleLogger = BepInEx.Logging.Logger.CreateLogSource("BrutalAPI Console");

        public static DebugController Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject("DebugController").AddComponent<DebugController>();

                return _instance;

            }
        }
        #endregion

        #region Private Data
        private bool showConsole;
        private string output;

        private string input;
        private Vector2 scroll;

        private readonly List<string> autocompleteOptions = [];
        private string argInfo = "";

        private int historyID;
        private readonly List<string> history = [];
        #endregion

        #region Commands
        public static readonly DebugCommandGroup Commands = new(string.Empty, string.Empty);

        public static readonly DebugCommand HELP;
        public static readonly DebugCommand GIVE;
        public static readonly DebugCommand ADDFOOL;
        public static readonly DebugCommand SPAWN;
        public static readonly DebugCommand DAMAGE;
        public static readonly DebugCommand HEAL;
        public static readonly DebugCommand LEVELUP;
        public static readonly DebugCommand MONEY;
        public static readonly DebugCommand TP;
        public static readonly DebugCommand ENDCOMBAT;
        public static readonly DebugCommand PIGMENT;
        public static readonly DebugCommand SKIP;
        public static readonly DebugCommand REFRESH;
        public static readonly DebugCommand CONSUMEPIGMENT;
        public static readonly DebugCommand CHANGENEXTZONEBOSS;
        public static readonly DebugCommand UNLOCKCONTENT;
        public static readonly DebugCommand LOCKCONTENT;
        #endregion

        #region Autocomplete
        public static readonly AutocompletionGroup ItemAutocomplete = new(LoadItemIds);
        public static readonly AutocompletionGroup CharacterAutocomplete = new(() => LoadedDBsHandler.CharacterDB.CharactersList);
        public static readonly AutocompletionGroup EnemyAutocomplete = new(() => LoadedDBsHandler.EnemyDB.EnemiesList);
        public static readonly AutocompletionGroup PigmentAutocomplete = new(() => LoadedDBsHandler.PigmentDB.PigmentPool.Keys);
        public static readonly AutocompletionGroup BossAutocomplete = new(LoadBossIds);
        public static readonly AutocompletionGroup UnlocksAutocomplete = new(() => LoadUnlockables().Keys);
        #endregion

        #region Log Colors
        public static readonly Color MessageColor = Color.white;
        public static readonly Color ErrorColor = Color.red;
        public static readonly Color FatalColor = new(0.5f, 0f, 0f);
        public static readonly Color WarningColor = Color.yellow;
        public static readonly Color InfoColor = Color.grey;
        public static readonly Color DebugColor = Color.grey;
        #endregion

        #region Misc Setup
        static DebugController()
        {
            //Help command
            HELP = new("help", "Shows available commands and information about them.", new()
            {
                new StringCommandArgument("command", optional: true)
            },
            args =>
            {
                if (args[0].TryRead<string>(out var c) && !string.IsNullOrEmpty(c))
                {
                    var cmd = string.Join(" ", args.Select(x => x.Read<string>() ?? ""));

                    if (Instance.TryReadCommand(cmd, out var command, out _, out var fullName))
                        Instance.WriteLine(command.FormatCommand(fullName));

                    else
                        Instance.WriteLine($"Unknown command \"{cmd}\".", LogLevel.Error);

                    return;
                }
                else
                {
                    static void RecurseCommands(DebugCommandGroup group, string hierarchy)
                    {
                        if (!string.IsNullOrEmpty(group.name))
                            Instance.WriteLine(group.FormatCommand(hierarchy));

                        foreach (var cmd in group.children)
                            RecurseCommands(cmd, string.IsNullOrEmpty(hierarchy) ? cmd.name : $"{hierarchy} {cmd.name}");
                    }

                    RecurseCommands(Commands, "");
                }
            }, true);

            //Give command
            GIVE = new("give", "Adds an item to your inventory.", new()
            {
                new StringCommandArgument("item", ItemAutocomplete)
            },
            args =>
            {
                var itmName = args[0].Read<string>();

                if (itmName == null)
                    return;

                var itm = LoadedAssetsHandler.GetWearable(itmName);

                if (itm == null)
                {
                    Instance.WriteLine($"Unknown item \"{itmName}\".", LogLevel.Error);

                    return;
                }

                var run = LoadedDBsHandler.InfoHolder.Run;

                if (run == null)
                {
                    Instance.WriteLine("No active run.", LogLevel.Error);

                    return;
                }

                run.playerData.AddNewItem(itm);

                Instance.WriteLine($"Added item {itmName} to inventory.");
            });

            //AddFool command
            ADDFOOL = new("addfool", "Adds a fool to your party.", new()
            {
                new StringCommandArgument("fool", CharacterAutocomplete),
                new NumericalCommandArgument("rank", 0, optional: true),
                new NumericalCommandArgument("abilityOne", optional: true),
                new NumericalCommandArgument("abilityTwo", optional: true)
            },
            args =>
            {
                var foolName = args[0].Read<string>();

                if (foolName == null)
                    return;

                var fool = LoadedAssetsHandler.GetCharacter(foolName);

                if (fool == null)
                {
                    Instance.WriteLine($"Unknown character \"{foolName}\".", LogLevel.Error);

                    return;
                }

                var run = LoadedDBsHandler.InfoHolder.Run;

                if (run == null)
                {
                    Instance.WriteLine("No active run.", LogLevel.Error);

                    return;
                }

                if (!args[1].TryRead(out int rank))
                    rank = 0;

                int? abilityOne = null;
                int? abilityTwo = null;

                if (args[2].TryRead(out int abOne))
                    abilityOne = abOne;
                if (args[3].TryRead(out int abTwo))
                    abilityTwo = abTwo;

                if (abilityOne == null && abilityTwo == null)
                {
                    var abs = fool.GenerateAbilities();

                    abilityOne = abs[0];
                    abilityTwo = abs[1];
                }

                else if (abilityOne == null)
                    abilityOne = abilityTwo switch
                    {
                        0 => UnityEngine.Random.Range(1, 3),
                        1 => UnityEngine.Random.Range(0, 2) * 2,
                        2 => UnityEngine.Random.Range(0, 2),

                        _ => UnityEngine.Random.Range(0, 3)
                    };

                else if (abilityTwo == null)
                    abilityTwo = abilityOne switch
                    {
                        0 => UnityEngine.Random.Range(1, 3),
                        1 => UnityEngine.Random.Range(0, 2) * 2,
                        2 => UnityEngine.Random.Range(0, 2),

                        _ => UnityEngine.Random.Range(0, 3)
                    };

                run.playerData.AddNewCharacter(fool, rank, new int[] { abilityOne.GetValueOrDefault(), abilityTwo.GetValueOrDefault() });

                Instance.WriteLine($"Added fool {foolName} to party.");
            });

            //Spawn command
            SPAWN = new("spawn", "Spawns an enemy in battle.", new()
            {
                new StringCommandArgument("enemy", EnemyAutocomplete),
                new NumericalCommandArgument("slot", 0, optional: true)
            },
            args =>
            {
                var enemyName = args[0].Read<string>();

                if (enemyName == null)
                    return;

                var enemy = LoadedAssetsHandler.GetEnemy(enemyName);

                if (enemy == null)
                {
                    Instance.WriteLine($"Unknown enemy \"{enemyName}\"", LogLevel.Error);

                    return;
                }

                if (!args[1].TryRead(out int slot))
                    slot = -1;

                CombatManager.Instance.AddPriorityRootAction(new SpawnEnemyAction(enemy, slot, true, false, CombatType_GameIDs.Spawn_Basic.ToString()));

                Instance.WriteLine($"Spawned enemy {enemyName} in slot {slot}.");
            });

            //Damage command
            DAMAGE = new("damage", "Damages an enemy or fool.", new()
            {
                new NumericalCommandArgument("damage"),
                new NumericalCommandArgument("slot", 0),
                new BoolCommandArgumnt("damageCharacters"),
                new BoolCommandArgumnt("indirect", true)
            },
            args =>
            {
                var dmg = args[0].Read<int>();
                var slot = args[1].Read<int>();
                var characters = args[2].Read<bool>();

                if (!args[3].TryRead<bool>(out var indirect))
                    indirect = false;

                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(x =>
                {
                    var target = x.combatSlots.GetGenericAllySlotTarget(slot, characters);
                    if (target == null || !target.HasUnit)
                        return;

                    var u = target.Unit;
                    var targetOffs = target.SlotID - u.SlotID;

                    if (indirect)
                        u.Damage(dmg, null, DeathType_GameIDs.Basic.ToString(), targetOffs, false, false, true, "");
                    else
                        u.Damage(dmg, null, DeathType_GameIDs.Basic.ToString(), targetOffs, true, true, false, "");
                }));

                Instance.WriteLine($"Damaged {(characters ? "character" : "enemy")} in slot {slot} for {dmg} damage.");
            });

            //Heal command
            HEAL = new("heal", "Heals an enemy or fool.", new()
            {
                new NumericalCommandArgument("healing"),
                new NumericalCommandArgument("slot", 0),
                new BoolCommandArgumnt("healCharacters"),
                new BoolCommandArgumnt("indirect", true)
            },
            args =>
            {
                var heal = args[0].Read<int>();
                var slot = args[1].Read<int>();
                var characters = args[2].Read<bool>();

                if (!args[3].TryRead<bool>(out var indirect))
                    indirect = false;

                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(x =>
                {
                    var target = x.combatSlots.GetGenericAllySlotTarget(slot, characters);
                    if (target == null || !target.HasUnit)
                        return;

                    var u = target.Unit;
                    u.Heal(heal, null, !indirect, "");
                }));

                Instance.WriteLine($"Healed {(characters ? "character" : "enemy")} in slot {slot} for {heal} health.");
            });

            //Level up command
            LEVELUP = new("lvlup", "Changes the level of a fool.", new()
            {
                new NumericalCommandArgument("partySlot", 0)
            },
            args =>
            {
                var slot = args[0].Read<int>();

                LoadedDBsHandler.InfoHolder.Run.playerData.RankUpCharacter(slot, 0);
                var character = LoadedDBsHandler.InfoHolder.Run.playerData.GetCharacterFromPartySlot(slot);

                Instance.WriteLine($"Leveled up {character.Character._characterName}.");
            });

            //Money command
            MONEY = new("money", "Adds coins.", new()
            {
                new NumericalCommandArgument("amount")
            },
            args =>
            {
                var amount = args[0].Read<int>();

                LoadedDBsHandler.InfoHolder.Run.playerData.AddCurrency(amount);

                Instance.WriteLine($"Added {amount} coins.");
            });

            //Teleport command
            TP = new("tp", "Teleports you to an area.", new()
            {
                new NumericalCommandArgument("areaIndex", 0),
                new BoolCommandArgumnt("restartArea", true)
            },
            args =>
            {
                var area = args[0].Read<int>();
                var restart = args[1].Read<bool>();

                var notif = new ZoneChangeNtf(area, restart);
                Tools.NtfUtils.notifications.PostNotification(Tools.Utils.changeSpecificZoneNtf, Instance, notif);

                Instance.WriteLine($"Teleported to {LoadedDBsHandler.InfoHolder.Run.CurrentZoneDB.ZoneName}.");
            });

            //ENDCOMBAT
            ENDCOMBAT = new("endcombat", "Instantly ends the current combat.", [], args =>
            {
                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(x =>
                {
                    x.TriggerPrematureFinalization();
                }));

                Instance.WriteLine("Ended combat.");
            });

            //PIGMENT
            PIGMENT = new("genpigment", "Generates the input pigment.", new()
            {
                new StringCommandArgument("pigment", PigmentAutocomplete, true)
            },
            args =>
            {
                var pigments = new List<ManaColorSO>();

                foreach (var arg in args)
                {
                    if (!arg.TryRead(out string pigmentId) || string.IsNullOrEmpty(pigmentId))
                        continue;

                    var pigment = LoadedDBsHandler.PigmentDB.GetPigment(pigmentId);

                    if (pigment == null)
                    {
                        Instance.WriteLine($"Invalid pigment \"{pigmentId}\".", LogLevel.Error);
                        continue;
                    }

                    pigments.Add(pigment);
                }

                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(x =>
                {
                    foreach (var p in pigments)
                        x.MainManaBar.AddMana(p);
                }));
            }, true);

            SKIP = new("skip", "Skips the current enemy room", [], args =>
            {
                var run = LoadedDBsHandler.InfoHolder.Run;

                if (run == null)
                {
                    Instance.WriteLine("No active run.", LogLevel.Error);
                    return;
                }

                var zone = run.ZoneData[run.CurrentZoneID];

                if (zone == null)
                {
                    Instance.WriteLine("No active zone.", LogLevel.Error);
                    return;
                }

                var card = zone.GetCard(run.CurrentCardID);

                if (card == null)
                {
                    Instance.WriteLine("No active room.", LogLevel.Error);
                    return;
                }

                if (card.CardType == CardType.Boss)
                {
                    Instance.WriteLine("Can't skip boss rooms.", LogLevel.Error);
                    return;
                }

                card.HasBeenSolved = true;

                var ui = FindObjectOfType<OverworldUIHandler>();

                if (ui != null)
                    ui.SetPileButtonState(true);

                Instance.WriteLine("Skipping current room.");
            });

            REFRESH = new("refresh", "Refreshes the target character's abilities and movement.", new()
            {
                new NumericalCommandArgument("slot", 0),
                new BoolCommandArgumnt("refreshAbilities", true),
                new BoolCommandArgumnt("refreshMovement", true),
            }, args =>
            {
                var slot = args[0].Read<int>();

                if (!args[1].TryRead<bool>(out var refreshAbilities))
                    refreshAbilities = true;
                if (!args[2].TryRead<bool>(out var refreshMovement))
                    refreshMovement = true;

                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(x =>
                {
                    var target = x.combatSlots.GetGenericAllySlotTarget(slot, true);
                    if (target == null || !target.HasUnit)
                        return;

                    var u = target.Unit;

                    if (refreshAbilities)
                        u.RefreshAbilityUse();
                    if (refreshMovement)
                        u.RestoreSwapUse();
                }));

                Instance.WriteLine($"Refreshed character in slot {slot}");
            });

            CONSUMEPIGMENT = new("consumepigment", "Consumes all pigment from the pigment bar", [], args =>
            {
                CombatManager.Instance.AddPriorityRootAction(new PerformDelegateAction(x =>
                {
                    x.MainManaBar.ConsumeAllMana();
                }));
            });

            CHANGENEXTZONEBOSS = new("changenextzoneboss", "Replaces the next area's boss. Can only replace the boss with bosses that can normally appear in the next area.", new()
            {
                new StringCommandArgument("boss", BossAutocomplete)
            }, args =>
            {
                var boss = args[0].Read<string>();

                var infoHolder = LoadedDBsHandler.InfoHolder;
                var run = infoHolder.Run;
                if (run == null)
                {
                    Instance.WriteLine("No active run.", LogLevel.Error);
                    return;
                }
                if (run.CurrentZoneID + 1 >= run.zoneData.Count)
                {
                    Instance.WriteLine("No next zone.", LogLevel.Error);
                    return;
                }

                var runZone = run.zoneData[run.CurrentZoneID + 1];
                var zoneData = runZone.LoadZoneDB();
                if (zoneData == null || zoneData.Equals(null))
                {
                    Instance.WriteLine("Invalid next zone.", LogLevel.Error);
                    return;
                }

                var bossBundles = runZone.BossBundleArray;
                if (bossBundles.Length == 0)
                {
                    Instance.WriteLine("Next zone has no boss.", LogLevel.Error);
                    return;
                }

                var lastBoss = bossBundles[bossBundles.Length - 1];
                if (boss == lastBoss.BossID)
                {
                    Instance.WriteLine($"Successfully swapped the next zone's boss to {boss}");
                    return;
                }

                var newBoss = zoneData.TryGenerateBossBundle(boss);
                if (newBoss == null)
                {
                    Instance.WriteLine($"Invalid boss \"{boss}\"", LogLevel.Error);
                    return;
                }

                runZone.SwapBossBundle(bossBundles.Length - 1, newBoss);
                Instance.WriteLine($"Successfully swapped the next zone's boss to {boss}");
            });

            UNLOCKCONTENT = new("unlockcontent", "Unlocks unlockable content.", new()
            {
                new StringCommandArgument("unlockableId", UnlocksAutocomplete)
            }, args =>
            {
                var id = args[0].Read<string>();
                var unlocks = LoadUnlockables();

                if (!unlocks.TryGetValue(id, out var unlockData))
                {
                    Instance.WriteLine($"Invalid unlockable \"{id}\".");
                    return;
                }

                LoadedDBsHandler.InfoHolder.UnlockableManager.TryUnlockContent(unlockData);
                Instance.WriteLine($"Successfully unlocked unlockable {id}");
            });

            LOCKCONTENT = new("lockcontent", "Locks unlocked content.", new()
            {
                new StringCommandArgument("unlockableId", UnlocksAutocomplete)
            }, args =>
            {
                var id = args[0].Read<string>();
                var unlocks = LoadUnlockables();

                if (!unlocks.TryGetValue(id, out var unlockData))
                {
                    Instance.WriteLine($"Invalid unlockable \"{id}\".");
                    return;
                }

                var infoHolder = LoadedDBsHandler.InfoHolder;
                var game = infoHolder.Game;
                var unlockManager = infoHolder.UnlockableManager;
                var achs = unlockManager._steamAchievements;

                if (unlockData.hasQuestCompletion && game != null)
                {
                    var dat = game.m_Data;
                    dat.completedQuests.Remove(unlockData.questID);
                    dat.NeedsToBeSaved = true;
                }

                if(unlockData.hasCharacterUnlock && game != null)
                {
                    var dat = game.m_Data;
                    var unlocked = dat.unlockedCharacters.Contains(unlockData.character);

                    dat.unlockedCharacters.Remove(unlockData.character);
                    dat.NeedsToBeSaved = true;

                    if (unlocked)
                    {
                        unlockManager.FreshlyCharacters.Remove(unlockData.character);
                        unlockManager._LoadedFreshs.NeedsToBeSaved = true;
                    }
                }

                if(unlockData.hasItemUnlock && unlockData.items != null && game != null)
                {
                    foreach(var itm in unlockData.items)
                    {
                        if (string.IsNullOrEmpty(itm))
                            continue;

                        var dat = game.m_Data;
                        var unlocked = dat.unlockedItems.Contains(itm);

                        dat.unlockedItems.Remove(itm);
                        dat.NeedsToBeSaved = true;

                        if (unlocked)
                        {
                            unlockManager.FreshlyItems.Remove(itm);
                            unlockManager._LoadedFreshs.NeedsToBeSaved = true;
                        }
                    }
                }

                if (unlockData.HasAchievementUnlock)
                    Instance.WriteLine("Can't lock steam achievements", LogLevel.Warning);

                if (unlockData.hasModdedAchievementUnlock && achs != null && achs.m_ModdedAchievementDict.TryGetValue(unlockData.moddedAchievementID, out var ach) && ach != null && ach.m_offlinebAchieved)
                {
                    ach.m_offlinebAchieved = false;
                    achs._LoadedAchievements.moddedAchievementIDs.Remove(ach.m_eAchievementID);
                    achs._LoadedAchievements.NeedsToBeSaved = true;
                }

                Instance.WriteLine($"Successfully locked unlockable {id}");
            });

            Commands.children.AddRange(new List<DebugCommandGroup>
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
                PIGMENT,
                SKIP,
                REFRESH,
                CONSUMEPIGMENT,
                CHANGENEXTZONEBOSS,
                UNLOCKCONTENT,
                LOCKCONTENT
            });
        }

        private static IEnumerable<string> LoadItemIds()
        {
            var processed = new List<string>();
            var unlocks = LoadedDBsHandler.ItemUnlocksDB;

            foreach (var i in unlocks.ShopItems)
            {
                if (!processed.Contains(i.itemName.ToLowerInvariant()))
                {
                    processed.Add(i.itemName.ToLowerInvariant());

                    yield return i.itemName;
                }
            }

            foreach (var i in unlocks.TreasureItems)
            {
                if (!processed.Contains(i.itemName.ToLowerInvariant()))
                {
                    processed.Add(i.itemName.ToLowerInvariant());

                    yield return i.itemName;
                }
            }

            foreach (var i in unlocks.ExtraItems)
            {
                if (!processed.Contains(i.itemName.ToLowerInvariant()))
                {
                    processed.Add(i.itemName.ToLowerInvariant());

                    yield return i.itemName;
                }
            }

            foreach (var kvp in LoadedAssetsHandler.LoadedWearables)
            {
                if (kvp.Value != null && !processed.Contains(kvp.Key.ToLowerInvariant()))
                {
                    processed.Add(kvp.Key.ToLowerInvariant());

                    yield return kvp.Key;
                }
            }
        }

        private static IEnumerable<string> LoadBossIds()
        {
            var processed = new List<string>();

            foreach (var e in LoadedDBsHandler.EnemyDB.m_EnemyEncounterPool.Values)
            {
                if (e == null || e.m_BossSelector == null)
                    continue;

                var enc = e.m_BossSelector._enemyEncounters;

                if(enc == null)
                    continue;

                foreach(var en in enc)
                {
                    if(en == null || string.IsNullOrEmpty(en.BundleName))
                        continue;

                    var bundl = LoadedAssetsHandler.GetEnemyBundle(en.BundleName);

                    if (bundl == null || string.IsNullOrEmpty(bundl.BossID) || processed.Contains(bundl.BossID))
                        continue;

                    yield return bundl.BossID;
                    processed.Add(bundl.BossID);
                }
            }
        }

        private static Dictionary<string, UnlockableModData> LoadUnlockables()
        {
            var dict = new Dictionary<string, UnlockableModData>();
            var unlocks = LoadedDBsHandler.UnlockablesDB;

            foreach(var c in unlocks.PostCombatChecks)
            {
                if(c == null)
                    continue;

                var dat = c.unlockData;

                if(dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var c in unlocks.CombatGameOverChecks)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;

                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var c in unlocks.HardEndingChecks)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;

                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var c in unlocks.ZoneExtraChecks)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;

                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var c in unlocks.m_ZoneUnlockables.Values)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;
                if (dat != null && !string.IsNullOrEmpty(dat.id))
                    dict[dat.id] = dat;

                var dat2 = c.unlockData;
                if (dat2 != null && !string.IsNullOrEmpty(dat2.id))
                    dict[dat2.id] = dat2;
            }

            foreach (var c in unlocks.m_CharacterDeathUnlockables.Values)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;

                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var c in unlocks.m_BeatBossEncounterUnlockables.Values)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;

                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var c in unlocks.m_BeatEnemyUnlockables.Values)
            {
                if (c == null)
                    continue;

                var dat = c.simpleDeathData;
                if (dat != null && !string.IsNullOrEmpty(dat.id))
                    dict[dat.id] = dat;

                foreach(var dat2 in c.specialDeathData.Values)
                {
                    if (dat2 == null || string.IsNullOrEmpty(dat2.id))
                        continue;

                    dict[dat2.id] = dat2;
                }
            }

            foreach (var c in unlocks.m_FinalBossCharUnlockables.Values)
            {
                if (c == null)
                    continue;

                foreach (var dat in c.entityIDUnlocks.Values)
                {
                    if (dat == null || string.IsNullOrEmpty(dat.id))
                        continue;

                    dict[dat.id] = dat;
                }
            }

            foreach (var c in unlocks.m_MiscUnlockables.Values)
            {
                if (c == null)
                    continue;

                var dat = c.unlockData;

                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            foreach (var dat in unlocks.m_ByIDUnlockables.Values)
            {
                if (dat == null || string.IsNullOrEmpty(dat.id))
                    continue;

                dict[dat.id] = dat;
            }

            return dict;
        }

        private void Awake()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (!showConsole)
                return;

            if (Keyboard.current.enterKey.wasPressedThisFrame && !string.IsNullOrEmpty(input))
            {
                TryExecuteCommand(input);

                ClearInput();
            }

            if (Keyboard.current.upArrowKey.wasPressedThisFrame && history.Count > 0)
            {
                input = history[historyID];
                historyID -= 1;
                historyID = Mathf.Clamp(historyID, 0, history.Count - 1);
            }

            if (Keyboard.current.downArrowKey.wasPressedThisFrame && history.Count > 0)
            {
                input = history[historyID];
                historyID += 1;
                historyID = Mathf.Clamp(historyID, 0, history.Count - 1);
            }
        }
        #endregion

        #region Command and Log Methods
        public void TryExecuteCommand(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;

            if (!TryReadCommand(input, out var cmd, out var eInfo, out _))
            {
                WriteLine($"Unknown command \"{input}\".", LogLevel.Error);

                return;
            }

            cmd.Invoke(eInfo);
            history.Add(input);
            historyID = history.Count - 1;
        }

        public bool TryReadCommand(string input, out DebugCommandGroup command, out string extraInfo, out string fullCommandName)
        {
            if (string.IsNullOrEmpty(input))
            {
                command = null;
                fullCommandName = string.Empty;
                extraInfo = string.Empty;

                return false;
            }

            var args = input.Split([' '], StringSplitOptions.RemoveEmptyEntries).ToList();
            var hierarchy = new List<string>();
            command = Commands;

            while (args.Count > 0 && command != null && command.children != null && command.children.Count > 0)
            {
                var ch = command.children.FirstOrDefault(x => x != null && x.name == args[0]);

                if (ch == null)
                    break;

                command = ch;
                hierarchy.Add(command.name);
                args.RemoveAt(0);
            }

            fullCommandName = string.Join(" ", hierarchy);
            extraInfo = string.Join(" ", args);

            return command != Commands;
        }

        public void WriteLine(string s, LogLevel logLevel = LogLevel.Message)
        {
            var col = logLevel switch
            {
                LogLevel.Fatal => FatalColor,
                LogLevel.Error => ErrorColor,
                LogLevel.Warning => WarningColor,
                LogLevel.Message => MessageColor,
                LogLevel.Info => InfoColor,
                LogLevel.Debug => DebugColor,

                _ => Color.white
            };

            output += "\n" + $"<color=#{ColorUtility.ToHtmlStringRGB(col)}>{s}</color>";

            ConsoleLogger.Log(logLevel, s);
        }

        public void ClearInput()
        {
            ClearTempData();
            input = "";
        }

        private void ClearTempData()
        {
            autocompleteOptions.Clear();
            argInfo = "";
        }
        #endregion

        #region GUI
        private void OnGUI()
        {
            if (!showConsole)
                return;

            GUI.skin.textArea.richText = true;

            var ySpaceOffset = 0f;

            GUI.Box(new Rect(0, 0, Screen.width, 110), "");
            GUI.Box(new Rect(0, 110, Screen.width, 30), "");

            var oldInput = input;
            input = GUI.TextField(new Rect(10f, 115f, Screen.width - 20f, 20f), input);

            AddAutocompleteScrollView(ref ySpaceOffset);

            if (input != oldInput || (Keyboard.current != null && Keyboard.current.ctrlKey.wasPressedThisFrame))
                ProcessInputChange();

            if (!string.IsNullOrEmpty(argInfo))
                GUI.TextArea(new Rect(10f, 135f + ySpaceOffset, Screen.width - 20f, 105f), argInfo);

            GUI.TextArea(new Rect(10f, 5f, Screen.width - 20f, 105f), output);
        }

        private void AddAutocompleteScrollView(ref float ySpaceOffset)
        {
            var buttonHeight = 20f;
            var numButtonsInScroll = 10;

            var scrollYSpace = 0f;

            scroll = GUI.BeginScrollView(new(10f, 140f, Screen.width - 20f, buttonHeight * numButtonsInScroll), scroll, new(0f, 0f, Screen.width - 45f, buttonHeight * autocompleteOptions.Count));

            for (int i = 0; i < autocompleteOptions.Count; i++)
            {
                var opt = autocompleteOptions[i];
                scrollYSpace += buttonHeight;

                if (GUI.Button(new(0f, buttonHeight * i, Screen.width - 25f, buttonHeight), opt))
                {
                    input = AutocompleteInput(input, opt);

                    MoveCursorToInputEnd();
                }
            }

            scrollYSpace = Mathf.Min(scrollYSpace, buttonHeight * numButtonsInScroll);
            scrollYSpace += autocompleteOptions.Count > 0 ? 10f : 5f;

            ySpaceOffset += scrollYSpace;

            GUI.EndScrollView();
        }

        private void ProcessInputChange()
        {
            ClearTempData();

            if (!string.IsNullOrEmpty(input) || (Keyboard.current != null && Keyboard.current.ctrlKey.wasPressedThisFrame))
            {
                if (TryReadCommand(input, out var cmd, out var extraInfo, out var fullname))
                {
                    var args = extraInfo.Split([' '], StringSplitOptions.RemoveEmptyEntries);

                    if (input.EndsWith(" "))
                        args = [.. args, ""];

                    if (args.Length <= 0)
                        return;

                    if (args.Length == 1)
                    {
                        autocompleteOptions.AddRange(cmd.AutocompleteChildren(args[0]));
                    }

                    var builder = new StringBuilder();

                    builder.AppendLine($"{fullname}: {cmd.description}");
                    builder.AppendLine();

                    cmd.ProcessAutocompletion(autocompleteOptions, args);
                    cmd.AddArgumentInfo(builder, args);

                    argInfo = builder.ToString();
                }
                else
                {
                    autocompleteOptions.AddRange(Commands.AutocompleteChildren(input));
                }
            }
        }

        private void MoveCursorToInputEnd()
        {
            TextEditor editor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            editor.text = input;
            editor.cursorIndex = input.Length + 1;
            editor.selectIndex = input.Length + 1;
        }

        public static string AutocompleteInput(string input, string autocomplete)
        {
            if (string.IsNullOrEmpty(input))
                return $"{autocomplete} ";

            var split = input.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            if (input.EndsWith(" "))
                split = [.. split, ""];

            split[split.Length - 1] = autocomplete;

            return $"{string.Join(" ", split)} ";
        }
        #endregion

        #region Patches
        [HarmonyPatch(typeof(Keyboard), nameof(Keyboard.OnTextInput))]
        [HarmonyPostfix]
        private static void ToggleConsole(char character)
        {
            if (character == BrutalAPI.OpenDebugConsoleKey[0])
                Instance.showConsole = !Instance.showConsole;
        }
        #endregion
    }
}
