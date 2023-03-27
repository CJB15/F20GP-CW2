using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    player_health playerhp; // Used to call function in player_health
    player_movment playermove; // Used to call function in player_movment

    CapsuleCollider2D[] ColliderEnemy; // Holds the enimes 2 colliders
    Rigidbody2D bodyEnemy; // Holds the enimes rigidbody

    public int enemyDamage = 1; //  Damage the enemy dose to the player
    
    public int yKnockback = 4; // Amount the player is puched after jumping on the enemy

    public float yCoordHeadStop = 0.5f; // If the players origin is this much higher (greater y coord) than the enemy then the collision is ocunted as a head stomp

    public float patrolTime = 2; // How long each phase of the patrol cycle lasts for
    public float enemyWalkSpeed = 2; // How fast the enemy moves

    private Animator thisAnim; // Holds the enemies animator

    int patrolStage = 0; // Stage of the patrol cycle

    public GameObject GemObject; // Creates a new gameobject, used to create a gem

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        playerhp = GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>(); // Used to call function in player_health
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in player_movment

        ColliderEnemy = GetComponents<CapsuleCollider2D>(); // Gets both colliders, ColliderEnemy[0] detects collison with the player to damage them and ColliderEnemy[1] is the physical collider that stops the player clipping into the enemy
        bodyEnemy = GetComponent<Rigidbody2D>(); // Gets the rigidbody
        thisAnim = GetComponent<Animator>(); // Gets the animator

        InvokeRepeating("patrolCycle", 0, patrolTime); // Starts the patrol cycle
    }

    // Update is called once per frame
    void FixedUpdate() // Depending on the stage of the patrol cycle the enemy moves left, right or idles
    {
        if(isDead)
        {
            // Do nothing
        }
        else if(patrolStage == 0)
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
                isDead = true;
                Destroy(bodyEnemy);
                Destroy(ColliderEnemy[0]);
                Destroy(ColliderEnemy[1]);
                GetComponent<SpriteRenderer>().flipX = false;
                thisAnim.SetBool("Dead", true);
                StartCoroutine(enemyDead());
                playermove.playerKnockBack(0, yKnockback); // Knock the play upwards
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

    IEnumerator enemyDead()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject); // Kill enemy
        GameObject newGem = Instantiate(GemObject, this.transform.position, this.transform.rotation); // Leave gem behind
    }

}
