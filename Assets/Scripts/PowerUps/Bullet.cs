using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Bullet Prefab scripting to attach to the bullet powerup. 
public class Bullet : MonoBehaviour
{
    // Variables to hold the rigidbody2d of the object, and the speed of the bullet.
    public float speed;
    private Rigidbody2D rb;


    
    // Start is called before the first frame update
    void Start()
    {   

        rb = GetComponent<Rigidbody2D>();

        if (Input.GetAxis("Horizontal") < 0 ){
            rb.velocity = -transform.right *speed;
            GetComponent<SpriteRenderer>().flipX = true;

        }
        else if(Input.GetAxis("Horizontal") > 0){
            Debug.Log("right");
            rb.velocity = transform.right *speed;

            
        }

    }






    private void OnTriggerEnter2D(Collider2D collision){
        enemy e1 = collision.GetComponent<enemy>();

        if (e1 != null){
          
              e1.isDead = true;
                Destroy(e1.bodyEnemy);
                Destroy(e1.ColliderEnemy[0]);
                Destroy(e1.ColliderEnemy[1]);
                e1.GetComponent<SpriteRenderer>().flipX = false;
                e1.thisAnim.SetBool("Dead", true);
                e1.StartCoroutine(e1.enemyDead());
        
            
            
            Destroy(gameObject);
       
       
       
        }
    }
}
