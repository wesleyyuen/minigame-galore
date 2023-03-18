using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GOAPAgent : MonoBehaviour
{
    public List<GOAPAction> Actions = new List<GOAPAction>();
    public Dictionary<GOAPGoal, int> Goals = new Dictionary<GOAPGoal, int>();
    private GOAPPlanner _planner;
    private Queue<GOAPAction> _plan;
    private GOAPAction CurrentAction;
    private GOAPGoal CurrentGoal;
    private bool _isInvoked;

    private void Awake()
    {
        var allActions = GetComponents<GOAPAction>();
        foreach (var a in allActions) Actions.Add(a);
    }

    private void LateUpdate()
    {
        // Wait for current action to complete
        if (CurrentAction != null && CurrentAction.IsRunning)
        {
            if (Vector2.Distance(transform.position, CurrentAction.Target.transform.position) < CurrentAction.Range)
            {
                if (!_isInvoked)
                {
                    StartCoroutine(DoAction());
                }
            }

            return;
        }

        // Get a new plan
        if (_planner == null || _plan == null)
        {
            _planner = new GOAPPlanner();

            // Try to plan for highest priority goal first
            var sortedGoals = from entry in Goals orderby entry.Value descending select entry;
            foreach (var (goal, priority) in sortedGoals)
            {
                _plan = _planner.Plan(Actions, new Dictionary<string, int>(){{goal.Key, goal.Value}}, null);
                if (_plan != null)
                {
                    CurrentGoal = goal;
                    break;
                }
            }
        }

        if (_plan != null)
        {
            if (_plan.Count <= 0)
            {
                if (CurrentGoal.ShouldRemove)
                {
                    Goals.Remove(CurrentGoal);
                }

                _planner = null;
            }
            else
            {
                CurrentAction = _plan.Dequeue();
                // Start next action in plan
                if (CurrentAction.PreAction() && CurrentAction.Target != null)
                {
                    CurrentAction.IsRunning = true;
                    // TODO: move towards target
                }
                else
                {
                    // replan
                    _plan = null;
                }
            }
        }
    }

    private IEnumerator DoAction()
    {
        _isInvoked = true;

        yield return StartCoroutine(CurrentAction.DoAction());

        CurrentAction.IsRunning = false;
        CurrentAction.PostAction();
        _isInvoked = false;
    }
}

public class GOAPGoal
{
    public string Key {get; private set;}
    public int Value {get; private set;}
    public bool ShouldRemove {get; private set;}

    public GOAPGoal(string key, int value, bool shouldRemove)
    {
        Key = key;
        Value = value;
        ShouldRemove = shouldRemove;
    }
}