using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreZones : MonoBehaviour
{
    public GameObject myPlayer;
    int score = 0;
    int highScore = 0;
    float playerMovementSpeed;
    float playerJumpVelocity;
    float modifiedMovementSpeed;
    float modifiedJumpVelocity;
    bool isEmpty = true;

    List<GameObject> collectedTreasure = new List<GameObject>();
    List<GameObject> newTreasure = new List<GameObject>();
    // Initilize the scorezones player and saves their speed and jump velocity
    public void IntializeScoreZone()
    {
        if (myPlayer != null)
        {
            playerMovementSpeed = myPlayer.GetComponent<MyPlayer>().movementSpeed;
            playerJumpVelocity = myPlayer.GetComponent<MyPlayer>().jumpVelocity;
        }
    }

    void Update()
    {
        // gets all the items within the area of the scorezones
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 3);
        newTreasure.Clear();
        foreach (var colliders in hitColliders)
        {
            if (!colliders.isTrigger)
                if (colliders.GetComponent<SpawnableObjects>() != null && !colliders.GetComponent<SpawnableObjects>().IsPowerUp())
                {
                    newTreasure.Add(colliders.gameObject);
                    colliders.GetComponent<SpawnableObjects>().mass = 10000;
                    score += colliders.GetComponent<SpawnableObjects>().GetScore();
                }
                else if (colliders.GetComponent<SpawnableObjects>() != null && colliders.GetComponent<SpawnableObjects>().IsPowerUp())
                {
                    isEmpty = false;
                    modifiedMovementSpeed += colliders.GetComponent<SpawnableObjects>().powerUps[0].speed;
                    modifiedJumpVelocity += colliders.GetComponent<SpawnableObjects>().powerUps[0].jump;
                }
        }

        StatChange();

        isEmpty = true;

        //checks the new value against the previous value and if they are different, will take the new value
        if (score > highScore || score < highScore)
            highScore = score;
        score = 0;
        modifiedMovementSpeed = 0;
        modifiedJumpVelocity = 0;
        bool isGone = false;
        for(int i = 0; i < collectedTreasure.Count; i++)
        {
            foreach(var treasure in newTreasure)
            {
                if(treasure == collectedTreasure[i])
                    isGone = true;

            }
            if (isGone)
                collectedTreasure[i].GetComponent<SpawnableObjects>().mass = collectedTreasure[i].GetComponent<SpawnableObjects>().orignalWeight;
        }

        collectedTreasure = newTreasure;
    }

    public void StatChange()
    {
        //changes the player speed and jump velocity based on powerups in range
        if (!isEmpty)
            myPlayer.GetComponent<MyPlayer>().movementSpeed = playerMovementSpeed + modifiedMovementSpeed;
        else if (isEmpty && myPlayer.GetComponent<MyPlayer>().treasureCarried == null)
            myPlayer.GetComponent<MyPlayer>().movementSpeed = playerMovementSpeed;
        else
            myPlayer.GetComponent<MyPlayer>().movementSpeed = playerMovementSpeed * 0.9f;

        if (!isEmpty)
            myPlayer.GetComponent<MyPlayer>().jumpVelocity = playerJumpVelocity + modifiedJumpVelocity;
        else if (isEmpty && myPlayer.GetComponent<MyPlayer>().treasureCarried == null)
            myPlayer.GetComponent<MyPlayer>().jumpVelocity = playerJumpVelocity;
    }

    // sets their player and gets their score for reference 
    public void SetPlayer(GameObject player) { myPlayer = player; }
    public int  GetScore() { return highScore; }
}
