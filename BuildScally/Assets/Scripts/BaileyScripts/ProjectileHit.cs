using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileHit : MonoBehaviour
{
    // is the script that the bullet uses when they collide with a object, be it player or enviroment. 
    Rigidbody body;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<MyPlayer>() != null)
        {
            if (!collision.transform.GetComponent<MyPlayer>().GetInvul())
            {
                collision.transform.GetComponent<MyPlayer>().StartCoroutine("InvulFrames", 3f);
                collision.transform.GetComponent<MyPlayer>().hitPoints -= 1;
                collision.transform.position += Vector3.forward * 2;
                Destroy(gameObject);
            }
        }
        Destroy(gameObject);
    }
}
