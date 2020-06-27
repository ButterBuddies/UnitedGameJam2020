using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Gate : MonoBehaviour
{
    private Collider2D col;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OpenGate()
    {
        col.enabled = false;
        sprite.enabled = false;
    }

    public void CloseGate()
    {
        Debug.Log("got here in close gate");
        col.enabled = true;
        sprite.enabled = true;
    }
    
}
