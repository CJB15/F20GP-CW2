using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_pickup : MonoBehaviour // This script is the functions for teh collecable gems
{
    player_health player; // Used to call function in player_health
    CircleCollider2D ColliderCherry;
    Animator thisAnim; // Holds the gems animator

    bool isCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>(); // Used to call function in player_health
        ColliderCherry = GetComponent<CircleCollider2D>();
        thisAnim = GetComponent<Animator>(); // Gets the animator
    }

    void OnTriggerStay2D(Collider2D coll) // If somthing collides with a gem
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            bool healed = player.playerHeal(1);

            if (healed)
            {
                isCollected = true;
                Destroy(ColliderCherry);
                thisAnim.SetBool("Collected", true);
                StartCoroutine(cherryCollected());
            }
            // TODO Add some cool visual and sound effects here
        }
    }

    IEnumerator cherryCollected()
    {
        yield return new WaitForSeconds(0.333f);
        Destroy(gameObject); // Destroy the gem
    }
}
