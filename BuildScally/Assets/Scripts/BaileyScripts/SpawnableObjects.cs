using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnableObjects : MonoBehaviour
{
    [SerializeField]
    private int score = 0;


    [SerializeField]
    private int spawnRangeMin = 0, spawnRangeMax = 10;

    [SerializeField] private bool isPowerUp;
    [SerializeField] private bool isChest;

    public GameObject Spawner = null;

    public int mass = 5;
    public int orignalWeight;

    public List<PowerUps> powerUps;
    public bool isInside = false;
    public Vector2 returnPoint = Vector2.zero;
    public Transform respawnLocation;
    public bool isCarried = false, wasCarried = false;


    private void Start()
    {
        //Sets weight and mass to the editor set mass
        orignalWeight = mass;
        transform.GetComponent<Rigidbody>().mass = mass;
    }

    private void Update()
    {
        //if the object falls below y:0, destroy the object, if it is a powerup, spawn on original ship or last location picked up
        if (transform.position.y < 0)
        {
            if (!isPowerUp)
            {
                Destroy(gameObject);
                Spawner.GetComponent<Spawner>().spawnLimit += 1;
            }
            else if(respawnLocation != null)
            {
                transform.position = respawnLocation.position;
            }

        }
        
        // spins the treasure when not carried and stops spinning from then on
        transform.GetComponent<Rigidbody>().mass = mass;
        if(!isCarried && !wasCarried)
        transform.Rotate(0,1,0);
        else if(isCarried)
            wasCarried = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        //when no longer carried, turn mass to original mass
        transform.GetComponent<Rigidbody>().mass = mass;
    }
    // to get information from the object
    public int GetScore() { return score; }

    public int GetMin() { return spawnRangeMin;  }
    public int GetMax() { return spawnRangeMax; }

    public bool IsPowerUp() { return isPowerUp; }
    public bool IsChest() { return isChest; }
}
