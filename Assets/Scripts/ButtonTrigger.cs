using UnityEngine;
using UnityEngine.Events;

// Must have Collider2D!
[RequireComponent(typeof(Collider2D))]
public class ButtonTrigger : MonoBehaviour
{
    public bool IsUnlocked = true;
    public UnityEvent EnterTouch;
    public UnityEvent ExitTouch;
    [SerializeField]
    public LayerMask Filter;
    public bool ShowDebug = false;

    // invoke when is touched
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("touched");
        // Get the collider layer mask
        int colMask = collision.gameObject.layer;
        // if both layermask matches the filter and is unchecked, invoke.
        if (colMask == (colMask | (1 << Filter)) || (IsUnlocked))
        {
            // Invoke the event set.
            EnterTouch.Invoke();
            // Debug logs in case we need to see it working.
            if( ShowDebug)
            {
                Debug.Log($"{this.gameObject.name} -> OnTouched");
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("untouched");
        // Get the collider layer mask
        int colMask = collision.gameObject.layer;
        // if both layermask matches the filter and is unchecked, invoke.
        if (colMask == (colMask | (1 << Filter)) || (IsUnlocked))
        {
            // Invoke the event set.
            ExitTouch.Invoke();
            // Debug logs in case we need to see it working.
            if (ShowDebug)
            {
                Debug.Log($"{this.gameObject.name} -> Exitted touch");
            }
        }
    }
}
