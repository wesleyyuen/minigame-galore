using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrainerNPCAI : ScriptableObject
{
    public abstract Move GetMove(Pokemon pokemon, Pokemon target);
}