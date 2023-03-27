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

    public void PlayLevel(int LevelNo)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        //Application.Quit();
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
