using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bullet;
    
    //public ShootEffect shootVar;

    bool hasFireBallAbility = false;
    
    bool gunShot = false;

    public float rechargeTime = 1f;
   
    public void Start(){
        
    }

    public void Update(){
        if (Input.GetKey("f") && !gunShot && hasFireBallAbility)
        {
            shoot();
        }
    }

    public void shoot()
    { 
        Instantiate(bullet, transform.position,transform.rotation);
        gunShot = true;
        StartCoroutine(rechargeShot());
    }
    
    public IEnumerator rechargeShot()
    {
        yield return new WaitForSeconds(rechargeTime);
        gunShot = false;
    }

    public void fireBallActive(bool active)
    {
        hasFireBallAbility = active;
    }
    
}
