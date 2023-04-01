using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public sealed class NonBattleState : State
    {
        public NonBattleState(
            StateMachine stateMachine) : base(BattleStateType.NonBattle.ToString(), stateMachine)
        {
        }
    }
}
