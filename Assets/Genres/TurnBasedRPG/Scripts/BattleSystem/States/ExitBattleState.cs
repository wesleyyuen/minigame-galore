using UnityEngine;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public sealed class ExitBattleState : State
    {
        private BattleStateMachine _battleFsm;
            
        public ExitBattleState(
            StateMachine stateMachine) : base(BattleStateType.ExitBattle.ToString(), stateMachine)
        {
            _battleFsm = (BattleStateMachine) stateMachine;
        }
    
        public override async void EnterState(object args = null)
        {
            switch (_battleFsm.BattleResult)
            {
                case BattleResult.Unresolved:
                    Debug.LogError("Battle is not resolved, should not reach ExitBattleState yet!");
                    break;
                case BattleResult.PlayerWon:
                    await UIManager.Instance.SetBattleText($"You won!");
                    _battleFsm.EndBattle();
                    break;
                case BattleResult.PlayerLost:
                    await UIManager.Instance.SetBattleText("You have no more Pokemon that can fight!");
                    await UIManager.Instance.SetBattleText("You were overwhelmed by your defeat!");
                    _battleFsm.BlackOut();
                    break;
                case BattleResult.WildPokemonCaught:
                    // Text already handled by item action
                    _battleFsm.EndBattle();
                    break;
                case BattleResult.PlayerRan:
                    if (_battleFsm.BattleInfo.IsTrainerBattle)
                    {
                        await UIManager.Instance.SetBattleText("You cannot run from a trainer battle!");
                        _battleFsm.ChangeState(BattleStateType.PlayerDecision.ToString());
                        return;
                    }
                    
                    await UIManager.Instance.SetBattleText("You ran away safely!");
                    _battleFsm.EndBattle();
                    break;
            }

            UIManager.Instance.OnBattleEnd();
            _battleFsm.ChangeState(BattleStateType.NonBattle.ToString());
        }
    }
}
