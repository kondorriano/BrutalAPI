using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BrutalAPI
{
    public class DebugCommandGroup(string name, string description)
    {
        public List<DebugCommandGroup> children = [];

        public string name = name;
        public string description = description;

        public virtual void Invoke(string extraInfo)
        {
            DebugController.Instance.WriteLine("Can't invoke command groups.", BepInEx.Logging.LogLevel.Error);
        }

        public virtual void ProcessAutocompletion(List<string> autocompleteOptions, string[] inputArgs)
        {

        }

        public virtual void AddArgumentInfo(StringBuilder builder, string[] inputArgs)
        {
        }

        public IEnumerable<string> AutocompleteChildren(string input)
        {
            foreach (var ch in children)
            {
                if (ch == null || string.IsNullOrEmpty(ch.name) || (!string.IsNullOrEmpty(input) && !ch.name.ToLowerInvariant().Contains(input.ToLowerInvariant())))
                    continue;

                yield return ch.name;
            }
        }

        public virtual string FormatCommand(string fullName)
        {
            return $"{fullName}: {description}";
        }
    }

    public class DebugCommand(string name, string description, List<DebugCommandArgument> arguments, Action<List<FilledCommandArgument>> execute, bool infinitelyRepeatLastArg = false) : DebugCommandGroup(name, description)
    {
        public List<DebugCommandArgument> arguments = arguments ?? [];
        public Action<List<FilledCommandArgument>> execute = execute;

        public bool infinitelyRepeatLastArg = infinitelyRepeatLastArg;

        public override void Invoke(string extraInfo)
        {
            if(TryParseArguments(extraInfo, out var fillArgs))
                execute?.Invoke(fillArgs);
        }

        public bool TryParseArguments(string info, out List<FilledCommandArgument> filledArgs)
        {
            var strArgs = info.Split([' '], StringSplitOptions.RemoveEmptyEntries);
            filledArgs = new List<FilledCommandArgument>();

            for (int i = 0; i < arguments.Count; i++)
            {
                var arg = arguments[i];

                if (arg == null)
                {
                    filledArgs.Add(new(null));
                    DebugController.Instance.WriteLine($"Null argument at position {i + 1}.", BepInEx.Logging.LogLevel.Error);

                    continue;
                }

                if (i >= strArgs.Length)
                {
                    if (arg.optional)
                    {
                        filledArgs.Add(new(null));

                        continue;
                    }
                    else
                    {
                        DebugController.Instance.WriteLine($"Missing required argument at position {i + 1}.", BepInEx.Logging.LogLevel.Error);

                        return false;
                    }
                }

                var couldRead = arg.TryRead(strArgs[i], out var fill, out var message);

                if(!string.IsNullOrEmpty(message))
                    DebugController.ConsoleLogger.Log(couldRead ? BepInEx.Logging.LogLevel.Message : BepInEx.Logging.LogLevel.Error, message);

                if (couldRead)
                    filledArgs.Add(fill);

                else
                    return false;
            }

            if(infinitelyRepeatLastArg && arguments.Count > 0)
            {
                var lastArg = arguments[arguments.Count - 1];

                if(lastArg != null)
                {
                    for (int i = arguments.Count; i < strArgs.Length; i++)
                    {
                        var couldRead = lastArg.TryRead(strArgs[i], out var fill, out var message);

                        if (!string.IsNullOrEmpty(message))
                            DebugController.ConsoleLogger.Log(couldRead ? BepInEx.Logging.LogLevel.Message : BepInEx.Logging.LogLevel.Error, message);

                        if (couldRead)
                            filledArgs.Add(fill);

                        else
                            return false;
                    }
                }
            }

            return true;
        }

        public override void AddArgumentInfo(StringBuilder builder, string[] inputArgs)
        {
            var argPosition = inputArgs.Length - 1;

            for (int i = 0; i < arguments.Count; i++)
            {
                var ar = arguments[i];
                var col = Color.white;

                var argLine = $"{ar.name}: {ar.ExtraInfo}";

                if (ar.optional)
                    argLine += " (Optional)";

                if (infinitelyRepeatLastArg && i == arguments.Count - 1)
                    argLine += " (Infinitely repeatable)";

                if (i > argPosition)
                {
                    col = Color.grey;
                }
                else if (!string.IsNullOrEmpty(inputArgs[i]))
                {
                    var readRes = ar.TryRead(inputArgs[i], out _, out var msg);

                    if (!string.IsNullOrEmpty(msg))
                        argLine += $" ({msg})";

                    if (!readRes)
                        col = Color.red;
                }

                if (i == argPosition || (infinitelyRepeatLastArg && i < argPosition && i == arguments.Count - 1))
                {
                    argLine = $"<b>{argLine}</b>";
                }

                argLine = $"<color=#{ColorUtility.ToHtmlStringRGB(col)}>{argLine}</color>";

                builder.AppendLine(argLine);
            }
        }

        public override void ProcessAutocompletion(List<string> autocompleteOptions, string[] inputArgs)
        {
            var argPosition = inputArgs.Length - 1;

            if (inputArgs.Length <= arguments.Count || infinitelyRepeatLastArg)
            {
                var arg = arguments[Mathf.Min(argPosition, arguments.Count - 1)];
                var argIn = inputArgs[argPosition];

                var coll = arg.Autocomplete(argIn);

                if (coll != null)
                    autocompleteOptions.AddRange(coll);
            }
        }

        public override string FormatCommand(string fullName)
        {
            if (arguments.Count > 0)
                return $"{fullName} {string.Join(" ", arguments)}: {description}";

            return base.FormatCommand(fullName);
        }
    }
}
