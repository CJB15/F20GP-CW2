using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal_gravity_platform : MonoBehaviour
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            playermove.normalGravPlatform(true);
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            playermove.normalGravPlatform(false);
        }
    }
}