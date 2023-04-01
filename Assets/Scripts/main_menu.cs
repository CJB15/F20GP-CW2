using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class main_menu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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
        Application.Quit();
    }

    public void GodMode(bool status) // TODO dose not work, set manually in inspector
    {
        if(status)
        {
            PlayerPrefs.SetInt("GodMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("GodMode", 0);
        }
    }

    public void DemiGodMode(bool status) // TODO dose not work, set manually in inspector
    {
        if(status)
        {
            PlayerPrefs.SetInt("DemiGodMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DemiGodMode", 0);
        }
    }
}
