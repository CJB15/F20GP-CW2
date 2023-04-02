using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movment : MonoBehaviour // This script is related too player movment
{
    Rigidbody2D rb;
    public float player_speed = 4; // Player run speed
    public float player_jump_height = 10; // Player jump height

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

    public bool facingRight = true; // player direction


    //dashing variables
    public bool canDash = true;
    public bool dashing;
    public float dashForce = 2f;
    public float dashTime = 0.4f;
    // public float dashInteval = 0.5f; //time before next dash, replaced with dashCounter which is set to 1 every time the ground is touched
    public int dashCounter;
    public TrailRenderer tr;

    //wall climbing variables
    LayerMask wallLayer;
    public bool sliding = false;

    Player_attack pa;
    public int wallJumpCounter = 0;
    public bool wallJumping;
    bool isOnAntiGrav = false; // Is the player on the anti gravity pad
    bool isOnNormalGrav = false; // Is the player on the normal gravity pad
    bool gravityIsInverted = false; // Is the player's gravity current inverted

    public bool doubleJumpAbility = false; // Dose the user have the double jump ability, make true when they have the ability
    bool canDoubleJump = false; // Can the user current double jump

    bool holdingJump = false; // Is the user still holding the jump button after first jumping, this stops the double jump activating without pressing the button again

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>(); // Gets the animator
        rb = GetComponent<Rigidbody2D>();

        ColliderPlayer = GetComponent<CapsuleCollider2D>(); // Gets the players collision box, capsule shape used as square collision corners cause issues with slopes.
        BodyPlayer = GetComponent<Rigidbody2D>(); // Gets players rigib body
        SpritePlayer = GetComponent<SpriteRenderer>(); // Gets players sprite
        wallLayer = LayerMask.GetMask("Wall");
        pa = GetComponent<Player_attack>();
        heightTestPlayer = ColliderPlayer.bounds.extents.y + 0.05f; // Gets the length of the raycast
        layerMaskGround = LayerMask.GetMask("Ground"); //  Ground objects are in the "Ground" layer, raycast looks for this
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isStunned && !isDead && !player_static && !dashing)
        {
            float inputX = Input.GetAxis("Horizontal"); // Gets users left or right input
            float inputY = Input.GetAxis("Vertical"); // Gets users up or down input

            // if (dashing){
            //     return;
            // }
            if (inputX > 0) // If user gives right imput
            {
                thisAnim.SetBool("Moving", true); // Enable player to moving animation
                GetComponent<SpriteRenderer>().flipX = false; // Sprite is redered default, facing right
                facingRight = true;  //sets facing right to true as the last input will direct the player to the right
                SpritePlayer.flipX = false; // Sprite is redered default, facing right
            }
            else if (inputX < 0) // If user gives left imput
            {
                //Debug.Log("Left");
                thisAnim.SetBool("Moving", true); // Enable player to moving animation
                GetComponent<SpriteRenderer>().flipX = true; // Sprite is redered mirrored, facing left
                facingRight = false; //sets facing right to false as the last input will direct the player to the left
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
            
            bool isGrounded = hit.collider != null; // If raycast hit's Ground layer object then is grounded, else not grounded.
            thisAnim.SetBool("Grounded", isGrounded); // If not grounded tells animtor to use falling animation, else use grounded animations
            
            if (isGrounded){
                dashCounter = 1;
                wallJumpCounter =0;
            }
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
            }
            else // If not
            {
                thisAnim.SetBool("Crouching", false); // Tell animator to not use crouching animation
            } // Note: this dose nothing gameplay-wise it's just to use one of teh animations

            if (Input.GetKey("escape")) // Is esc button is pressed
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu"); // Return to main menu
            }


            //dash code
            if(Input.GetKeyDown(KeyCode.L) && canDash && !dashing && dashCounter>0)
            {
                StartCoroutine(playerDash());
            }

            //wall sliding code
            if(touchedWall() && !isGrounded && inputX >= 0 && !wallJumping){
                sliding = true;
                wallJumpCounter =1;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -2, float.MaxValue));
            }else if(touchedWall() && !isGrounded && inputX < 0 && !wallJumping){
                sliding = true;
                wallJumpCounter =1;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -2, float.MaxValue));
            }else{
                sliding = false;
            }



            //wall JumpCode
            if(wallJumpCounter>0){
                if(Input.GetButtonDown("Jump")){
                    Debug.Log("Input x is: " + inputX);
                    wallJumping = true;
                    wallJumpCounter-=1;
                    if(Physics2D.OverlapCircle(pa.attackPointR.position, 0.5f,wallLayer)){
                        // rb.AddForce(new Vector2(5f, player_jump_height), ForceMode2D.Impulse);
                        Debug.Log("jumping left");
                        rb.velocity = new Vector2(player_jump_height *-1, player_jump_height);
                    }else if(Physics2D.OverlapCircle(pa.attackPointL.position, 0.5f,wallLayer)){
                        Debug.Log("jumping right");
                        // rb.AddForce(new Vector2(5f, player_jump_height), ForceMode2D.Impulse);
                        rb.velocity = new Vector2(player_jump_height, player_jump_height);
                    }
                    Invoke(nameof(stopWallJump), 0.5f);
                }
            }

        }
    }

    void stopWallJump(){
        wallJumping = false;
        rb.velocity = new Vector2(rb.velocity.x,rb.velocity.y);
    }

    bool touchedWall(){
        if(Physics2D.OverlapCircle(pa.attackPointR.position, 0.5f,wallLayer) 
        ||
        Physics2D.OverlapCircle(pa.attackPointL.position, 0.2f,wallLayer)){
            if(Physics2D.OverlapCircle(pa.attackPointR.position, 0.5f,wallLayer) ){
                // Debug.Log("detect right");
            }else if (Physics2D.OverlapCircle(pa.attackPointL.position, 0.2f,wallLayer)){
                // Debug.Log("detect left");
            }
            return true;
        }else{
            return false;
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

    IEnumerator playerDash(){
        canDash = false;
        dashing = true;
        dashCounter -=1;
        float initGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if(facingRight){
            rb.velocity  = new Vector2(transform.localScale.x * dashForce, 0f);
        }else{
            rb.velocity  = new Vector2((transform.localScale.x * -1.0f) * dashForce, 0f);
        }
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        tr.emitting = false;
        rb.gravityScale = initGravity;
        rb.velocity = new Vector2(0,0);
        dashing = false;
        // yield return new WaitForSeconds(dashInteval);
        canDash = true;

    }

    public void setDead() // Player is dead
    {
        thisAnim.SetBool("Dead", true);
        isDead = true;
    }

    public void setAlive() // Player is dead
    {
        thisAnim.SetBool("Dead", false);
        isDead = false;
    }



    public void antiGravPlatform(bool on_or_off)
    {
        isOnAntiGrav = on_or_off; // When player steps on anti grav pad, flag that
    }

    public void normalGravPlatform(bool on_or_off)
    {
        isOnNormalGrav = on_or_off; // When player steps on normal grav pad, flag that
    }
}