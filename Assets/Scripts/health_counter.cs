using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class health_counter : MonoBehaviour
{
    private TMP_Text hpText; // Createds a Text Mesh Pro object

    public void updateHealthCount(int player_current_health, int player_max_health) // Called by player_health to update the health display count
    {
        hpText = GetComponent<TMP_Text>(); // Gets the TMP object current being used
        hpText.text =  "" + player_current_health + "/" + player_max_health; // Updates the text of the TMP UI element with the players current health out of the players max health
    }
}
