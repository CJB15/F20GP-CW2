using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class menu_backround_scaler : MonoBehaviour
{
    Image bkgrImage;
    RectTransform rt;

    public int menu_width = 500;

    // Start is called before the first frame update
    void Start()
    {
        bkgrImage = GetComponent<Image>();
        rt = bkgrImage.rectTransform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rt.sizeDelta = new Vector2(menu_width, Screen.height); // Sets the gery backrunf to the main menu to be the height of the screen
    }
}
