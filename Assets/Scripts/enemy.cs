using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    player_health playerhp; // Used to call function in player_health
    player_movment playermove; // Used to call function in player_movment

    CapsuleCollider2D ColliderEnemy;

    public int enemyDamage = 1; //  Damage the enemy dose to the player
    
    public int yKnockback = 4; // Amount the player is puched after jumping on the enemy

    public float yCoordHeadStop = 0.5f; // If the players origin is this much higher (greater y coord) than the enemy then the collision is ocunted as a head stomp

    public float patrolTime = 2; // How long each phase of the patrol cycle lasts for
    public float enemyWalkSpeed = 2; // How fast the enemy moves

    private Animator thisAnim;

    int patrolStage = 0;

    public GameObject GemObject;

    // Start is called before the first frame update
    void Start()
    {
        playerhp = GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>(); // Used to call function in player_health
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in player_movment

        ColliderEnemy = GetComponent<CapsuleCollider2D>(); // Gets the enimes collision box, capsule shape used as square collision corners cause issues with slopes.

        thisAnim = GetComponent<Animator>(); // Gets the animator

        InvokeRepeating("patrolCycle", 0, patrolTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(patrolStage == 0)
       {
            GetComponent<SpriteRenderer>().flipX = false;
            thisAnim.SetBool("Walking", true);

            transform.Translate((-enemyWalkSpeed * Time.deltaTime), 0, 0);
       }
       else if(patrolStage == 1)
       {
            GetComponent<SpriteRenderer>().flipX = false;
            thisAnim.SetBool("Walking", false);
       }
       else if(patrolStage == 2)
       {
            GetComponent<SpriteRenderer>().flipX = true;
            thisAnim.SetBool("Walking", true);

            transform.Translate((enemyWalkSpeed * Time.deltaTime), 0, 0);
       }
       else if(patrolStage == 3)
       {
            GetComponent<SpriteRenderer>().flipX = true;
            thisAnim.SetBool("Walking", false);
       }
       
    }

    void OnTriggerStay2D(Collider2D coll) // If somthing collides with the enemy
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            if ((coll.transform.position.y - this.transform.position.y) > yCoordHeadStop) // If the enemy is hit from the top
            {
                playermove.playerKnockBack(0, yKnockback); // Knock the play upwards
                Destroy(gameObject); // Kill enemy
                GameObject newGem = Instantiate(GemObject, this.transform.position, this.transform.rotation); // Leave gem behind
            }
            else if ((coll.transform.position.x - this.transform.position.x) > 0) // If hit from left
            {
                playerhp.playerDamage(enemyDamage, "Left"); // Hurt player, push right
            }
            else // If hit from right
            {
                playerhp.playerDamage(enemyDamage, "Right"); // Hurt player, push left
            }
        }
    }

    void patrolCycle()
    {
        if(patrolStage == 0)
        {
            patrolStage = 1;
        }
        else if(patrolStage == 1)
        {
            patrolStage = 2;
        }
        else if(patrolStage == 2)
        {
            patrolStage = 3;
        }
        else if(patrolStage == 3)
        {
            patrolStage = 0;
        }
    }

}
