using System.Collections;
using System.Collections.Generic;
using TurnBasedRPG.States;
using TurnBasedRPG.UI;
using UnityEngine;

namespace TurnBasedRPG
{
    public class BattleStateMachine : MonoStateMachine
    {
        public UIManager UIManager { get; private set; }
        public PlayerModel PlayerModel { get; private set; }
        public BattleResult BattleResult { get; set; }
        public BattleInfo BattleInfo { get; set; }
        protected override MonoState GetInitialState() => GetState(BattleState.NonBattle.ToString());

        public void Awake()
        {
            States.Add(BattleState.NonBattle.ToString(), new NonBattleState(this));
            States.Add(BattleState.EnterBattle.ToString(), new EnterBattleState(this));
        }
        
        public void Init(UIManager uiManager, PlayerModel playerModel)
        {
            UIManager = uiManager;
            PlayerModel = playerModel;
        }
        
        public void StartBattle(BattleInfo battleInfo)
        {
            BattleInfo = battleInfo;
            ChangeState(BattleState.EnterBattle.ToString());
        }
    }

    public enum BattleState
    {
        NonBattle,
        EnterBattle,
        PlayerDecision,
        OpponentDecision,
        PlayerTurn,
        OpponentTurn,
        PlayerChooseNextMon,
        OpponentChooseNextMon,
        ExitBattle
    }
}