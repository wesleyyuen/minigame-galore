using UnityEngine;
using Fighting;

public class BattlefieldOffscreenKill : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<FighterStateMachine>(out var target))
        {
            target.KnockOut();
        }
    }
}
