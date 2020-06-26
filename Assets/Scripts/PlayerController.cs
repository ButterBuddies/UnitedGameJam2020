using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// needs rigidbody!
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 1.0f;
    public float jump = 10.0f;
    public LayerMask JumpMask;

    public PhysicsMaterial2D Stop;
    public PhysicsMaterial2D Moving;

    private Rigidbody2D rb;
    private bool canJump = true;
    private float dir = 0;

    private Collision2D col;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        dir = Input.GetAxis("Horizontal");
        Jump(Input.GetButtonDown("Jump"));
    }

    private void Jump(bool v)
    {
        if( canJump && v )
        {
            rb.AddForce(Vector3.up * rb.gravityScale * jump, ForceMode2D.Impulse);
            //canJump = false;
        }
    }

    public void FixedUpdate()
    {

        #region Movement

        Vector2 velocity = rb.velocity;
        velocity.x = dir * speed;
        rb.velocity = velocity;
        // adjust the physical material to make the movement more smooth.
        rb.sharedMaterial = velocity.magnitude > 0 ? Moving : Stop;

        #endregion


        #region Check Ground

        //if ( col is null )
        //{
        //    canJump = false;
        //}
        //else
        //{
        //    // Hmm we need to be able to filter just the ground for this? Solve this by checking if the y velocity is zero
        //    int mask = col.gameObject.layer;
        //    // Mask matches
        //    if (mask == (mask | 1 << JumpMask))
        //    {
        //        canJump = true;
        //    }
        //}

        #endregion
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        col = collision;
    }

    // once you leave the object, then set it to null..
    public void OnCollisionExit2D(Collision2D collision)
    {
        if( col == collision )
            col = null;
    }
}
