using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;
using TurnBasedRPG.UI;

public class BattleState : MonoState
{
    protected GameStateMachine _fsm;
    protected UIManager _uiManager;
    private RoundController _roundController;
    protected BattleInfo _battleInfo = new BattleInfo();
    private List<Turn> _turns = new List<Turn>();
    private BattleResult _result = BattleResult.Unresolved;

    protected BattleStateMachine _battleFSM;

    public BattleState(
        string name,
        GameStateMachine stateMachine,
        RoundController roundController) : base(name, stateMachine)
    {
        _fsm = stateMachine;
        _roundController = roundController;
        _uiManager = stateMachine.battleScreen.GetComponent<UIManager>();
        stateMachine.battleScreen.alpha = 0f;
        
        if (_fsm.gameObject.TryGetComponent<BattleStateMachine>(out var battleFSM))
        {
            _battleFSM = battleFSM;
        }
        else
        {
            _battleFSM = _fsm.gameObject.AddComponent<BattleStateMachine>();
        }
    }

    public void StartTrainerBattle(Trainer trainer)
    {
        _battleInfo.Add(_fsm.player.TrainerInfo.GetFirstAvailablePokemon());

        _battleInfo.Add(trainer.GetFirstAvailablePokemon());

        _fsm.player.TrainerInfo.IChooseYou();
        trainer.IChooseYou();

        _uiManager.SetPokemon(_fsm.player.TrainerInfo);
        _uiManager.SetPokemon(trainer);
        
        _battleFSM.StartBattle(_battleInfo);
    }

    public void StartWildBattle(Pokemon pokemon)
    {
        _battleInfo.Add(_fsm.player.TrainerInfo.GetFirstAvailablePokemon());

        _battleInfo.Add(pokemon);

        _fsm.player.TrainerInfo.IChooseYou();

        _uiManager.SetPokemon(_fsm.player.TrainerInfo);
        _uiManager.SetPokemon(pokemon);
        
        _battleFSM.StartBattle(_battleInfo);
    }

