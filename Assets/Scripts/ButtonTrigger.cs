using UnityEngine;
using UnityEngine.Events;

// Must have Collider2D!
[RequireComponent(typeof(Collider2D))]
public class ButtonTrigger : MonoBehaviour
{
    public bool IsUnlocked = true;
    public UnityEvent EnterTouch;
    public UnityEvent ExitTouch;
    public UnityEvent ExitAll;
    [SerializeField]
    public LayerMask Filter;
    public bool ShowDebug = false;

    private int counter = 0;
    private AudioSource audio;

    public void Start()
    {
        audio = GetComponent<AudioSource>();
    }

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
            audio.Play();
            counter++;
            // Debug logs in case we need to see it working.
            if ( ShowDebug)
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
            counter--;
            // If all object are no longer interacting with the button then we need to invoke that all entity has left the object.
            if (counter == 0)
                OnTriggerExitAll2D();
            // Debug logs in case we need to see it working.
            if (ShowDebug)
            {
                Debug.Log($"{this.gameObject.name} -> Exitted touch");
            }
        }
    }

    public void OnTriggerExitAll2D()
    {
        ExitAll.Invoke();
        audio.Play();
    }
}
