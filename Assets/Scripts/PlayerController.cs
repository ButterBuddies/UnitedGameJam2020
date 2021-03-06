﻿using System;
using UnityEngine;

// needs rigidbody!
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerSounds sound;
    public float speed = 5.0f;
    public float jumpForce= 5.0f;
    public int maxJumps = 1;
    int jumpCount = 0;
    public LayerMask JumpMask;  // er?
    // invertly flips the controls
    public bool playerOne = true;

    // To control how the physics responds to the character movement.
    public PhysicsMaterial2D Stop;
    public PhysicsMaterial2D Moving;

    // Set the position of how we can pick up the object.... oooo how are we going to do the opposite side??
    public Transform HoldingTransformation;
    public Transform PickupPosition;
    public Transform DropoffPosition;

    public bool isFaceRight = true;
    private bool faceRight = true;

    // object used to hold.
    private PickupObject holding;
    private Rigidbody2D belowFeet;
    /// <summary>
    /// Return true if the player is holding object, false if none.
    /// </summary>
    private bool IsHolding => !(holding is null);
    
    private bool holdingfreezeRotation;
    private RigidbodyConstraints2D holdingConstraintSettings;

    private Rigidbody2D rb;
    private bool canJump = true;
    private float dir = 0;

    private Collision2D col;
    private bool isCrouching;
    public float crouchHeight = 0.5f;
    private float orgHeight;
    public float blockSafeDistanceCheck;

    private Animator anim;
    public float groundCheck = 1.0f;

    public bool ShowDebug = false;
    public LayerMask dropoffMask;

    private void Start()
    {
        // set metadata for the offset at start.
        //pickupOffset = PickupPosition.position;
        //holdingOffset = HoldingTransformation.position;
        anim = GetComponentInChildren<Animator>();
        
        jumpCount = maxJumps;
        rb = this.GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        orgHeight = this.transform.localScale.y;

        if (!isFaceRight)
        {
            FlipSprite();
        }
    }

    private void OnDrawGizmos()
    {
        if (!ShowDebug) return;

        Gizmos.color = canJump ? Color.green : Color.red;
        Gizmos.DrawRay(this.transform.position, Vector3.down * ( rb?.gravityScale ?? 1 ) * groundCheck);
    }

    public void SwapPlayerWorld()
    {
        playerOne = !playerOne;
        CheckGravityCondition();
        if (belowFeet != null)
            belowFeet = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerOne)
        {
            dir = Input.GetAxis("Horizontal");
            
            

            if (Input.GetButtonDown("Jump"))
            {
                //if(jumpCount > 0)
                //{
                    Jump();
                //}
            }
            Crouch(Input.GetButton("Crouch"));
        }
        else
        {
            dir = Input.GetAxis("HorizontalYang");

            if(Input.GetButtonDown("JumpYang"))
            {
                //if(jumpCount > 0)
                //{
                    Jump();
                //}
            }
            Crouch(Input.GetButton("CrouchYang"));
        }
    }

    /// <summary>
    /// Tell the player to pick up the object
    /// </summary>
    /// <param name="go"></param>
    /// <returns></returns>
    private bool PickupObject(GameObject go )
    {
        // safeguard in case we would try and pick up multiple of object?
        if (holding != null) return true;

        holding = go.GetComponent<PickupObject>();
        // only pick up when it's a tag as pickupObject instead.
        if (holding != null && holding.IsLock == false )
        {
            if(sound !=null )
                sound.PlayPickup();
            holding.IsLock = true;
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
            CheckGravityCondition();
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        // Hmm.
        float rayDir = Mathf.Clamp( this.transform.position.x - DropoffPosition.position.x, -1, 1 ) * blockSafeDistanceCheck;
        Gizmos.DrawRay(this.transform.position, Vector3.left * rayDir);
    }

    private bool DropoffObject()
    {
        if (holding is null) return false;

        float rayDir = Mathf.Clamp(this.transform.position.x - DropoffPosition.position.x, -1, 1) * blockSafeDistanceCheck;
        Debug.Log(rayDir);
        float rayDist = Vector2.Distance(this.transform.position, DropoffPosition.position);
        RaycastHit2D[] hit = Physics2D.RaycastAll(this.transform.position, Vector2.left * rayDir, rayDist, dropoffMask);
        Collider2D c = this.GetComponent<Collider2D>();
        bool isMirror = holding.GetComponent<MirrorBlock>() != null;


        // in this special case where we have a mirror block, also check for the mirror world for safe drop.
        if (isMirror)
        {
            Vector3 mirrorPos = this.transform.position;
            mirrorPos.y = -mirrorPos.y;
            RaycastHit2D[] mirrorHit = Physics2D.RaycastAll(mirrorPos, Vector2.left * rayDir, rayDist, dropoffMask);
            // I DETECT SOMETHING! PREVENT PUTTING DA BLOCK DOWN!!!
            foreach (var h in mirrorHit)
            {
                // THIS IS INTERESTING WE GOTTA CHECK IF THIS IS THE MIRROR BLOCK!!!
                if (h.collider == c)
                    continue;
                if (h)
                {
                    if (sound != null)
                        sound.PlayNegative();
                    return false;
                }
            }
        }

        // I DETECT SOMETHING! PREVENT PUTTING DA BLOCK DOWN!!!
        foreach (var h in hit)
        {
            // THIS IS INTERESTING WE GOTTA CHECK IF THIS IS THE MIRROR BLOCK!!!
            

            // this sounds expensive....
            if (h.collider == c)
                continue;
            if (h)
            {
                if (sound != null)
                    sound.PlayNegative();
                return false;
            }
        }

        if (sound != null)
            sound.PlayDrop();

        Vector3 pos = DropoffPosition?.position ?? PickupPosition.position;
         

        if ( holding.GetComponent<MirrorBlock>())
        {
            pos.y = this.transform.position.y;
            holding.transform.position = pos;
        }
        else
        {
            holding.transform.position = pos;
        }

        Rigidbody2D temp = holding.GetComponent<Rigidbody2D>();
        if (temp != null)
        {
            temp.freezeRotation = holdingfreezeRotation;
            temp.constraints = holdingConstraintSettings;
        }
        holding.IsLock = false;
        holding = null;
        CheckGravityCondition();

        return true;
    }

    private void CheckGravityCondition()
    {
        // If you are player one AND still holding block, then make sure your gravity is not affected by sarah's script.
        // in this case, the proper gravity scale should be set to 1. 
        if (!holding?.IsGreenBlock ?? false ) return;
        if( playerOne && IsHolding && rb.gravityScale < 0 )
        {
            // make sure that the rigidbody 
            rb.gravityScale = -rb.gravityScale;
        }
        
        // If you are not player one AND you are holding a block in your hand, but also the gravityscale is negative 1, set it to positive one by game design.
        // in this case the proper gravity scale should be set to -1 otherwise make it 1
        if( !playerOne && IsHolding && rb.gravityScale < 0 )
        {
            rb.gravityScale = -rb.gravityScale;
        }
        else if( !playerOne && !IsHolding && rb.gravityScale > 0 )
        {
            rb.gravityScale = -rb.gravityScale;
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
                    DropoffObject();
                }
                else
                {
                    RaycastHit2D[] hit = Physics2D.RaycastAll(PickupPosition.position, Vector2.up * rb.gravityScale, 0.01f);
                    foreach ( var h in hit )
                    {
                        // skip itself.
                        if (h.transform == PickupPosition) continue;
                        if (PickupObject(h.transform.gameObject)) break;
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
        if (!canJump) return;


        // avoid jumping when holding objects.
        if (IsHolding) return;
        //GetComponent<Rigidbody2D>().velocity = transform.up * 10;
        rb.AddForce(Vector3.up * rb.gravityScale * jumpForce, ForceMode2D.Impulse);
        if (sound != null)
            sound.PlayJump();
        //jumpCount -= 1;
        //canJump = false;   
    }

    /// <summary>
    /// Main controller movement.
    /// </summary>
    public void FixedUpdate()
    {

        anim.SetBool("IsDangling", false);
        anim.SetBool("IsSurfing", false);

        #region Movement

        if ( dir < 0 && faceRight )
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
        if (CanMove())
        {
            velocity.x = dir * speed;
            rb.velocity = velocity;
        }
        // If the player is upside down we want to move the block across as if it was surfing through the air...
        if( belowFeet != null )
        {
            //surfing
            anim.SetBool("IsSurfing", true);
            belowFeet.velocity = velocity;
        }
        // adjust the physical material to make the movement more smooth.
        rb.sharedMaterial = velocity.magnitude > 0 ? Moving : Stop;

        #endregion

        #region Holding update position

        if( holding != null )
        {
            if (!playerOne)
            {
                //riding
                anim.SetBool("IsDangling", true);
            }
            

            holding.transform.position = HoldingTransformation.position;
        }

        #endregion

        #region Check Ground

        // hmm?
        canJump = false;
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, Vector3.down * rb.gravityScale, groundCheck, JumpMask);
        foreach( var h in ray )
        {
            if (h.rigidbody      == rb) continue;
            // in this case if it's not the player itself, then we're obvioulsy touching the ground at this point..
            canJump = true;
        }

        #endregion

        #region Animation

        //???
        //anim.SetBool("IsWalking", false);
        //anim.SetBool("IsFalling", false);
        //if (dir != 0)
        //{
            anim.SetBool("IsWalking", dir != 0);
        //}

        anim.SetBool("IsFalling", !canJump);

        #endregion

        #region Sound
        
        // Blame sarah for not catching null exception
        if( sound != null )
            sound.PlayWalkingToggle( dir != 0 );  
        
        #endregion
    }

    private bool CanMove()
    {
        if (!playerOne && IsHolding && holding.IsGreenBlock)
            return false;
        return true;
        
    }

    private void FlipSprite()
    {
        Vector3 scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "LevelGround")
        //{
        //    //Debug.Log($"{this.gameObject.name} -> {collision.contacts[0].normal}");
        //    //if( ( playerOne && collision.contacts[0].normal == Vector2.up ) ||
        //    //    ( !playerOne && collision.contacts[0].normal == Vector2.down ) )
        //        jumpCount = maxJumps;
        //}

        // this will be interesting..
        // if the block hits the player from the above in player 1
        // Or if the block hits the player from the bottom in player 2
        // pick up the block anyway. 

        // for now.... this seems to only work when you're player 1 at the moment....
        if ( playerOne && !IsHolding)
        {
            // check and see where the collision hits if the collision was coming from the top... 
            // do some weird mumbo jumbo script ehre to check and see if the block did hit from the top and everything all goes well    
            if (collision.contacts[0].normal == Vector2.down)
            {
                PickupObject po = collision.gameObject.GetComponent<PickupObject>();
                if( po != null && po.IsGreenBlock )
                    PickupObject(collision.gameObject);
            }   
        }

        // This works great, but now we need to move the block if the player under the block's feet....
        //if ( !playerOne )
        //{
        //    // if the object is still colliding as normal vector2.down, then we need to apply physics motion as the player moves across...
        //    if ( collision.contacts[0].normal == Vector2.down && collision.collider.GetComponent<PickupObject>())
        //    {
        //        belowFeet = collision.rigidbody;
        //    }
        //}
    }       

    // once you leave the object, then set it to null..
    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.rigidbody == belowFeet)
        {
            belowFeet = null;
        }
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (!playerOne)
        {
            // if the object is still colliding as normal vector2.down, then we need to apply physics motion as the player moves across...
            if (collision.contacts[0].normal == Vector2.down && collision.collider.GetComponent<PickupObject>())
            {
                belowFeet = collision.rigidbody;
            }
        }
    }
}
