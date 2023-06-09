using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_health : MonoBehaviour // This script holds the players health and function to reduce it
{
    public int player_max_health = 6; // Players Max health
    public int player_current_health = 0; // Players current health

    bool doubleHealth;

    bool InvulnerabilityFrames = false; // Is the player temporarly invulenrable after taking damage
    public int iFrameLength = 2; // The seconds the player is immortal for after taking damage

    bool GodMode = false; // Used for debugging, makes player unable to take damage
    bool DemiGodMode = false; // Used for debugging, makes player unable to drop below 1 health

    public int xKnockback = 3;
    public int yKnockback = 4;

    public health_counter hpcounter; // Used to call function in health_counter
    public player_movment playermove; // Used to call function in player_movment
    public player_collectable playergem;
    public Renderer rend; // Used to disable the rederer later

    bool isDead = false;



    //spawnpoint fields
    private Vector2 spawnpoint;
    private Vector2 initSpawn;

    // Start is called before the first frame update
    void Start()
    {
        //player_current_health = PlayerPrefs.GetInt("player_current_health"); // Sets their health to the saved value, uncomment if you wan tot save health between levels
        if(player_current_health <= 0) // If the saved value is 0 the set to max
        {
            player_current_health = player_max_health; // Set players health to max
        }
        
        hpcounter = GameObject.FindGameObjectWithTag("Health UI").GetComponent<health_counter>(); // Used to call function in player_movment
        hpcounter.updateHealthCount(player_current_health, player_max_health); // Calls function in health_counter, sets the health display to show current health
    
        rend = GetComponent<Renderer>();

        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in health_counter
        playergem = GameObject.FindGameObjectWithTag("Player").GetComponent<player_collectable>(); // Used to call function in health_counter
    
        if(PlayerPrefs.GetInt("GodMode") == 1)
        {
            GodMode = true;
        }

        if(PlayerPrefs.GetInt("DemiGodMode") == 1)
        {
            DemiGodMode = true;
        }


        initSpawn = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>().transform.position;
        spawnpoint = initSpawn ;
        Debug.Log("SpawnPoint: "+ spawnpoint);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }

    public bool playerHeal(int healAmount) // Can be called to heal the player by any amount
    {
        if (player_current_health < player_max_health) // If player is not at full health
        {
            player_current_health = player_current_health + healAmount; // Add the heal amount to the players health

            if(player_current_health > player_max_health) // If this put the player over their max health
            {
                player_current_health = player_max_health; // Set them to max health
            }

            hpcounter.updateHealthCount(player_current_health, player_max_health); // Calls function in health_counter, sets the health display to show current health 
            PlayerPrefs.SetInt("player_current_health", player_current_health); // Updates the saved health value
            return true; // Returns true to indicate they were healed
        }
        else
        {
            return false; // Return false to indicate the player wasn't healed
        }
    }

    public void playerDamage(int damage, string direction) // Can be called to make player take any amount of damage
    {
        if(!InvulnerabilityFrames && !GodMode && !isDead) // If not invulnerable, god mode or already dead, take damage
        {
            player_current_health = player_current_health - damage; // Reduce player health by damage

            if(player_current_health <= 0 && DemiGodMode) // If in demi god mode, dont let health drop below 1
            {
                player_current_health = 1;
            }
            
            if(player_current_health <= 0) // If player has no health
            {
                StartCoroutine(playerDead()); // Kill player
            }
            else // If they do have health
            {
                StartCoroutine(iFrames()); // Give the player invulnerability frames
            }

            hpcounter.updateHealthCount(player_current_health, player_max_health);  // Calls function in health_counter, sets the health display to show current health
            PlayerPrefs.SetInt("player_current_health", player_current_health); // Updates the saved health value
            playermove.playerHurtKnockBack(xKnockback, yKnockback, direction); // Knock the play backwards
        } // If invulenrable, godmode or dead do not take damage
    }

    public IEnumerator iFrames()
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

    IEnumerator playerDead()
    {
        isDead = true; // Set them as dead
        playermove.setDead(); // Calls function in player_movment, stops them moving
        yield return new WaitForSeconds(3);
        playergem.setGem(0); // Sets gemcount to 0
        changeSpawn(initSpawn);
        reSpawn();
        // UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu"); // Send the user back to the menu
    }


    public void reSpawn(){
        // playerDamage(1, "Left");
        if(isDead){
            isDead = false;
            playermove.setAlive();
            player_current_health = player_max_health;
            hpcounter.updateHealthCount(player_current_health, player_max_health);  // Calls function in health_counter, sets the health display to show current health
            PlayerPrefs.SetInt("player_current_health", player_current_health); // Updates the saved health value
            transform.position = spawnpoint;
        }
        else{
            transform.position = spawnpoint;
        }


    }
    public void changeSpawn(Vector2 position)
    {
        spawnpoint = position;
        Debug.Log("new Spawnpoint: "+spawnpoint);
    }

    public void doubleHealthActive(bool active)
    {
        if(!doubleHealth && active)
        {
            player_max_health = player_max_health * 2;
            player_current_health = player_current_health * 2;

            doubleHealth = true;
        }
        else if(doubleHealth && !active)
        {
            player_max_health = player_max_health / 2;
            player_current_health = (int)Mathf.Ceil(player_current_health / 2f);

            doubleHealth = false;
        }
        hpcounter.updateHealthCount(player_current_health, player_max_health);  // Calls function in health_counter, sets the health display to show current health
    }
}
