using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikes : MonoBehaviour
{
    public int spikeDMG = 1;
    public player_health playerhp; // Used to call function in player_health
    public player_movment playermove; // Used to call function in player_movment
    // Start is called before the first frame update
    void Start()
    {
        playerhp = GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>(); // Used to call function in player_health
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in player_movment
 
    }


      public void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (!collision.GetComponent<enemy>()){
            
        
        if (playerhp != null)
        {
           playerhp.playerDamage(spikeDMG, "Left");
           
        }
        }
    }

}
