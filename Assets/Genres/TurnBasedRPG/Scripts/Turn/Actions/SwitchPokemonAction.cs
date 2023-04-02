using System;
using Cysharp.Threading.Tasks;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    public class SwitchPokemon : ITurnAction
    {
        // Switching Always goes first
        public int Priority => int.MaxValue;
        private Action _uiCallback;

        public SwitchPokemon(Action uiCallback)
        {
            _uiCallback = uiCallback;
        }

        public async UniTask<BattleResult> DoAction(Pokemon owner, Pokemon target)
        {
            if (owner != null)
            {
                await UIManager.Instance.SetBattleText($"Come back, {owner.Name}!");
            }

            _uiCallback?.Invoke();
            await UIManager.Instance.SetBattleText($"Go, {target.Name}!");

            return BattleResult.Unresolved;
        }
    }
}