using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moving_platform : MonoBehaviour
{
    public float xAxisChange = 0;
    public float yAxisChange = 0;
    public float speed = 0;
    public float delay = 0;

    bool xChangeDone = false;
    bool yChangeDone = false;
    bool waiting = false;

    float xTransform;
    float yTransform;

    float xDestination;
    float yDestination;

    string xMoving;
    string yMoving;

    bool returning = false;

    Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!xChangeDone || !yChangeDone)
        {
            //Debug.Log(returning);
            if (!returning)
            {
                xDestination = initPosition.x + xAxisChange;
                yDestination = initPosition.y + yAxisChange;

                if (xAxisChange > 0)
                {
                    xMoving = "Right";
                }
                else
                {
                    xMoving = "Left";
                }

                if (yAxisChange > 0)
                {
                    yMoving = "Up";
                }
                else
                {
                    yMoving = "Down";
                }
            }
            else
            {
                xDestination = initPosition.x;
                yDestination = initPosition.y;

                if (xAxisChange < 0)
                {
                    xMoving = "Right";
                }
                else
                {
                    xMoving = "Left";
                }

                if (yAxisChange < 0)
                {
                    yMoving = "Up";
                }
                else
                {
                    yMoving = "Down";
                }
            }

            if (transform.position.x < xDestination && xMoving == "Right")
            {
                xTransform = (speed * Time.deltaTime);
            }
            else if (transform.position.x > xDestination && xMoving == "Left")
            {
                xTransform = (-speed * Time.deltaTime);
            }
            else
            {
                xChangeDone = true;
                xTransform = 0;
            }

            if (transform.position.y < yDestination && yMoving == "Up")
            {
                yTransform = (speed * Time.deltaTime);
            }
            else if (transform.position.y > yDestination && yMoving == "Down")
            {
                yTransform = (-speed * Time.deltaTime);
            }
            else
            {
                yChangeDone = true;
                yTransform = 0;
            }
            transform.Translate(xTransform, yTransform, 0);
        }
        else if (!waiting)
        {
            waiting = true;
            returning = !returning;
            StartCoroutine(platformWait());
        }
        
    }

    IEnumerator platformWait()
    {
        yield return new WaitForSeconds(delay);
        xChangeDone = false;
        yChangeDone = false;
        waiting = false;

    }
}
