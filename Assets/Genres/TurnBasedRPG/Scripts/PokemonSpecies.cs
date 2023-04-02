using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBasedRPG
{
    [CreateAssetMenu(fileName = "PokemonSpecies", menuName = "ScriptableObjects/TurnBasedRPG/PokemonSpecies")]
    public class PokemonSpecies : ScriptableObject
    {
        [SerializeField] private PokemonSpeciesName _name;
        public PokemonSpeciesName Name => _name;
        [SerializeField] private PokemonType _type1;
        public PokemonType Type1 => _type1;
        [SerializeField] private PokemonType _type2;
        public PokemonType Type2 => _type2;
        [SerializeField] private PokemonStat _baseStat;
        public PokemonStat BaseStat => _baseStat; // TODO: make to base stats
        [SerializeField] private List<Move> _moveset;
        public List<Move> Moveset => _moveset.ToList();
        [SerializeField] private GameObject _sprite;
        public GameObject Sprite => _sprite;
    }

    [System.Serializable]
    public class PokemonStat
    {
        public float HP;
        public float HPPerLevel = 1f;
        public float Attack;
        public float AttackPerLevel = 1f;
        public float Defense;
        public float DefensePerLevel = 1f;
        public float Speed;
        public float SpeedPerLevel = 1f;

        public PokemonStat Clone()
        {
            return (PokemonStat) this.MemberwiseClone();
        }

        public void ScaleToLevel(int level)
        {
            HP += level * HPPerLevel;
            Attack += level * AttackPerLevel;
            Defense += level * DefensePerLevel;
            Speed += level * SpeedPerLevel;
        }
    }
}