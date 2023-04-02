using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/ShootEffect")]




public class ShootEffect : PowerUpEffect
{
  
    public GameObject bullet;
  
    public bool isPicked = false;
    public override void Apply(GameObject player = null)
    { 
     shootFunction();
    }

    public void shootFunction(GameObject character = null)
    { 
        
       isPicked = true;

    
    }
}
