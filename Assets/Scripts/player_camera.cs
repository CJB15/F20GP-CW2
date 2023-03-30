using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_camera : MonoBehaviour // This script handes the camera that follows the player
{
    public Transform player; // Used to get the players position

    public float x_coord_from_player = 0; // The distance of the camera from the player, on the x axis
    public float y_coord_from_player = 0.75f; // The distance of the camera from the player, on the y axis, set to 0 to remove camera jank when using anti/normal gravity pad
    public float z_coord_from_player = -5; // The distance of the camera from the player, on the z axis

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = player.transform.position + new Vector3(x_coord_from_player, y_coord_from_player, z_coord_from_player); // Updates the camers location to follow the player
    }

    public void cameraGravitySwitch()
    {
        y_coord_from_player = -y_coord_from_player; // If the camera is above the player, move it below the player when gravity is swapped, dose nothing if y_coord_from_player is 0
    }
}
