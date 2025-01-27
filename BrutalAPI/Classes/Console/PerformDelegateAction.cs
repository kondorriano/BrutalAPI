using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BrutalAPI
{
    public class PerformDelegateAction(Action<CombatStats> action) : CombatAction
    {
        public override IEnumerator Execute(CombatStats stats)
        {
            action?.Invoke(stats);
            yield break;
        }
    }
}
