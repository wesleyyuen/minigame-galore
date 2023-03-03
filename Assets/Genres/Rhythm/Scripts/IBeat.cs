using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBeat : IPooledObject
{
    int Id {get; set;}
    BeatInfo Info {get; set;}
    float SecondsBehindBeat {get; set;}
}
