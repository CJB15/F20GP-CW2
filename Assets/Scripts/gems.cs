using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gems : MonoBehaviour // This script is the functions for teh collecable gems
{
    private player_collectable player; // Used to call function in player_collectable

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_collectable>(); // Used to call function in player_collectable
    }

    void OnTriggerEnter2D(Collider2D coll) // If somthing collides with a gem
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            player.addGem(1); // Call a function in player_collectable to increase their gem count by 1
            Destroy(gameObject); // Destroy the gem
            // TODO Add some cool visual and sound effects here
        }
    }
}
