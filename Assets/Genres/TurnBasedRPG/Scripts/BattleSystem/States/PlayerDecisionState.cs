using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    public sealed class PlayerDecisionState : MonoState
    {
        private BattleStateMachine _battleFsm;
            
        private PlayerDecisionState(
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
