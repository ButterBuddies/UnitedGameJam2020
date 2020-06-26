using UnityEngine;
using UnityEngine.Events;

// Must have Collider2D!
[RequireComponent(typeof(Collider2D))]
public class ButtonTrigger : MonoBehaviour
{
    public bool IsUnlocked = true;
    public UnityEvent OnTouch;
    [SerializeField]
    public LayerMask Filter;
    public bool ShowDebug = false;

    // invoke when is touched
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // Get the collider layer mask
        int colMask = collision.gameObject.layer;
        // if both layermask matches the filter and is unchecked, invoke.
        if (colMask == (colMask | (1 << Filter)) || (IsUnlocked))
        {
            // Invoke the event set.
            OnTouch.Invoke();
            // Debug logs in case we need to see it working.
            if( ShowDebug)
            {
                Debug.Log($"{this.gameObject.name} -> OnTouched");
            }
        }
    }
}
