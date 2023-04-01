using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movment : MonoBehaviour // This script is related too player movment
{
    public float player_speed = 4; // Player run speed
    public float player_jump_height = 6; // Player jump height

    public bool player_static = false; // Makes player unable to move, used in main menu

    Animator thisAnim; // Holds the animator the changes the animations

    CapsuleCollider2D ColliderPlayer; // Holds the players collider
    Rigidbody2D BodyPlayer; // Holds players rigib body
    SpriteRenderer SpritePlayer; // Holds players sprite

    public player_camera movingCamera; // Holds the camera that follows the player, needs to be set in the inspector

    RaycastHit2D hit; // Holds what, if anything, the raycast hit
    int layerMaskGround; // Holds the layer that the raycst will check for
    float heightTestPlayer; // Holds length of raycast

    bool isJumping = false; // Boolean that hold is player just jumped

    bool isStunned = false; // If player is stunend then they cannot move
    public int stunnedTime = 1; // How long the player is stunned for

    bool isDead = false; // Is player Dead

    bool isOnAntiGrav = false; // Is the player on the anti gravity pad
    bool isOnNormalGrav = false; // Is the player on the normal gravity pad
    bool gravityIsInverted = false; // Is the player's gravity current inverted

    public bool doubleJumpAbility = false; // Dose the user have the double jump ability, make true when they have the ability
    bool canDoubleJump = false; // Can the user current double jump

    bool holdingJump = false; // Is the user still holding the jump button after first jumping, this stops the double jump activating without pressing the button again

    public float inputX;
    public float inputY;

    bool crouching = false;

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>(); // Gets the animator

        ColliderPlayer = GetComponent<CapsuleCollider2D>(); // Gets the players collision box, capsule shape used as square collision corners cause issues with slopes.
        BodyPlayer = GetComponent<Rigidbody2D>(); // Gets players rigib body
        SpritePlayer = GetComponent<SpriteRenderer>(); // Gets players sprite

        heightTestPlayer = ColliderPlayer.bounds.extents.y + 0.05f; // Gets the length of the raycast
        layerMaskGround = LayerMask.GetMask("Ground"); //  Ground objects are in the "Ground" layer, raycast looks for this
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isStunned && !isDead && !player_static)
        {
            inputX = Input.GetAxis("Horizontal"); // Gets users left or right input
            inputY = Input.GetAxis("Vertical"); // Gets users up or down input

            if (inputX > 0) // If user gives right imput
            {
                thisAnim.SetBool("Moving", true); // Enable player to moving animation
                SpritePlayer.flipX = false; // Sprite is redered default, facing right
            }
            else if (inputX < 0) // If user gives left imput
            {
                //Debug.Log("Left");
                thisAnim.SetBool("Moving", true); // Enable player to moving animation
                SpritePlayer.flipX = true; // Sprite is redered mirrored, facing left
            }
            else // If player not inputing either
            {
                thisAnim.SetBool("Moving", false); // Disbale player to not moving animation
            }

            transform.Translate(((player_speed * inputX) * Time.deltaTime), 0, 0); // Convert user left/right input to left/right movment
            
            if(gravityIsInverted)
            {
                hit = Physics2D.Raycast(ColliderPlayer.bounds.center, Vector2.up, heightTestPlayer, layerMaskGround); // Create a raycast pointing up if gravity is inverted
            }
            else
            {
                hit = Physics2D.Raycast(ColliderPlayer.bounds.center, Vector2.down, heightTestPlayer, layerMaskGround); // Create a raycast pointing down if gravity is normal
            }
            
            bool isGrounded = (hit.collider != null); // If raycast hit's Ground layer object then is grounded, else not grounded.
            thisAnim.SetBool("Grounded", isGrounded); // If not grounded tells animtor to use falling animation, else use grounded animations
            
            if(isGrounded && doubleJumpAbility)
            {
                canDoubleJump = true; // If the player is on the ground and has the double jump ability re-enable the ability
            }

            if((BodyPlayer.velocity.y < 0.001 && !gravityIsInverted) || (BodyPlayer.velocity.y > 0.01 && gravityIsInverted)) // If player isn't moving away from the point they jumped away from
            {
                thisAnim.SetBool("Jumping", false); // Tell animator to not use jumping animaton
                isJumping = false; // Flag that the jump has ended
            }
            
            //if (inputY > 0 && isGrounded  && !isJumping) // This version uses the up arrow instead of the jump button
            if (Input.GetButton("Jump") && isGrounded && !isJumping) // If the user presses the jump button while grounded and not already jumping
            { // Note: isJumping is checked to ensure that the player dosn't jump on multiplr frames before leaving the ground, as that results in random and extreme jumps
                if(true)
                {
                    thisAnim.SetBool("Jumping", true); // Tell the animator to use the jumping animation
                    isJumping = true; // Flag that the player is jumping
                    holdingJump = true; // Flag that the user is currenlt holding the jump button
                    if (gravityIsInverted)
                    {
                        Vector2 temp = BodyPlayer.velocity;
                        temp.y = -player_jump_height;
                        BodyPlayer.velocity = temp; // If gravity is inverted, set players y velocity to their jump height but negative

                        if(isOnNormalGrav) // If they are on a normal gravity pad then set gravity to normal and flip player
                        {
                            BodyPlayer.gravityScale = 1; // set gravity to normal
                            transform.Rotate(180.0f, 0.0f, 0.0f, Space.World); // flip player
                            gravityIsInverted = false; // flag that gravity is not inverted
                            movingCamera.cameraGravitySwitch(); // Flip camera position on the y axis
                        }
                    }
                    else if (!gravityIsInverted)
                    {
                        Vector2 temp = BodyPlayer.velocity;
                        temp.y = player_jump_height;
                        BodyPlayer.velocity = temp; // If gravity is not inverted, set players y velocity to their jump height

                        if(isOnAntiGrav) // If they are on a normal gravity pad then set gravity to normal and flip player
                        {
                            BodyPlayer.gravityScale = -1; // invert gravity
                            transform.Rotate(180.0f, 0.0f, 0.0f, Space.World); // flip player
                            gravityIsInverted = true; // flag that gravity is inverted
                            movingCamera.cameraGravitySwitch(); // Flip camera position on the y axis
                        }
                    }
                    // TODO add jumping sound effect
                }
                
            }
            else if(Input.GetButton("Jump") && !isGrounded && !isJumping && canDoubleJump && !holdingJump) // If the player has jumped, can double jump but is not holding the jumbutton but has pressed the button again
            {
                canDoubleJump = false; // Set the player to not be able to double jump
                isJumping = true; // Sets the player to be jumping again
                thisAnim.SetBool("Jumping", true); // Play jumping anaimation
                
                if (gravityIsInverted)
                {
                    Vector2 temp = BodyPlayer.velocity;
                    temp.y = -player_jump_height;
                    BodyPlayer.velocity = temp; // If gravity is inverted, set players y velocity to their jump height but negative
                }
                else
                {
                    Vector2 temp = BodyPlayer.velocity;
                    temp.y = player_jump_height;
                    BodyPlayer.velocity = temp; // If gravity is not inverted, set players y velocity to their jump height
                }
            }
            else if(!Input.GetButton("Jump") && isJumping) // If user releases the jump button while still traveing upwards
            {
                Vector2 temp = BodyPlayer.velocity;
                temp.y = 0;
                BodyPlayer.velocity = temp; // Stop jump immediatly by setting y velocity to 0
                isJumping = false; // Is no longer jumping
                holdingJump = false; // Is not holding the jump button
            }
            else if(!Input.GetButton("Jump")) // If the user just isnt holding the jump button
            {
                holdingJump = false; // Is not holding the jump button
            }


            if (inputY < 0) // If user presses down
            {
                thisAnim.SetBool("Crouching", true); // Tell animator to use crouching animation
                crouching = true;
            }
            else // If not
            {
                thisAnim.SetBool("Crouching", false); // Tell animator to not use crouching animation
                crouching = false;
            }

            if (Input.GetKey("escape")) // Is esc button is pressed
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu"); // Return to main menu
            }
        }
    }

    public void playerKnockBack(int xAmount, int yAmount) // Can be called by other scripts to push players
    {
        Vector2 temp = BodyPlayer.velocity;
        temp.x = -xAmount;
        temp.y = yAmount;
        BodyPlayer.velocity = temp;
    }

    public void playerHurtKnockBack(int xAmount, int yAmount, string direction) // Called by player_health to push player specificaly when they takes damage
    {
        if(direction == "Right") // Knock the player left if hit from right
        {
            Vector2 temp = BodyPlayer.velocity;
            temp.x = -xAmount;
            temp.y = yAmount;
            BodyPlayer.velocity = temp;
        }
        else // Knock the player right if hit from the left
        {
            Vector2 temp = BodyPlayer.velocity;
            temp.x = xAmount;
            temp.y = yAmount;
            BodyPlayer.velocity = temp;
        }
        StartCoroutine(stunned()); // Stun the player
    }

    IEnumerator stunned()
    {
        thisAnim.SetBool("Stunned", true);
        isStunned = true; // Set the flag to true
        yield return new WaitForSeconds(stunnedTime);
        thisAnim.SetBool("Stunned", false);
        isStunned = false; // Set the flag to false
    }

    public void setDead() // Player is dead
    {
        thisAnim.SetBool("Dead", true);
        isDead = true;
    }

    public void antiGravPlatform(bool on_or_off)
    {
        isOnAntiGrav = on_or_off; // When player steps on anti grav pad, flag that
    }

    public void normalGravPlatform(bool on_or_off)
    {
        isOnNormalGrav = on_or_off; // When player steps on normal grav pad, flag that
    }

    public void doubleJumpActive(bool active)
    {
        doubleJumpAbility = active;
        canDoubleJump = active;
    }

    public bool getCrouching()
    {
        return crouching;
    }
}