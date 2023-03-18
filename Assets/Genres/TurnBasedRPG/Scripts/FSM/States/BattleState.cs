using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TurnBasedRPG;
using TurnBasedRPG.UI;

public class BattleState : State
{
    protected GameStateMachine _fsm;
    protected UIManager _uiManager;
    protected RoundController _roundController;
    protected BattleInfo _battleInfo = new BattleInfo();
    protected List<Turn> _turns = new List<Turn>();
    protected BattleResult _result = BattleResult.Unresolved;

    public BattleState(
        GameStateMachine stateMachine,
        RoundController roundController) : base(stateMachine)
    {
        _fsm = stateMachine;
        _roundController = roundController;
        _uiManager = stateMachine.battleScreen.GetComponent<UIManager>();
        stateMachine.battleScreen.alpha = 0f;
    }

    protected IEnumerator _StartBattle()
    {
        _fsm.battleScreen.alpha = 1f;
        _result = BattleResult.Unresolved;
        _uiManager.OnBattleStart();

        while (_result == BattleResult.Unresolved)
        {
            bool roundEnded = false;
            _fsm.StartCoroutine(_ProcessRound(() => roundEnded = true));
            yield return new WaitUntil(() => roundEnded);
        }
 
        _fsm.StartCoroutine(_EndBattle());
    }

    protected IEnumerator _ProcessRound(Action callback)
    {
        // Getting all turns in this round
        foreach (var (pkmn, trainer) in _battleInfo.Pokemons)
        {
            var pokemon = trainer == null ? pkmn : trainer.PokemonInBattle;
            var targetKvp = _battleInfo.GetOpponent(trainer);
            var targetTrainer = targetKvp.Value;
            var target = targetTrainer == null ? targetKvp.Key : targetTrainer.PokemonInBattle;

            // Wait for player input
            if (trainer != null && trainer.IsPlayer)
            {
                // Wait for player to choose next mon
                if (pokemon == null)
                {
                    bool switchMonSelected = false;
                    _uiManager.GetPlayerPokemonSelection(action => {
                        _uiManager.OnPokemonSelectedFinished();
                        ChooseNextPokemon(trainer, null, trainer.GetPokemonOnHandByIndex(action.Index));
                        switchMonSelected = true;
                    });
                    yield return new WaitUntil(() => switchMonSelected);

                    bool roundEnded = false;
                    _roundController.StartRound(_turns.ToList(), (result) => {
                        roundEnded = true;
                        _turns.Clear();
                    });

                    yield return new WaitUntil(() => roundEnded);
                    
                    pokemon = trainer.PokemonInBattle;
                }

                _uiManager.SetMoveText(pokemon);
                 
                bool selected = false;
                _uiManager.GetPlayerActionSelection_1v1(action => {
                    switch (action.Type)
                    {
                        case TurnActionType.Fight:
                            _turns.Add(new Turn(_turns.Count, pokemon, pokemon.GetMoveByIndex(action.Index), target));
                            break;
                        case TurnActionType.Pokemon:
                            _uiManager.OnPokemonSelectedFinished();
                            ChooseNextPokemon(trainer, pokemon, trainer.GetPokemonOnHandByIndex(action.Index));
                            break;
                        case TurnActionType.Items:
                            _uiManager.OnItemSelectedFinished();
                            _turns.Add(new Turn(_turns.Count, pokemon, new UseItemAction(trainer, trainer.GetItem(action.Name)), target));
                            break;
                        case TurnActionType.Run:
                            _result = BattleResult.PlayerRan;
                            break;
                        default: break;
                    }
                    selected = true;
                });

                yield return new WaitUntil(() => selected);
                _uiManager.OnRoundStart();
            }
            else
            {
                // TODO: improve enemy logic variations
                if (trainer != null && pokemon == null)
                {
                    ChooseNextPokemon(trainer, null, trainer.GetFirstAvailablePokemon());
                }
                else
                {
                    Move move = null;
                    if (trainer != null && trainer.TrainerModel is TrainerNPCModel model)
                    {
                        move = model.AI.GetMove(pokemon, target);
                    }
                    else
                    {
                        move = pokemon.GetRandomMove();
                    }

                    _turns.Add(new Turn(_turns.Count, pokemon, move, target));
                }
            }
        }

        if (_result != BattleResult.PlayerRan)
        {
            bool roundEnded = false;
            _roundController.StartRound(_turns.ToList(), result => {
                _result = result;
                roundEnded = true;
            });

            yield return new WaitUntil(() => roundEnded);
        }

        _turns.Clear();
        callback?.Invoke();
    }

    protected void ChooseNextPokemon(Trainer trainer, Pokemon curr, Pokemon next)
    {
        trainer.IChooseYou(next);
        _turns.Add(new Turn(_turns.Count, curr, new SwitchPokemon(() => {
            _uiManager.SetPokemon(trainer);
        }), next));
    }

    protected IEnumerator _EndBattle()
    {
        switch (_result)
        {
            case BattleResult.Unresolved: Debug.LogError("Incorrect Battle Winning State! Battle shouldn't have ended!"); break;
            case BattleResult.PlayerWon:
                UIManager.SetBattleText($"You won!");
                yield return new WaitForSeconds(Constants.DIALOG_DURATION);

                _fsm.ChangeState(_fsm.GetState(GameState.Overworld));
                break;
            case BattleResult.PlayerLost:
                UIManager.SetBattleText("You have no more Pokemon that can fight!");
                yield return new WaitForSeconds(Constants.DIALOG_DURATION);
                // TODO: lose money
                UIManager.SetBattleText("You were overwhelmed by your defeat!");
                yield return new WaitForSeconds(Constants.DIALOG_DURATION);

                _fsm.ChangeState(_fsm.GetState(GameState.PlayerBlackOut));
                break;
            case BattleResult.WildPokemonCaught:
                _fsm.ChangeState(_fsm.GetState(GameState.Overworld));
                break;
            case BattleResult.PlayerRan:
                UIManager.SetBattleText("You ran away safely!");
                yield return new WaitForSeconds(Constants.DIALOG_DURATION);
                  
                _fsm.ChangeState(_fsm.GetState(GameState.Overworld));
                break;
        }
    }

    public override void ExitState()
    {
        _uiManager.RemoveListener();
        _battleInfo.Clear();
        _fsm.battleScreen.alpha = 0f;
    }

    protected class BattleInfo
    {
        // 1v1 Only for simplicity sake
        // Wild pokemon will have null trainer value
        public Dictionary<Pokemon, Trainer> Pokemons = new Dictionary<Pokemon, Trainer>();

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
                trainer?.IChooseYou(null);
            }

            Pokemons.Clear();
        }
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