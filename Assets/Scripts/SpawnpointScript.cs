using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnpointScript : MonoBehaviour
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PointActive()

    {
        SpawnpointScript[] spawns = FindObjectsOfType<SpawnpointScript>();
        // Debug.Log(spawns);

        foreach (SpawnpointScript sp in spawns)
        {
            sp.PointInactive();
        }

        // Debug.Log(gameObject.name);

        render.color = active;
    }

    public void PointInactive()
    {
        render.color = inactive;
    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if(obj.gameObject.tag == "Player")
        {
            Debug.Log("Collided with Spawn");
            FindObjectOfType<player_health>().changeSpawn(transform.position);
            PointActive();
        }
    }


}
