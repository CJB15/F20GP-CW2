using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //public PowerUpEffect powerUpEffect;
    public int powerUpEffect;
    SpriteRenderer SpritePowerUp;

    player_powerups playerPowers;
    player_movment playermove;

    void Start()
    {
        playerPowers = GameObject.FindGameObjectWithTag("Player").GetComponent<player_powerups>();
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>();
        SpritePowerUp = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && playermove.getCrouching()) // If that thing is the player
        {
            Destroy(gameObject);
            playerPowers.setNewAbility(powerUpEffect, SpritePowerUp);
        }
    }
}
