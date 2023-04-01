using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Bullet Prefab scripting to attach to the bullet powerup. 
public class Bullet : MonoBehaviour
{
    // Variables to hold the rigidbody2d of the object, and the speed of the bullet.
    public float speed = 5f;
    public float decayTime = 5f;
    private Rigidbody2D rb;

    SpriteRenderer SpritePlayer;
    
    // Start is called before the first frame update
    void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        SpritePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>(); // Used to call function in health_counter

        if (SpritePlayer.flipX == true)
        {
            rb.velocity = -transform.right * speed;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            rb.velocity = transform.right * speed;
        }

        StartCoroutine(fireballDecay());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy e1 = collision.GetComponent<enemy>();
        if (e1 != null)
        {
            e1.StartCoroutine(e1.enemyDead());
            Destroy(gameObject);
        }
    }

    public IEnumerator fireballDecay()
    {
        yield return new WaitForSeconds(decayTime);
        Destroy(gameObject);
    }
}
