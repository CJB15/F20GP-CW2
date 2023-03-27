using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_collectable : MonoBehaviour // This script is holds call teh players collecables and the functions to increase them
{
    private gem_counter counter; // Used to call function in gem_counter

    public int gem_count; // The players total gem count
    
    // Start is called before the first frame update
    void Start()
    {
        gem_count = PlayerPrefs.GetInt("gem_count");

        counter = GameObject.FindGameObjectWithTag("Gem UI").GetComponent<gem_counter>(); // Used to call function in gem_counter
        counter.updateGemCount(gem_count); // Calls function in gem_counter, sets the gem count display to show current gem count
    }

    public void addGem(int amount) // Can be called to make player gain any amount of gems
    {
        gem_count = gem_count + amount; // The gem count increases by the passed amount
        
        counter.updateGemCount(gem_count); // Calls function in gem_counter, sets the gem count display to show current gem count
        
        PlayerPrefs.SetInt("gem_count", gem_count);
    }

    public void setGem(int amount)
    {
        gem_count = amount;

        counter.updateGemCount(gem_count); // Calls function in gem_counter, sets the gem count display to show current gem count
        
        PlayerPrefs.SetInt("gem_count", gem_count);
    }
}