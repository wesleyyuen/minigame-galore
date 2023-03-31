using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public sealed class ExitBattleState : MonoState
    {
        private BattleStateMachine _battleFsm;
            
        private ExitBattleState(
            string name,
            MonoStateMachine stateMachine) : base(name, stateMachine)
        {
            _battleFsm = (BattleStateMachine) stateMachine;
        }
    
        public override void EnterState(object args = null)
        {
        }
    }
}
