using System;
using System.Linq;
using UnityEngine;
using TurnBasedRPG;

[Serializable]
public class Pokemon : IComparable<Pokemon>
{
    public Guid Guid {get;}
    [SerializeField] private PokemonSpecies _species;
    public PokemonSpecies Species {get => _species; private set => _species = value;}
    [SerializeField] private string _name;
    public string Name {
        get => _name ?? _species?.Name.ToString() ?? "Unknown";
        private set => _name = value;
    }
    [SerializeField] private int _level = 1;
    public int Level {get => _level; private set => _level = Math.Clamp(value, 1, 100);}
    public PokemonStat Stat {get; private set;}
    public float CurrentHP {get; private set;}
    public Move[] CurrentMoves {get; private set;}
    public Trainer Trainer {get; private set;}
    public event Action<Pokemon> Event_TookDamage;
    public event Action<Pokemon> Event_KnockedOut;
    public bool IsFainted => CurrentHP <= 0f;

    public Pokemon(PokemonSpecies species, int level, string name = null)
    {
        Guid = Guid.NewGuid();
        _species = species;
        _name = name ?? species.Name.ToString();
        _level = level;
        Stat = species.BaseStat.Clone();
        Stat.ScaleToLevel(level);
        CurrentHP = Stat.HP;

        // fill first 4 moves
        CurrentMoves = species.Moveset.Take(Constants.MAX_MOVE_COUNT).ToArray();
    }

    public void TakeDamage(float damage, Pokemon dealer)
    {
        if (IsFainted) return;
        
        damage = Mathf.Max(0, damage + dealer.Stat.Attack - Stat.Defense);
        CurrentHP = Mathf.Max(0, CurrentHP - damage);
        Event_TookDamage?.Invoke(this);

        if (IsFainted)
        {
            if (Trainer != null) Trainer.IChooseYou(null);
            Event_KnockedOut?.Invoke(this);
        }
    }

    public void Heal(float heals)
    {
        CurrentHP = Mathf.Max(CurrentHP + heals, Stat.HP);
    }

    public void SetTrainer(Trainer trainer)
    {
        Trainer = trainer;
    }

    public Move GetMoveByIndex(int index)
    {
        if (index < Constants.MAX_MOVE_COUNT)
        {
            return index < CurrentMoves.Length ? CurrentMoves[index] : null;
        }
        
        return null;
    }

    public Move GetRandomMove()
    {
        Move move = CurrentMoves[UnityEngine.Random.Range(0, CurrentMoves.Length - 1)];
        do
        {
            move = CurrentMoves[UnityEngine.Random.Range(0, CurrentMoves.Length - 1)];
        } while (move == null);
        return move;
    }

    public int CompareTo(Pokemon other)
    {
        if (other == null) return 1;
        return this.Guid.CompareTo(other.Guid);
    }
}