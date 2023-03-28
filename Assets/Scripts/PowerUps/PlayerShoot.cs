using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    
    public ShootEffect shootVar;
    
    public bool gunShot = true;
    public void Start(){
     //  shootVar = FindObjectOfType<ShootEffect>();

    }

    public void Update(){

       // shootVar = FindObjectOfType<ShootEffect>();
        if (shootVar != null) {
           
             if (shootVar.isPicked){
              shoot();
             }
        }
     
       
    }

    public void shoot()
    { 
        
      //  Debug.Log("hello");

        if (Input.GetKeyDown("f") && gunShot)
        {
           // Debug.Log("hello");
            Instantiate(bullet, transform.position,transform.rotation);
            shootVar.isPicked = false;
            //gunShot = false;
        }
    }
    
    
}
