using TurnBasedRPG;

public sealed class WildBattleState : BattleState
{
    public WildBattleState(
        GameStateMachine stateMachine,
        RoundController roundController) : base(GameState.WildBattle.ToString(), stateMachine, roundController)
    {
    }

    public override void EnterState(System.Object args = null)
    {
        _battleInfo.Add(_fsm.player.TrainerInfo.GetFirstAvailablePokemon());

        if (args is Pokemon pokemon)
        {
            _battleInfo.Add(pokemon);

            _fsm.player.TrainerInfo.IChooseYou();

            _uiManager.SetPokemon(_fsm.player.TrainerInfo);
            _uiManager.SetPokemon(pokemon);
        }

        _fsm.StartCoroutine(_StartBattle());
    }
}