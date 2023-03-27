using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class menu_backround_scaler : MonoBehaviour
{
    Image backgroundImage;
    RectTransform rt;
    float ratio;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        rt = backgroundImage.rectTransform;
        ratio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rt)
            return;

        rt.sizeDelta = new Vector2(500, Screen.height);
    }
}
