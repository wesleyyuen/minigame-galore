using UnityEngine;
using Cysharp.Threading.Tasks;

namespace TurnBasedRPG
{
    public abstract class Move : ScriptableObject, ITurnAction
    {
        public string Name => name;
        [field: SerializeField] public PokemonType Type {get; private set;}
        [field: SerializeField, Range(0, 1)] public float Accuracy {get; private set;} = 1f;
        [field: SerializeField] public int PP {get; private set;} = 15;
        [SerializeField] private bool _isPriorityMove;
        public int Priority => _isPriorityMove ? Constants.PRIORITY_MOVE_PRIORITY : Constants.ATTACK_MOVE_PRIORITY;
        public abstract UniTask<BattleResult> DoAction(Pokemon owner, Pokemon target);
    }
}