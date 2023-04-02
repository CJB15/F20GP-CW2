using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_powerups : MonoBehaviour
{

    player_movment playermove; // Used to call function in player_movment
    PlayerShoot playerShoot;
    player_collectable playergem;
    player_health playerHealth;
    ui_powerup uiPowerUp;

    int ability = 0;

    // 0 None
    // 1 DoubleJump
    // 2 FireBall
    // 3 Double Health
    // 4 Double Gems

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<player_health>(); // Used to call function in health_counter
        playermove = GameObject.FindGameObjectWithTag("Player").GetComponent<player_movment>(); // Used to call function in health_counter
        playerShoot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerShoot>(); // Used to call function in health_counter
        playergem = GameObject.FindGameObjectWithTag("Player").GetComponent<player_collectable>(); // Used to call function in health_counter
        uiPowerUp = GameObject.FindGameObjectWithTag("Power Up UI").GetComponent<ui_powerup>(); // Used to call function in health_counter
    }

    public void setNewAbility(int newAbility, SpriteRenderer powerUpSprite)
    {
        switch (ability)
        {
            case 0:
                // Do nothing
                break;

            case 1:
                setDoubleJump(false);
                break;
    
            case 2:
                setFireBall(false);
                break;

            case 3:
                setDoubleHealth(false);
                break;
    
            case 4:
                setDoubleGems(false);
                break;
        }

        ability = newAbility;

        switch (ability)
        {
            case 1:
                setDoubleJump(true);
                uiPowerUp.updatePowerUpUI(powerUpSprite);
                break;
    
            case 2:
                setFireBall(true);
                uiPowerUp.updatePowerUpUI(powerUpSprite);
                break;
            
            case 3:
                setDoubleHealth(true);
                uiPowerUp.updatePowerUpUI(powerUpSprite);
                break;
    
            case 4:
                setDoubleGems(true);
                uiPowerUp.updatePowerUpUI(powerUpSprite);
                break;
    
            default:
                Debug.Log("Not valid Ability");
                ability = 0;
                uiPowerUp.updatePowerUpUI(null);
                break;
        }
    }

    void setDoubleJump(bool active)
    {
        playermove.doubleJumpActive(active);
    }

    void setFireBall(bool active)
    {
        playerShoot.fireBallActive(active);
    }

    void setDoubleHealth(bool active)
    {
        playerHealth.doubleHealthActive(active);
    }

    void setDoubleGems(bool active)
    {
        playergem.doubleGemsctive(active);
    }
}
