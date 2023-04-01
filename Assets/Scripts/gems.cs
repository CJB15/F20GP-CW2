using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gems : MonoBehaviour // This script is the functions for teh collecable gems
{
    player_collectable player; // Used to call function in player_collectable
    CircleCollider2D ColliderGem;
    Animator thisAnim; // Holds the gems animator

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_collectable>(); // Used to call function in player_collectable
        ColliderGem = GetComponent<CircleCollider2D>();
        thisAnim = GetComponent<Animator>(); // Gets the animator
    }

    void OnTriggerEnter2D(Collider2D coll) // If somthing collides with a gem
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            player.addGem(1); // Call a function in player_collectable to increase their gem count by 1
            Destroy(ColliderGem);
            thisAnim.SetBool("Collected", true);
            StartCoroutine(gemCollected());
            // TODO Add some cool sound effects here
        }
    }

    IEnumerator gemCollected()
    {
        yield return new WaitForSeconds(0.333f);
        Destroy(gameObject); // Destroy the gem
    }
}
