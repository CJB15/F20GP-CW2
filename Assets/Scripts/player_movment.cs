using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movment : MonoBehaviour // This script is related too player movment
{
    public float player_speed = 4; // Player run speed
    public float player_jump_height = 6; // Player jump height

    public bool player_static = false; // Makes player unable to move, used in main menu

    private Animator thisAnim; // The animator the changes the animations

    CapsuleCollider2D ColliderPlayer;
    Rigidbody2D BodyPlayer;
    SpriteRenderer SpritePlayer;

    public player_camera movingCamera;

    RaycastHit2D hit;

    int layerMaskGround;
    float heightTestPlayer;

    bool isJumping = false; // Boolean that hold is player just jumped

    bool isStunned = false; // If player is stunend then they cannot move
    public int stunnedTime = 1; // How long the player is stunned for

    bool isDead = false; // Is player Dead

    bool isOnAntiGrav = false;
    bool isOnNormalGrav = false;
    bool gravityIsInverted = false;

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>(); // Gets the animator

        ColliderPlayer = GetComponent<CapsuleCollider2D>(); // Gets the players collision box, capsule shape used as square collision corners cause issues with slopes.
        BodyPlayer = GetComponent<Rigidbody2D>(); // Gets players rigib body
        SpritePlayer = GetComponent<SpriteRenderer>(); // Gets players sprite

        heightTestPlayer = ColliderPlayer.bounds.extents.y + 0.05f; //  Gets ...used in ray cast later on
        layerMaskGround = LayerMask.GetMask("Ground"); //  Ground objects are in teh "Ground" layer
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isStunned && !isDead && !player_static)
        {
            float inputX = Input.GetAxis("Horizontal"); // Gets users left or right input
            float inputY = Input.GetAxis("Vertical"); // Gets users up or down input

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
                hit = Physics2D.Raycast(ColliderPlayer.bounds.center, Vector2.up, heightTestPlayer, layerMaskGround); // Create a raycast pointing down
            }
            else
            {
                hit = Physics2D.Raycast(ColliderPlayer.bounds.center, Vector2.down, heightTestPlayer, layerMaskGround); // Create a raycast pointing down
            }
            
            bool isGrounded = (hit.collider != null); // If raycast hit's Ground layer object then is grounded, else not grounded.
            thisAnim.SetBool("Grounded", isGrounded); // If not grounded tells animtor to use falling animation, else use grounded animations
            //if(isGrounded){Debug.Log("ground");}
            if((BodyPlayer.velocity.y < 0.001 && !gravityIsInverted) || (BodyPlayer.velocity.y > 0.01 && gravityIsInverted)) // If player dosn't have positive verical velocity then they're jump has ended
            {
                //Debug.Log("stop");
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
                    if (gravityIsInverted)
                    {
                        Vector2 temp = BodyPlayer.velocity;
                        temp.y = -player_jump_height;
                        BodyPlayer.velocity = temp;

                        if(isOnNormalGrav)
                        {
                            BodyPlayer.gravityScale = 1;
                            transform.Rotate(180.0f, 0.0f, 0.0f, Space.World);
                            gravityIsInverted = false;
                            movingCamera.cameraGravitySwitch();
                        }
                    }
                    else if (!gravityIsInverted)
                    {
                        Vector2 temp = BodyPlayer.velocity;
                        temp.y = player_jump_height;
                        BodyPlayer.velocity = temp;

                        if(isOnAntiGrav)
                        {
                            BodyPlayer.gravityScale = -1;
                            transform.Rotate(180.0f, 0.0f, 0.0f, Space.World);
                            gravityIsInverted = true;
                            movingCamera.cameraGravitySwitch();
                        }
                    }
                    // TODO add jumping sound effect
                }
                
            }
            else if(!Input.GetButton("Jump") && isJumping) // If user releases the jump button while still traveing upwards
            {
                Vector2 temp = BodyPlayer.velocity;
                temp.y = 0;
                BodyPlayer.velocity = temp; // Stop jump immediatly by setting y velocity to 0
                isJumping = false; // Is no longer jumping
            }


            if (inputY < 0) // If user presses down
            {
                thisAnim.SetBool("Crouching", true); // Tell animator to use crouching animation
            }
            else // If not
            {
                thisAnim.SetBool("Crouching", false); // Tell animator to not use crouching animation
            } // Note: this dose nothing gameplay-wise it's just to use one of teh animations

            if (Input.GetKey("escape"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
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
            temp.y = -yAmount;
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

    public void setDead()
    {
        thisAnim.SetBool("Dead", true);
        isDead = true;
    }

    public void antiGravPlatform(bool on_or_off)
    {
        isOnAntiGrav = on_or_off;
    }

    public void normalGravPlatform(bool on_or_off)
    {
        isOnNormalGrav = on_or_off;
    }
}