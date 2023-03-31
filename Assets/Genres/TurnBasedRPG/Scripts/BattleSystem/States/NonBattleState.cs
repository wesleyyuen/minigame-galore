using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public sealed class NonBattleState : MonoState
    {
        public NonBattleState(
            MonoStateMachine stateMachine) : base(BattleState.NonBattle.ToString(), stateMachine)
        {
        }
    }
}
