using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_powerup : MonoBehaviour
{
    Image uiImage;

    // Start is called before the first frame update
    void Start()
    {
        uiImage = GetComponent<Image>();
        uiImage.enabled = false;
    }

    public void updatePowerUpUI(SpriteRenderer powerUpSprite)
    {
        if(powerUpSprite == null)
        {
            uiImage.enabled = false;
        }
        else
        {
            uiImage.sprite = powerUpSprite.sprite;
            uiImage.color = powerUpSprite.color;
            uiImage.enabled = true;
        }
    }
}
