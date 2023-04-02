using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackgroundScript : MonoBehaviour
{
    // BoxCollider2D col;
    public int counter = 0;
    // public int highestScreen = 0;
    public Vector3 pos;
    public List<Vector3> posList;
    public float backgroundwidth;
    public float offset;
    public float zAxis = 4;

    GameObject player;
    public bool tried = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        // col = GetComponent<BoxCollider2D>();
        backgroundwidth = transform.localScale.x;
        player = GameObject.FindGameObjectWithTag("Player");
        offset = (backgroundwidth/2) - Mathf.Abs(player.transform.position.x - pos.x);

        // posList.Add(pos);
        // highestScreen++;

        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 playerPos = player.transform.position;

        // if (posList.Count == 0){
        //     return;
        // }
        if(playerPos.x - offset >= pos.x && !tried){
            counter++;
            if(counter>posList.Count){
                // highestScreen++;
                Debug.Log(pos.x + "new position");
                Vector3 newBackgroundPos = new Vector3(
                    ((pos.x) + (backgroundwidth/2)), pos.y, zAxis);


                pos = newBackgroundPos;
                transform.position = pos;
                posList.Add(pos); //add current position to previous counter index
            }
            else{
                Vector3 newBackgroundPos = new Vector3(
                    ((pos.x) + (backgroundwidth/2)), pos.y, zAxis);

                pos = newBackgroundPos;
                transform.position = pos;
            }

        }
        else if(playerPos.x - offset < (pos.x -backgroundwidth/2)){
            counter--;
            Vector3 newBackgroundPos = new Vector3(
                ((pos.x) - (backgroundwidth/2)), pos.y, zAxis);
            transform.position = pos;
        }
        

    }
}
