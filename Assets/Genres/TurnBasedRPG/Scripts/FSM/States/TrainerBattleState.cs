using TurnBasedRPG;
using TurnBasedRPG.UI;

public class TrainerBattleState : BattleState
{
    public TrainerBattleState(
        GameStateMachine stateMachine,
        RoundController roundController) : base(stateMachine, roundController)
    {
    }

    public override void EnterState(object args = null)
    {
        _battleInfo.Add(_fsm.player.TrainerInfo.GetFirstAvailablePokemon());

        if (args is Trainer trainer)
        {
            _battleInfo.Add(trainer.GetFirstAvailablePokemon());

            _fsm.player.TrainerInfo.IChooseYou();
            trainer.IChooseYou();

            _uiManager.SetPokemon(_fsm.player.TrainerInfo);
            _uiManager.SetPokemon(trainer);
        }

        _fsm.StartCoroutine(_StartBattle());
    }
}