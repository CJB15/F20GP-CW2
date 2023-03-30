using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class main_menu : MonoBehaviour
{
    public Toggle godToggle;
    public Toggle demiGodToggle;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("GodMode") == 1) // Turn on toggle if god mode is already on
        {
            godToggle.isOn = true;
        }
        else
        {
            godToggle.isOn = false;
        }

        if(PlayerPrefs.GetInt("DemiGodMode") == 1) // Turn on toggle if demi-god mode is already on
        {
            demiGodToggle.isOn = true;
        }
        else
        {
            demiGodToggle.isOn = false;
        }
    }

    public void PlayLevel(int LevelNo) // Is called by the level buttons, parses an int to indicate which button
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level " + LevelNo); // Loads the level, TODO: currently just loads SampleScene, change when more levels are made
    }

    public void TestWorld() // Is called by the level buttons, parses an int to indicate which button
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene"); // Loads the level, TODO: currently just loads SampleScene, change when more levels are made
    }

    public void QuitGame()
    {
        Application.Quit(); // Quits the application, only works in built version
    }

    public void GodMode() // Toggle on or off god mode
    {
        if(godToggle.isOn)
        {
            PlayerPrefs.SetInt("GodMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("GodMode", 0);
        }
    }

    public void DemiGodMode() // Toggle on or off demi-god mode
    {
        if(demiGodToggle.isOn)
        {
            PlayerPrefs.SetInt("DemiGodMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DemiGodMode", 0);
        }
    }
}
