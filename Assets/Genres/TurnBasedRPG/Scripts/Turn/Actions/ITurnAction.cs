using System;
using System.Collections.Generic;

public interface ITurnAction
{
    int Priority {get;}
    abstract IEnumerator<float> DoAction(Pokemon Owner, Pokemon Target, Action<BattleResult> callback);
}