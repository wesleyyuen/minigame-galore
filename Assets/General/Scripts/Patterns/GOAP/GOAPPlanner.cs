using System.Linq;
using System.Collections.Generic;

public class GOAPPlanner
{
    public Queue<GOAPAction> Plan(List<GOAPAction> actions, Dictionary<string, int> goal, GOAPWorldStates world)
    {
        List<Node> leaves = new List<Node>();
        Node root = new Node(null, 0, GOAPWorld.Instance.World.States, null);

        var success = BuildGraph(root, leaves, actions, goal);

        if (!success) return null;
        
        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null || leaf.Cost < cheapest?.Cost) cheapest = leaf;
        }

        Queue<GOAPAction> plan = new Queue<GOAPAction>();
        Node lastStep = cheapest;
        while (lastStep != null)
        {
            if (lastStep.Action != null)
            {
                plan.Enqueue(lastStep.Action);
            }
            lastStep = lastStep.Parent;
        }

        return plan;
    }

    private bool BuildGraph(Node root, List<Node> leaves, List<GOAPAction> actions, Dictionary<string, int> goals)
    {
        bool pathFound = false;

        foreach (var action in actions)
        {
            if (action.IsAchievableGiven(root.State))
            {
                // Get State after current action
                Dictionary<string, int> currentStates = new Dictionary<string, int>(root.State);
                foreach (var (effectKey, effectValue) in action.Effects)
                {
                    currentStates.TryAdd(effectKey, effectValue);
                }

                Node next = new Node(root, root.Cost + action.Cost, currentStates, action);

                if (IsGoalAchieved(goals, currentStates))
                {
                    leaves.Add(next);
                    pathFound = true;
                }
                else
                {
                    bool found = BuildGraph(next, leaves, actions.Where(a => a != action).ToList(), goals);
                    if (found) pathFound = true;
                    // pathFound = BuildGraph(next, leaves, actions.Where(a => a != action).ToList(), goals);
                }
            }
        }

        return pathFound;
    }

    private bool IsGoalAchieved(Dictionary<string, int> goals, Dictionary<string, int> states)
    {
        foreach (var goal in goals)
        {
            if (!states.ContainsKey(goal.Key)) return false;
        }
        return true;
    }
}

public class Node
{
    public Node Parent {get; private set;}
    public float Cost {get; private set;}
    public Dictionary<string, int> State {get; private set;} = new Dictionary<string, int>();
    public GOAPAction Action {get; private set;}

    public Node(Node parent, float cost, Dictionary<string, int> state, GOAPAction action)
    {
        Parent = parent;
        Cost = cost;
        State = new Dictionary<string, int>(state);
        Action = action;
    }
}