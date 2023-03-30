using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    
    public ShootEffect shootVar;
    
    public bool gunShot = true;
    
   
    public void Start(){

    }

    public void Update(){

      
        if (shootVar != null) {
           
             if (shootVar.isPicked){
              shoot();
             }
        }
     
       
    }

    public void shoot()
    { 

        if (Input.GetKeyDown("f") && gunShot && Input.GetAxis("Horizontal")!=0 )
        {
           
            Instantiate(bullet, transform.position,transform.rotation);
            shootVar.isPicked = false;
           
        }
    }
    
    
}
