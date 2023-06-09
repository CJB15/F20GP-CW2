using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class anti_gravity_platform : MonoBehaviour
{
    player_movment playermove; // Used to call function in player_movment

    // Start is called before the first frame update
    void Start()
    {
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in player_movment
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll) // If player steps on anti gravity pad
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            playermove.antiGravPlatform(true); // Flag the player movment script
        }
    }

    void OnCollisionExit2D(Collision2D coll) // If player steps off anti gravity pad
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            playermove.antiGravPlatform(false); // Flag the player movment script
        }
    }
}