    // protected IEnumerator _StartBattle()
    // {
    //     _fsm.battleScreen.alpha = 1f;
    //     _result = BattleResult.Unresolved;
    //     _uiManager.OnBattleStart();
    //
    //     while (_result == BattleResult.Unresolved)
    //     {
    //         yield return _ProcessRound();
    //     }
    //
    //     _fsm.StartCoroutine(_EndBattle());
    // }
    //
    // protected IEnumerator _ProcessRound()
    // {
    //     // Getting all turns in this round
    //     foreach (var (pkmn, trainer) in _battleInfo.Pokemons)
    //     {
    //         var pokemon = trainer == null ? pkmn : trainer.PokemonInBattle;
    //         var targetKvp = _battleInfo.GetOpponent(trainer);
    //         var targetTrainer = targetKvp.Value;
    //         var target = targetTrainer == null ? targetKvp.Key : targetTrainer.PokemonInBattle;
    //
    //         // Wait for player input
    //         if (trainer != null && trainer.IsPlayer)
    //         {
    //             // Wait for player to choose next mon
    //             if (pokemon == null)
    //             {
    //                 bool switchMonSelected = false;
    //                 _uiManager.GetPlayerPokemonSelection(action => {
    //                     _uiManager.OnPokemonSelectedFinished();
    //                     ChooseNextPokemon(trainer, null, trainer.GetPokemonOnHandByIndex(action.Index));
    //                     switchMonSelected = true;
    //                 });
    //                 yield return new WaitUntil(() => switchMonSelected);
    //
    //                 bool roundEnded = false;
    //                 _roundController.StartRound(_turns.ToList(), (result) => {
    //                     roundEnded = true;
    //                     _turns.Clear();
    //                 });
    //
    //                 yield return new WaitUntil(() => roundEnded);
    //                 
    //                 pokemon = trainer.PokemonInBattle;
    //             }
    //
    //             _uiManager.SetMoveText(pokemon);
    //              
    //             bool selected = false;
    //             _uiManager.GetPlayerActionSelection_1v1(action => {
    //                 switch (action.Type)
    //                 {
    //                     case TurnActionType.Fight:
    //                         _turns.Add(new Turn(_turns.Count, pokemon, pokemon.GetMoveByIndex(action.Index), target));
    //                         break;
    //                     case TurnActionType.Pokemon:
    //                         _uiManager.OnPokemonSelectedFinished();
    //                         ChooseNextPokemon(trainer, pokemon, trainer.GetPokemonOnHandByIndex(action.Index));
    //                         break;
    //                     case TurnActionType.Items:
    //                         _uiManager.OnItemSelectedFinished();
    //                         _turns.Add(new Turn(_turns.Count, pokemon, new UseItemAction(trainer, trainer.GetItem(action.Name)), target));
    //                         break;
    //                     case TurnActionType.Run:
    //                         // TODO: handle trainer battle cannot run
    //                         _result = BattleResult.PlayerRan;
    //                         break;
    //                     default: break;
    //                 }
    //                 selected = true;
    //             });
    //
    //             yield return new WaitUntil(() => selected);
    //             _uiManager.OnRoundStart();
    //         }
    //         else
    //         {
    //             // TODO: improve enemy logic variations
    //             if (trainer != null && pokemon == null)
    //             {
    //                 ChooseNextPokemon(trainer, null, trainer.GetFirstAvailablePokemon());
    //             }
    //             else
    //             {
    //                 Move move = null;
    //                 if (trainer != null && trainer.TrainerModel is TrainerNPCModel model)
    //                 {
    //                     move = model.AI.GetMove(pokemon, target);
    //                 }
    //                 else
    //                 {
    //                     move = pokemon.GetRandomMove();
    //                 }
    //
    //                 _turns.Add(new Turn(_turns.Count, pokemon, move, target));
    //             }
    //         }
    //     }
    //
    //     if (_result != BattleResult.PlayerRan)
    //     {
    //         bool roundEnded = false;
    //         _roundController.StartRound(_turns.ToList(), result => {
    //             _result = result;
    //             roundEnded = true;
    //         });
    //
    //         yield return new WaitUntil(() => roundEnded);
    //     }
    //
    //     _turns.Clear();
    // }
    //
    // private void ChooseNextPokemon(Trainer trainer, Pokemon curr, Pokemon next)
    // {
    //     trainer.IChooseYou(next);
    //     _turns.Add(new Turn(_turns.Count, curr, new SwitchPokemon(() => {
    //         _uiManager.SetPokemon(trainer);
    //     }), next));
    // }
    //
    // private IEnumerator _EndBattle()
    // {
    //     switch (_result)
    //     {
    //         case BattleResult.Unresolved: Debug.LogError("Incorrect Battle Winning State! Battle shouldn't have ended!"); break;
    //         case BattleResult.PlayerWon:
    //             UIManager.SetBattleText($"You won!");
    //             yield return new WaitForSeconds(Constants.DIALOG_DURATION);
    //
    //             _fsm.ChangeState(GameState.Overworld.ToString());
    //             break;
    //         case BattleResult.PlayerLost:
    //             UIManager.SetBattleText("You have no more Pokemon that can fight!");
    //             yield return new WaitForSeconds(Constants.DIALOG_DURATION);
    //             // TODO: lose money
    //             UIManager.SetBattleText("You were overwhelmed by your defeat!");
    //             yield return new WaitForSeconds(Constants.DIALOG_DURATION);
    //
    //             _fsm.ChangeState(GameState.PlayerBlackOut.ToString());
    //             break;
    //         case BattleResult.WildPokemonCaught:
    //             _fsm.ChangeState(GameState.Overworld.ToString());
    //             break;
    //         case BattleResult.PlayerRan:
    //             UIManager.SetBattleText("You ran away safely!");
    //             yield return new WaitForSeconds(Constants.DIALOG_DURATION);
    //               
    //             _fsm.ChangeState(GameState.Overworld.ToString());
    //             break;
    //     }
    // }
    //
    // public override void ExitState()
    // {
    //     _uiManager.RemoveListener();
    //     _battleInfo.Clear();
    //     _fsm.battleScreen.alpha = 0f;
    // }
}

public class BattleInfo
{
    // 1v1 Only for simplicity sake
    // Wild pokemon will have null trainer value
    public Dictionary<Pokemon, Trainer> Pokemons = new Dictionary<Pokemon, Trainer>();
    public bool IsTrainerBattle => Pokemons.All(kvp => kvp.Value != null);

    public void Add(Pokemon pokemon)
    {
        Pokemons.Add(pokemon, pokemon.Trainer);
    }

    public KeyValuePair<Pokemon, Trainer> GetOpponent(Trainer trainer)
    {
        return Pokemons.FirstOrDefault(kvp => kvp.Value != trainer);
    }

    public void Clear()
    {
        foreach (var (pkmn, trainer) in Pokemons)
        {
            if (trainer == null) continue;

            // reset enemy health
            if (!trainer.IsPlayer) trainer.FullHealTeam();

            trainer.IChooseYou(null);
        }

        Pokemons.Clear();
    }
}

public enum BattleResult
{
    Unresolved,
    PlayerWon,
    PlayerLost,
    PlayerRan,
    WildPokemonCaught
}