using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCollision : MonoBehaviour
{ //causes the player to respawn if they are hit by the wave
   private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<MyPlayer>() != null)
        {
            collision.gameObject.GetComponent<MyPlayer>().Respawn();
        }
    }
}
