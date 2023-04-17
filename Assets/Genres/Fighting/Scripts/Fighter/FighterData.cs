using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Fighting
{
    [CreateAssetMenu(fileName = "FighterData", menuName = "ScriptableObjects/Fighting/FighterData")]
    public class FighterData : ScriptableObject
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed;
        public float MovementSpeed => _movementSpeed;

        [Header("Jump")]
        [SerializeField] private float _jumpVelocity;
        public float JumpVelocity => _jumpVelocity;
        [SerializeField] private float _lowJumpGravity;
        public float LowJumpGravity => _lowJumpGravity;
        [SerializeField] private float _jumpBufferTime;
        public float JumpBufferTime => _jumpBufferTime;
        [SerializeField] private float _coyoteTime;
        public float CoyoteTime => _coyoteTime;
    }
}