using System;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;

public abstract class Move : ScriptableObject, ITurnAction
{
    public string Name => name;
    [SerializeField] private PokemonType _type;
    public PokemonType Type => _type;
    [SerializeField] private bool _isPriorityMove;
    [SerializeField] private int _pp = 5;
    public int PP => _pp;
    public int Priority => _isPriorityMove ? Constants.PRIORITY_MOVE_PRIORITY : Constants.ATTACK_MOVE_PRIORITY;
    public abstract IEnumerator<float> DoAction(Pokemon owner, Pokemon target, Action<BattleResult> callback);
}