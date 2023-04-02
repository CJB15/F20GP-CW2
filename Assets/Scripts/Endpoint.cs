using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endpoint : MonoBehaviour

{
    public SpriteRenderer render;
    private Vector2 spawnpoint;
    public Color active;
    public Color inactive;


    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        spawnpoint = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        render.color = inactive;
        
    }

 

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            Debug.Log("Collided with end");
            render.color = active;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");


        }
    }


}


