namespace TurnBasedRPG
{
    public static class Constants
    {
        public const int MAX_POKEMON_ON_HAND = 6;
        public const int MAX_MOVE_COUNT = 4;
        public const int ATTACK_MOVE_PRIORITY = 10;
        public const int PRIORITY_MOVE_PRIORITY = 10000;

        public const float CRITICAL_HIT_MULTIPLIER = 1.5f;
        public const float CRITICAL_HIT_CHANCE_PER_STAGE = 0.1f;

        // Damage Multipliers
        public const float DEFAULT_DAMAGE_MULTIPLIER = 1f;
        public const float IMMUNE_DAMAGE_MULTIPLIER = 0f;
        public const float STAB_DAMAGE_MULTIPLIER = 1.5f;
        public const float STRENGTH_DAMAGE_MULTIPLIER = 2f;
        public const float RESIST_DAMAGE_MULTIPLIER = 0.5f;

        public const float CHARACTER_MOVEMENT_DURATION = 0.15f;
        public const float RANDOM_ENCOUNTER_CHANCE = 15f; 
        public const float DIALOG_DURATION = 1.5f;
    }

    public enum TurnActionType
    {
        Fight,
        Pokemon,
        Items,
        Run
    }

    public enum DAMAGE_EFFECTIVENESS
    {
        Immune,
        Resist,
        Neutral,
        Super
    }

    public enum PokemonSpeciesName
    {
        Bulbasaur,
        Squirtle,
        Charmander
    }

    public enum PokemonTypeName
    {
        Null,
        Normal,
        Grass,
        Water,
        Fire
    }
}