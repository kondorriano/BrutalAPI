using System;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public class AutocompletionGroup(Func<IEnumerable<string>> getOptions)
    {
        public Func<IEnumerable<string>> getOptions = getOptions;

        public IEnumerable<string> Autocomplete(string input)
        {
            if (getOptions == null)
                yield break;

            var opts = getOptions();

            foreach(var opt in opts)
            {
                if (string.IsNullOrEmpty(opt) || (!string.IsNullOrEmpty(input) && !opt.ToLowerInvariant().Contains(input.ToLowerInvariant())))
                    continue;

                yield return opt;
            }
        }
    }
}
