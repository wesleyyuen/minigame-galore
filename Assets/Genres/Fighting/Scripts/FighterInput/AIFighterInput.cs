using System.Collections.Generic;
using UnityEngine;
using Fighting;

[CreateAssetMenu(fileName = "AIFighterInput", menuName = "ScriptableObjects/Fighting/FighterInput/AIFighterInput")]
public class AIFighterInput : FighterInput
{
    // [SerializeField] private float jumpProbability;
    private FighterStateMachine _player;

    private void OnEnable()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.TryGetComponent<FighterStateMachine>(out var fsm))
        {
            _player = fsm;
        }
    }

    public override void Update()
    {
        if (HasJumpInput()) Jump();
        if (HasAttackInput()) Attack();
        if (HasBlockInput()) Block();
    }

    public override Vector2 GetDirectionalInputVector()
    {
        return Vector2.zero;
        return new Vector2(Random.Range(-1f, 1f), 0f);
    }

    public override bool HasJumpInput()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }

    public override bool HasAttackInput()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }

    public override bool HasBlockInput()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }
}
