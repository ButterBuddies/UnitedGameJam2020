using System;
using UnityEngine;

// needs rigidbody!
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 1.0f;
    public float jump = 10.0f;
    public LayerMask JumpMask;
    //public string horizontalVar = "Horizontal";
    //public string jumpVar = "Jump";
    public bool playerOne = true;
    public PhysicsMaterial2D Stop;
    public PhysicsMaterial2D Moving;

    // Set the position of how we can pick up the object.... oooo how are we going to do the opposite side??
    public Transform HoldingTransformation;
    public Transform PickupPosition;
    public LayerMask PickupMask;

    // object used to hold.
    private GameObject holding;
    private bool holdingfreezeRotation;
    private RigidbodyConstraints2D holdingConstraintSettings;

    private Rigidbody2D rb;
    private bool canJump = true;
    private float dir = 0;

    private Collision2D col;
    private bool isCrouching;
    public float crouchHeight = 0.5f;
    private Vector3 orgScale;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        orgScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOne)
        {
            dir = Input.GetAxis("Horizontal");
            Jump(Input.GetButtonDown("Jump"));
            Crouch(Input.GetButton("Crouch"));
        }
        else
        {
            dir = Input.GetAxis("HorizontalYang");
            Jump(Input.GetButtonDown("JumpYang"));
            Crouch(Input.GetButton("CrouchYang"));
        }
        

    }

    private void Crouch(bool v)
    {
        // in this case we can just simply do two things. 
        // we'll squash the player animation
        // and then check and see if there's the object that we can pick up

        // needs a timer huh?
        if (v && !isCrouching)
        {
            isCrouching = true;

            Vector3 scale = this.transform.localScale;
            scale.y = crouchHeight;
            this.transform.localScale = scale;

            if (HoldingTransformation != null && PickupPosition != null)
            {
                if (holding != null)
                {
                    holding.transform.position = PickupPosition.position;
                    Rigidbody2D temp = holding.GetComponent<Rigidbody2D>();
                    if(temp != null )
                    {
                        temp.freezeRotation = holdingfreezeRotation;
                        temp.constraints = holdingConstraintSettings;
                    }
                    holding = null;
                }
                else
                {
                    RaycastHit2D[] hit = Physics2D.RaycastAll(PickupPosition.position, Vector2.up * 0.01f);
                    foreach ( var h in hit )
                    {
                        if (h.transform == PickupPosition)
                            continue;
                        //int mask = h.transform.gameObject.layer;
                        //if (mask == ( mask | 1 << PickupMask) )
                        //{
                        holding = h.collider.gameObject;
                        holding.transform.position = HoldingTransformation.position;
                        Rigidbody2D temp = holding.GetComponent<Rigidbody2D>();
                        if(temp != null )
                        {
                            // create a temp holder in case we need to let go of these blocks
                            holdingConstraintSettings = temp.constraints;
                            holdingfreezeRotation = temp.freezeRotation;

                            // freeze the blocks and have it parent ot the object.
                            temp.constraints = RigidbodyConstraints2D.FreezeAll;
                            temp.freezeRotation = true;
                        }
                        break;
                        //}
                    }
                }
        }

        }
        else if( !v && isCrouching )
        {
            isCrouching = false;
            this.transform.localScale = orgScale;
        }
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

        #region Holding update position

        if( holding != null )
        {
            holding.transform.position = HoldingTransformation.position;
        }

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
