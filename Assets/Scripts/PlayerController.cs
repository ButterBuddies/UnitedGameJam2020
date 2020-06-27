using System;
using UnityEngine;

// needs rigidbody!
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 5.0f;
    public float jump = 5.0f;
    public int maxJumps = 1;
    int jumpCount = 0;
    public LayerMask JumpMask;
    //public string horizontalVar = "Horizontal";
    //public string jumpVar = "Jump";
    public bool playerOne = true;
    public PhysicsMaterial2D Stop;
    public PhysicsMaterial2D Moving;

    // Set the position of how we can pick up the object.... oooo how are we going to do the opposite side??
    public Transform HoldingTransformation;
    //private Vector3 holdingOffset;
    public Transform PickupPosition;
    //private Vector3 pickupOffset;
    public LayerMask PickupMask;

    public bool isFaceRight = true;
    private bool faceRight = true;

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
    private float orgHeight;

    private void Start()
    {
        // set metadata for the offset at start.
        //pickupOffset = PickupPosition.position;
        //holdingOffset = HoldingTransformation.position;

        jumpCount = maxJumps;
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        orgHeight = this.transform.localScale.y;

        if (!isFaceRight)
        {
            FlipSprite();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOne)
        {
            dir = Input.GetAxis("Horizontal");
            if(Input.GetButtonDown("Jump"))
            {
                if(jumpCount > 0)
                {
                    Jump();
                }
            }
            Crouch(Input.GetButton("Crouch"));
            //Jump(Input.GetButtonDown("Jump"));
        }
        else
        {
            dir = Input.GetAxis("HorizontalYang");
            if(Input.GetButtonDown("JumpYang"))
            {
                if(jumpCount > 0)
                {
                    Jump();
                }
            }
            Crouch(Input.GetButton("CrouchYang"));
            //Jump(Input.GetButtonDown("JumpYang"));
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

            // nice to have : Make it "squish" when picking up object. Affect sprite only
            //Vector3 scale = this.transform.localScale;
            //scale.y = crouchHeight;
            //this.transform.localScale = scale;

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
                        // skip itself.
                        if (h.transform == PickupPosition)
                            continue;
                        PickupObject po = h.transform.GetComponent<PickupObject>();
                        // only pick up when it's a tag as pickupObject instead.
                        if ( po != null )
                        {
                            holding = h.collider.gameObject;
                            holding.transform.position = HoldingTransformation.position;
                            Rigidbody2D temp = holding.GetComponent<Rigidbody2D>();
                            if (temp != null)
                            {
                                // create a temp holder in case we need to let go of these blocks
                                holdingConstraintSettings = temp.constraints;
                                holdingfreezeRotation = temp.freezeRotation;

                                // freeze the blocks and have it parent ot the object.
                                temp.constraints = RigidbodyConstraints2D.FreezeAll;
                                temp.freezeRotation = true;
                            }
                            break;
                        }
                    }
                }
        }

        }
        else if( !v && isCrouching )
        {
            isCrouching = false;
            //Vector3 scale = this.transform.localScale;
            //scale.y = orgHeight;
            //this.transform.localScale = scale;
        }
    }
    
    private void Jump()
    {
        
        GetComponent<Rigidbody2D>().velocity = transform.up * 10;
        jumpCount -= 1;
        /*if( canJump && v )
        {

            //rb.AddForce(Vector3.up * rb.gravityScale * jump, ForceMode2D.Impulse);
            //canJump = false;
        }*/
    }

    public void FixedUpdate()
    {

        #region Movement

        if( dir < 0 && faceRight )
        {
            faceRight = !faceRight;
            FlipSprite();
        }
        else if( dir > 0 && !faceRight )
        {
            faceRight = !faceRight;
            FlipSprite();
        }

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

    private void FlipSprite()
    {
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "LevelGround")
        {
            jumpCount = maxJumps;
        }
    }

    // once you leave the object, then set it to null..
    /*public void OnCollisionExit2D(Collision2D collision)
    {
        if( col == collision )
            col = null;
    }*/
}
