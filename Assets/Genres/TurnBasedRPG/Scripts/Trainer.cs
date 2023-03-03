using System;
using System.Linq;
using System.Collections.Generic;
using TurnBasedRPG;

public class Trainer : IEquatable<Trainer>
{
    public string Name {get;}
    public bool IsPlayer {get; private set;}
    public bool IsBlackedOut => !PokemonsOnHand.Any(pokemon => !pokemon.IsFainted);
    public Pokemon PokemonInBattle {get; private set;}
    public List<Pokemon> PokemonsOnHand {get;} = new List<Pokemon>();
    public Dictionary<Item, int> Inventory {get;} = new Dictionary<Item, int>();
    public TrainerModel TrainerModel {get;}

    // TODO: better way to distinguish player
    public Trainer(string name, TrainerModel model, bool isPlayer = false)
    {
        Name = name;
        TrainerModel = model;
        IsPlayer = isPlayer;
    }

    public void AddCreature(Pokemon pokemon)
    {
        if (PokemonsOnHand.Contains(pokemon)) return;

        pokemon.SetTrainer(this);

        if (PokemonsOnHand.Count < Constants.MAX_POKEMON_ON_HAND)
        {
            PokemonsOnHand.Add(pokemon);
        }
    }

    public void IChooseYou()
    {
        var first = GetFirstAvailablePokemon();
        PokemonInBattle = first;
    }

    public void IChooseYou(Pokemon pokemon)
    {
        PokemonInBattle = PokemonsOnHand.Contains(pokemon) ? pokemon : null;
    }

    public Pokemon GetPokemonOnHandByIndex(int index)
    {
        if (PokemonsOnHand.Count < index + 1) return null;

        return PokemonsOnHand[index] ?? null;
    }

    public Pokemon GetFirstAvailablePokemon()
    {
        return PokemonsOnHand.FirstOrDefault(pokemon => !pokemon.IsFainted);
    }

    public void ConsumeItem(Item item)
    {
        if (Inventory.ContainsKey(item))
        {
            Inventory[item]--;
            if (Inventory[item] <= 0)
            {
                Inventory.Remove(item);
            }
        }
    }

    public void AddItem(Item item, int amount = 1)
    {
        if (Inventory.ContainsKey(item))
        {
            Inventory[item] += amount;
        }
        else
        {
            Inventory[item] = amount;
        }
    }

    public Item GetItem(string name)
    {
        return Inventory.Keys.FirstOrDefault(item => item.Name == name);
    }

    public bool Equals(Trainer other)
    {
        if (other == null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetHashCode() != other.GetHashCode();
    }
}