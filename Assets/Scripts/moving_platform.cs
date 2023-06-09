using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_platform : MonoBehaviour
{
    public float xAxisChange = 0; // The amount the platform moves along the x axis, can be negative
    public float yAxisChange = 0; // The amount the platform moves along the y axis, can be negative
    public float speed = 0; // The speed at which the platform moves
    public float delay = 0; // The time before the platofrm starts moving again

    bool xChangeDone = true; // If the platform has fully moved along the x axis
    bool yChangeDone = true; // If the platform has fully moved along the y axis
    bool waiting = false; // If the platform is currently waiting to move

    float xTransform; // The amount the platform is to transform on the x axis this update
    float yTransform; // The amount the platform is to transform on the y axis this update

    float xDestination; // The x coord the platform is moving to
    float yDestination; // The y coord the platform is moving to

    string xMoving; // The direction the platform is moving to in this cycle
    string yMoving; // The direction the platform is moving to in this cycle

    bool returning = false; // Is the platform rturning to it's original position

    Vector3 initPosition; // Holds the platforms original location

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position; // Sets the platforms original location
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!xChangeDone || !yChangeDone) // If the platform hasn't finished moving
        {
            if (!returning) // If platform is moving to the new location, not returning to original location
            {
                xDestination = initPosition.x + xAxisChange; // It's destination is it's inital location plus the distince it is to move
                yDestination = initPosition.y + yAxisChange; // It's destination is it's inital location plus the distince it is to move

                if (xAxisChange > 0) // If the amount it is to move on the x axis is positive then it is to move right
                {
                    xMoving = "Right";
                }
                else // Else move left
                {
                    xMoving = "Left";
                }

                if (yAxisChange > 0) // If the amount it is to move on the y axis is positive then it is to move up
                {
                    yMoving = "Up";
                }
                else // Else move down
                {
                    yMoving = "Down";
                }
            }
            else // If platform is returning to it's original location, not moving to the new location
            {
                xDestination = initPosition.x; // It's destination is it's inital location
                yDestination = initPosition.y; // It's destination is it's inital location

                if (xAxisChange < 0) // If it moved right otiginaly, move left
                {
                    xMoving = "Right";
                }
                else // If it moved left otiginaly, move right
                {
                    xMoving = "Left";
                }

                if (yAxisChange < 0) // If it moved up originaly, move down
                {
                    yMoving = "Up";
                }
                else // If it moved down otiginaly, move up
                {
                    yMoving = "Down";
                }
            }

            if (transform.position.x < xDestination && xMoving == "Right") // If it is moving right and it is to the left of it's destination, move right
            {
                xTransform = (speed * Time.deltaTime);
            }
            else if (transform.position.x > xDestination && xMoving == "Left") // If it is moving left and it is to the right of it's destination, move left
            {
                xTransform = (-speed * Time.deltaTime);
            }
            else // If it has reached it's destination, mark as such and stop moving
            {
                xChangeDone = true;
                xTransform = 0;
            }

            if (transform.position.y < yDestination && yMoving == "Up") // If it is moving up and it is below of it's destination, move up
            {
                yTransform = (speed * Time.deltaTime);
            }
            else if (transform.position.y > yDestination && yMoving == "Down") // If it is moving down and it is above of it's destination, move down
            {
                yTransform = (-speed * Time.deltaTime);
            }
            else // If it has reached it's destination, mark as such and stop moving
            {
                yChangeDone = true;
                yTransform = 0;
            }


            transform.Translate(xTransform, yTransform, 0); // Move the platform using the just assinged values
        }
        else if (!waiting) // If it has reached it's destination and not yet waiting to move again
        {
            waiting = true; //  Start waiting to move again
            returning = !returning; // Set it to return to it's original location
            StartCoroutine(platformWait()); // Start waiting before moving again
        }
        
    }

    IEnumerator platformWait()
    {
        yield return new WaitForSeconds(delay); // Wait for the specified seconds
        xChangeDone = false; // Set it to no longer be wanted x location
        yChangeDone = false; // Set it to no longer be wanted y location
        waiting = false; // Set it to no longer be waiting

    }
    
    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.transform.parent = transform; // If an object is on the platformer, move with the platformer
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        coll.transform.parent = null; // If an object leaves the platformer, stop moving with the platformer
    }
}
