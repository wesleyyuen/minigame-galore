using UnityEngine;
using System.Collections.Generic;
using Effectiveness = TurnBasedRPG.DAMAGE_EFFECTIVENESS;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "PokemonType", menuName = "ScriptableObjects/TurnBasedRPG/PokemonType")]
    public class PokemonType : ScriptableObject
    {
        public PokemonTypeName Name;
        [SerializeField] private List<PokemonTypeName> _strengths = new List<PokemonTypeName>();
        [SerializeField] private List<PokemonTypeName> _resistences = new List<PokemonTypeName>();
        [SerializeField] private List<PokemonTypeName> _immunities = new List<PokemonTypeName>();

        public Effectiveness GetDamageEffectiveness(PokemonSpecies attacker, PokemonSpecies defender, out float actualMultiplier)
        {
            float multiplier = 1f;
            Effectiveness effectiveness = Effectiveness.Neutral;

            // STAB
            if (Name == attacker.Type1?.Name || Name == attacker.Type2?.Name)
            {
                multiplier *= Constants.STAB_DAMAGE_MULTIPLIER;
            }

            // Type 1
            if (defender.Type1 != null)
            {
                if (_strengths.Contains(defender.Type1.Name))
                {
                    multiplier *= Constants.STRENGTH_DAMAGE_MULTIPLIER;
                    if (effectiveness != Effectiveness.Immune) effectiveness++;
                }
                else if (_resistences.Contains(defender.Type1.Name))
                {
                    multiplier *= Constants.RESIST_DAMAGE_MULTIPLIER;
                    if (effectiveness != Effectiveness.Immune) effectiveness--;
                }
                else if (_immunities.Contains(defender.Type1.Name))
                {
                    multiplier *= Constants.IMMUNE_DAMAGE_MULTIPLIER;
                    effectiveness = Effectiveness.Immune;
                }
            }

            // Type 2
            if (defender.Type2 != null)
            {
                if (_strengths.Contains(defender.Type2.Name))
                {
                    multiplier *= Constants.STRENGTH_DAMAGE_MULTIPLIER;
                    if (effectiveness != Effectiveness.Immune) effectiveness++;
                }
                else if (_resistences.Contains(defender.Type2.Name))
                {
                    multiplier *= Constants.RESIST_DAMAGE_MULTIPLIER;
                    if (effectiveness != Effectiveness.Immune) effectiveness--;
                }
                else if (_immunities.Contains(defender.Type2.Name))
                {
                    multiplier *= Constants.IMMUNE_DAMAGE_MULTIPLIER;
                    effectiveness = Effectiveness.Immune;
                }
            }

            actualMultiplier = multiplier;

            return effectiveness;
        }
    }
}