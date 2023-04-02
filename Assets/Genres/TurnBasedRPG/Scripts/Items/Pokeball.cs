using UnityEngine;
using Cysharp.Threading.Tasks;
using TurnBasedRPG.UI;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "Pokeball", menuName = "ScriptableObjects/TurnBasedRPG/Items/Pokeball")]
    public class Pokeball : Item
    {
        [SerializeField] private float _catchMultiplier;
        public float CatchMultiplier => _catchMultiplier;

        public override async UniTask<BattleResult> Use(Trainer user, Pokemon owner, Pokemon target)
        {
            if (target.Trainer != null)
            {
                await UIManager.Instance.SetBattleText("You cannot catch someone's pokemon!");
            }
            else
            {
                float targetHealthPercent = target.CurrentHP / target.Stat.HP;
                if (UnityEngine.Random.Range(0, 1) < (1 - targetHealthPercent))
                {
                    user.AddCreature(target);
                    await UIManager.Instance.SetBattleText($"You caught {target.Name}!");
                    return BattleResult.WildPokemonCaught;
                }
                else
                {
                    await UIManager.Instance.SetBattleText($"{target.Name} broke free!");
                }
            }

            return BattleResult.Unresolved;
        }
    }
}
