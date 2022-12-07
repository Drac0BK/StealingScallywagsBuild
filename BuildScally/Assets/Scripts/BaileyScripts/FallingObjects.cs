using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjects : MonoBehaviour
{
    public Vector3 targetPos;
    public float timer;
    public GameObject fallManager;
    public float speed;
    public GameObject dustEffect;
    GameObject dust;
    public GameObject shadow;
    bool hasHitGround = false;
    bool hasHit = false;

    // will go towards the point of reference(floor) then will delete itself using its manager
    public void GoTowardsPoint()
    {
        if (transform.position.y > targetPos.y)
            transform.position -= transform.up * speed * Time.deltaTime;
        else
        {
            if(!hasHit)
                dust = Instantiate(dustEffect, targetPos, dustEffect.transform.rotation);
            hasHitGround = true;
            if (shadow != null)
                Destroy(shadow);

            timer -= Time.deltaTime;
            if (timer < 0)
            {
                Destroy(dust);
                fallManager.GetComponent<FallingEvent>().DeleteObject(gameObject);
            }
        }
        if(hasHitGround && !hasHit)
        {
            //hits all players in range
            hasHit = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, 4.0f);
            foreach (var collider in colliders)
            {
                if (collider.GetComponent<Rigidbody>() != null)
                    collider.GetComponent<Rigidbody>().AddExplosionForce(20, transform.position, 7.5f, 0.1f, ForceMode.Impulse);
                if (collider.GetComponent<MyPlayer>() != null && collider.GetComponent<MyPlayer>().isHit == false)
                    collider.GetComponent<MyPlayer>().StartCoroutine("InvulFrames", 3f);
            }
        }
    }
}
