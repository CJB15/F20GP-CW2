using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_pickup : MonoBehaviour // This script is the functions for teh collecable gems
{
    private player_health player; // Used to call function in player_health

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>(); // Used to call function in player_health
    }

    void OnTriggerStay2D(Collider2D coll) // If somthing collides with a gem
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            bool healed = player.playerHeal(1);

            if (healed)
            {
                Destroy(gameObject);
            }
            // TODO Add some cool visual and sound effects here
        }
    }
}
