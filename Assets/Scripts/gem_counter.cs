using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gem_counter : MonoBehaviour
{
    private TMP_Text gemText; // Createds a Text Mesh Pro object

    public void updateGemCount(int gem_count) // Called by player_collectable to update the gem count
    {
        gemText = GetComponent<TMP_Text>(); // Gets the TMP object current being used

        if(gem_count < 10)
        {
            gemText.text =  "00" + gem_count; // Updates the text of the TMP UI element with the gem count
        }
        else if(gem_count < 100)
        {
            gemText.text =  "0" + gem_count; // Updates the text of the TMP UI element with the gem count
        }
        else
        {
            gemText.text =  "" + gem_count; // Updates the text of the TMP UI element with the gem count
        }
    }
}
