using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emote_text : MonoBehaviour
{
    public float timeToDestroy = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyText());
    }

    IEnumerator destroyText()
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
