using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_attack : MonoBehaviour
{
    public Transform attackPointL;
    public Transform attackPointR;

    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    player_movment playermove;
    
    public SpriteRenderer playerSprite;

    public float show;

    public bool turn = false; //false by default player is facing forward (right). 

    // Start is called before the first frame update
    void Start()
    {
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in health_counter
        playerSprite = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        if (playerSprite.flipX==false){
            turn = false;
        }
        else{
            turn = true;
        }

        if(Input.GetKeyDown(KeyCode.J) && !turn ){
            AttackRight();
        }
        else if(Input.GetKeyDown(KeyCode.J) && turn){
            
            AttackLeft();
        }  
    }


    void AttackRight(){
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPointR.position, attackRange,enemyLayers);
        Debug.LogWarning("attacking right...");
        foreach(Collider2D enemy in enemiesHit){
            Debug.Log("Right hit " + enemy.name);
          
            // if (playerSprite.flipX) {
                
           if(playermove.facingRight){
                enemy.GetComponent<enemy>().damageEnemy(1);
            }
        }
    }
     void AttackLeft()
    {
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(attackPointL.position, attackRange,enemyLayers);
        Debug.LogWarning("attacking left...");
        foreach(Collider2D enemy in enemiesHit){
            Debug.Log("Left hit " + enemy.name);  
            // if(playerSprite.flipX==false){
         if(!playermove.facingRight){
                enemy.GetComponent<enemy>().damageEnemy(1);
            }
        }
    }
    void OnDrawGizmosSelected(){
        if(attackPointL == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPointL.position, attackRange);
        if(attackPointR == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPointR.position, attackRange);
    }

}
