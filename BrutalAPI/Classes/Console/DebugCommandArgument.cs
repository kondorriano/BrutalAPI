using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public abstract class DebugCommandArgument(string name, bool optional)
    {
        public string name = name;
        public bool optional = optional;

        public abstract string ExtraInfo { get; }

        public abstract bool TryRead(string argString, out FilledCommandArgument filledArg, out string message);
        public abstract IEnumerable<string> Autocomplete(string content);

        public override string ToString()
        {
            var output = $"{name}: {ExtraInfo}";

            if (optional)
                output = $"({output})";

            else
                output = $"<{output}>";

            return output;
        }
    }

    public class NumericalCommandArgument(string name, int? rangeMin = null, int? rangeMax = null, bool optional = false) : DebugCommandArgument(name, optional)
    {
        public int? rangeMin = rangeMin;
        public int? rangeMax = rangeMax;

        public override string ExtraInfo
        {
            get
            {
                if (rangeMin != null && rangeMax != null)
                    return $"{rangeMin.GetValueOrDefault()}-{rangeMax.GetValueOrDefault()}";

                if (rangeMin != null)
                    return $"{rangeMin.GetValueOrDefault()}+";

                if (rangeMax != null)
                    return $"{rangeMax.GetValueOrDefault()}-";

                return "numerical";
            }
        }

        public override IEnumerable<string> Autocomplete(string content)
        {
            return null;
        }

        public override bool TryRead(string argString, out FilledCommandArgument filledArg, out string message)
        {
            if(!int.TryParse(argString, out var i))
            {
                message = $"Value of numerical argument {name} isn't numerical.";
                filledArg = null;

                return false;
            }

            if(rangeMin != null && i < rangeMin.GetValueOrDefault())
            {
                message = $"Value of numerical argument {name} is less than its minimum value ({rangeMin.GetValueOrDefault()})";
                filledArg = null;

                return false;
            }

            if (rangeMax != null && i > rangeMax.GetValueOrDefault())
            {
                message = $"Value of numerical argument {name} is greater than its maximum value ({rangeMax.GetValueOrDefault()})";
                filledArg = null;

                return false;
            }

            filledArg = new(i);
            message = null;
            return true;
        }
    }

    public class StringCommandArgument(string name, AutocompletionGroup autocomplete = null, bool optional = false) : DebugCommandArgument(name, optional)
    {
        public AutocompletionGroup autocomplete = autocomplete;

        public override string ExtraInfo => "text";

        public override IEnumerable<string> Autocomplete(string content)
        {
            if (autocomplete == null)
                return null;

            return autocomplete.Autocomplete(content);
        }

        public override bool TryRead(string argString, out FilledCommandArgument filledArg, out string message)
        {
            filledArg = new(argString);
            message = null;

            return true;
        }
    }

    public class BoolCommandArgumnt(string name, bool optional = false) : DebugCommandArgument(name, optional)
    {
        public override string ExtraInfo => "true/false";

        public override IEnumerable<string> Autocomplete(string content)
        {
            yield return "true";
            yield return "false";
        }

        public override bool TryRead(string argString, out FilledCommandArgument filledArg, out string message)
        {
            if(!bool.TryParse(argString, out var ret))
            {
                message = $"Value of logical argument {name} isn't true or false.";
                filledArg = null;

                return false;
            }

            filledArg = new(ret);
            message = null;
            return true;
        }
    }

    public class FilledCommandArgument(object value)
    {
        private object value = value;

        public T Read<T>()
        {
            if(value is T ret)
                return ret;

            return default;
        }

        public bool TryRead<T>(out T val)
        {
            if(value is T ret)
            {
                val = ret;

                return true;
            }

            val = default;
            return false;
        }
    }
}
