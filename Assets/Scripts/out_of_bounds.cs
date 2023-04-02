using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class out_of_bounds : MonoBehaviour // This script is the functions for teh collecable gems
{

    // Start is called before the first frame update
    // void Start()
    // {

    // }

    void OnTriggerEnter2D(Collider2D coll) // If somthing collides with a gem
    {
        if(coll.gameObject.tag == "Player") // If that thing is the player
        {
            player_health playerHp = coll.GetComponent<player_health>();
            if(playerHp.player_current_health <= 0){
                playerHp.playerDamage(1,"Left");
            }
            else{
                playerHp.player_current_health -=1;
                Debug.Log("player hp: "+ playerHp.player_current_health);
                playerHp.hpcounter.updateHealthCount(playerHp.player_current_health, playerHp.player_max_health);
                PlayerPrefs.SetInt("player_crrent_health",playerHp.player_current_health);
                StartCoroutine(playerHp.iFrames());
                playerHp.reSpawn();
            }
        }
    }

}
