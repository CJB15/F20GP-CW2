using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_health : MonoBehaviour // This script holds the players health and function to reduce it
{
    public int player_max_health = 6; // Players Max health
    public int player_current_health = 0; // Players current health, set below so alright to leav at 0

    public bool InvulnerabilityFrames = false; // Is the player temporarly invulenrable after taking damage
    public int iFrameLength = 2; // The seconds the player is immortal for after taking damage

    public bool GodMode = false; // Used for debugging, makes player unable to take damage
    public bool DemiGodMode = false; // Used for debugging, makes player unable to drop below 1 health

    public int xKnockback = 3;
    public int yKnockback = 4;

    public health_counter hpcounter; // Used to call function in health_counter
    public player_movment playermove; // Used to call function in player_movment
    public Renderer rend; // Used to disable the rederer later

    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        player_current_health = player_max_health; // Set players health to max

        hpcounter = GameObject.FindGameObjectWithTag("Health UI").GetComponent<health_counter>(); // Used to call function in player_movment
        hpcounter.updateHealthCount(player_current_health, player_max_health); // Calls function in health_counter, sets the health display to show current health
    
        rend = GetComponent<Renderer>();

        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in health_counter
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    public void playerDamage(int damage, string direction) // Can be called to make player take any amount of damage
    {
        if(!InvulnerabilityFrames && !GodMode && !isDead) // If not invulnerable, god mode or already dead, take damage
        {
            if(DemiGodMode && (player_current_health - damage) < 0) // If the player has demi god mode and is about to die, don't die
            {
                player_current_health = 1;
            }
            else
            {
                player_current_health = player_current_health - damage; // Reduce player health by damage
            }
            hpcounter.updateHealthCount(player_current_health, player_max_health);  // Calls function in health_counter, sets the health display to show current health

            if(player_current_health <= 0) // If player has no health
            {
                isDead = true; // Set them as dead
                playermove.setDead(); // Calls function in player_movment, stops them moving
                // TODO add more here
            }
            else
            {
                StartCoroutine(iFrames()); // Give the player invulnerability frames
            }
            playermove.playerHurtKnockBack(xKnockback, yKnockback, direction); // Knock the play backwards
        } // If invulenrable, godmode or dead do not take damage
    }

    IEnumerator iFrames()
    {
        InvulnerabilityFrames = true; // Set the flag to true
        for(var i = 0 ; i < iFrameLength * 4 ; i++) // Loop for the set amount of time
        {
            rend.enabled = false; // Flickering their dprite to indocate the immortality
            yield return new WaitForSeconds(0.125f);
            rend.enabled = true;
            yield return new WaitForSeconds(0.125f);
        }
        InvulnerabilityFrames = false; // Set the flag to false
    }
}
