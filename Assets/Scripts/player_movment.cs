using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movment : MonoBehaviour // This script is related too player movment
{
    Rigidbody2D rb;
    public float player_speed = 4; // Player run speed
    public float player_jump_height = 10; // Player jump height

    public bool player_static = false; // Makes player unable to move, used in main menu

    private Animator thisAnim; // The animator the changes the animations

    CapsuleCollider2D ColliderPlayer;
    int layerMaskGround;
    float heightTestPlayer;

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

    // Start is called before the first frame update
    void Start()
    {
        thisAnim = GetComponent<Animator>(); // Gets the animator
        rb = GetComponent<Rigidbody2D>();

        ColliderPlayer = GetComponent<CapsuleCollider2D>(); // Gets the players collision box, capsule shape used as square collision corners cause issues with slopes.
        heightTestPlayer = ColliderPlayer.bounds.extents.y + 0.05f; //  Gets ...used in ray cast later on
        layerMaskGround = LayerMask.GetMask("Ground"); //  Ground objects are in teh "Ground" layer
        wallLayer = LayerMask.GetMask("Wall");
        pa = GetComponent<Player_attack>();

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
            }
            else if (inputX < 0) // If user gives left imput
            {
                //Debug.Log("Left");
                thisAnim.SetBool("Moving", true); // Enable player to moving animation
                GetComponent<SpriteRenderer>().flipX = true; // Sprite is redered mirrored, facing left
                facingRight = false; //sets facing right to false as the last input will direct the player to the left
            }
            else // If player not inputing either
            {
                thisAnim.SetBool("Moving", false); // Disbale player to not moving animation
            }

            transform.Translate(((player_speed * inputX) * Time.deltaTime), 0, 0); // Convert user left/right input to left/right movment
            
            RaycastHit2D hit = Physics2D.Raycast(ColliderPlayer.bounds.center, Vector2.down, heightTestPlayer, layerMaskGround); // Create a raycast pointing down
            bool isGrounded = hit.collider != null; // If raycast hit's Ground layer object then is grounded, else not grounded.
            if (isGrounded){
                dashCounter = 1;
                wallJumpCounter =0;
            }
            thisAnim.SetBool("Grounded", isGrounded); // If not grounded tells animtor to use falling animation, else use grounded animations

            if(GetComponent<Rigidbody2D>().velocity.y < 0.01) // If player dosn't have positive verical velocity then they're jump has ended
            {
                thisAnim.SetBool("Jumping", false); // Tell animator to not use jumping animaton
                isJumping = false; // Flag that the jump has ended
            } 
            
            //if (inputY > 0 && isGrounded  && !isJumping) // This version uses the up arrow instead of the jump button
            if (Input.GetButton("Jump") && isGrounded && !isJumping) // If the user presses the jump button while grounded and not already jumping
            { // Note: isJumping is checked to ensure that the player dosn't jump on multiplr frames before leaving the ground, as that results in random and extreme jumps
                thisAnim.SetBool("Jumping", true); // Tell the animator to use the jumping animation
                isJumping = true; // Flag that the player is jumping
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, player_jump_height), ForceMode2D.Impulse); // Apply a force upwards
                // TODO add jumping sound effect
            }
            else if(!Input.GetButton("Jump") && isJumping) // If user releases the jump button while still traveing upwards
            {
                Vector2 temp = GetComponent<Rigidbody2D>().velocity;
                temp.y = 0;
                GetComponent<Rigidbody2D>().velocity = temp; // Stop jump immediatly by setting y velocity to 0
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
        GetComponent<Rigidbody2D>().AddForce(new Vector2(xAmount, yAmount), ForceMode2D.Impulse); // Apply a specified force to the player
    }

    public void playerHurtKnockBack(int xAmount, int yAmount, string direction) // Called by player_health to push player specificaly when they takes damage
    {
        if(direction == "Right") // Knock the player left if hit from right
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-xAmount, yAmount), ForceMode2D.Impulse); // Apply a force to knock player backwards
        }
        else // Knock the player right if hit from the left
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(xAmount, yAmount), ForceMode2D.Impulse); // Apply a force to knock player backwards
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

    public void setDead()
    {
        thisAnim.SetBool("Dead", true);
        isDead = true;
    }



}