public class GOAPWorld
{
    private static readonly GOAPWorld _instance = new GOAPWorld();
    public static GOAPWorld Instance => _instance;
    private GOAPWorldStates _world = new GOAPWorldStates();
    public GOAPWorldStates World => _world;
}
